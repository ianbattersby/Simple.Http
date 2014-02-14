// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PipelineFunctionFactory.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the PipelineFunctionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using Simple.Http.Behaviors.Implementations;
    using Simple.Http.Hosting;
    using Simple.Http.Protocol;

    internal class PipelineFunctionFactory
    {
        private static readonly IDictionary<Type, IDictionary<string, Func<IContext, HandlerInfo, Task>>> RunMethodCache =
                                    new Dictionary<Type, IDictionary<string, Func<IContext, HandlerInfo, Task>>>();

        private static readonly ICollection RunMethodCacheCollection;

        private readonly Type handlerType;
        private readonly ParameterExpression context;
        private readonly ParameterExpression scopedHandler;
        private readonly ParameterExpression handler;
        private readonly ParameterExpression handlerInfoVariable;

        static PipelineFunctionFactory()
        {
            var cache = new Dictionary<Type, IDictionary<string, Func<IContext, HandlerInfo, Task>>>();

            RunMethodCache = cache;
            RunMethodCacheCollection = cache;
        }

        public PipelineFunctionFactory(Type handlerType)
        {
            this.handlerType = handlerType;
            //this.context = Expression.Parameter(typeof(IContext), "context");
            //this.scopedHandler = Expression.Variable(typeof(IScopedHandler), "scopedHandler");
            //this.handler = Expression.Variable(this.handlerType, "handlerType");
            //this.handlerInfoVariable = Expression.Variable(typeof(HandlerInfo), "handlerInfo");
        }

        public static Func<IContext, HandlerInfo, Task> Get(Type handlerType, string httpMethod)
        {
            IDictionary<string, Func<IContext, HandlerInfo, Task>> handlerCache;

            if (!RunMethodCache.TryGetValue(handlerType, out handlerCache))
            {
                lock (RunMethodCacheCollection.SyncRoot)
                {
                    if (!RunMethodCache.TryGetValue(handlerType, out handlerCache))
                    {
                        var asyncRunMethod = new PipelineFunctionFactory(handlerType).BuildAsyncRunMethod(httpMethod);
                        
                        handlerCache = new Dictionary<string, Func<IContext, HandlerInfo, Task>>
                            {
                                { httpMethod, asyncRunMethod }
                            };

                        RunMethodCache.Add(handlerType, handlerCache);
                        
                        return asyncRunMethod;
                    }
                }
            }

            // It's not really worth all the locking palaver here,
            // worst case scenario the AsyncRunMethod gets built more than once.
            Func<IContext, HandlerInfo, Task> method;

            if (!handlerCache.TryGetValue(httpMethod, out method))
            {
                method = new PipelineFunctionFactory(handlerType).BuildAsyncRunMethod(httpMethod);
                handlerCache[httpMethod] = method;
            }

            return method;
        }

        /// <summary>
        /// Generates a compiled method to run a Handler.
        /// </summary>
        /// <returns>A compiled delegate to run the Handler asynchronously.</returns>
        public Func<IContext, HandlerInfo, Task> BuildAsyncRunMethod(string httpMethod)
        {
            var blocks = new List<object>();

            blocks.AddRange(CreateBlocks(this.GetSetupBehaviorInfos()));

            var second = new HandlerBlock(this.handlerType, this.GetRunMethod(httpMethod));

            blocks.Add(second);

            var redirectBehavior = new ResponseBehaviorInfo(typeof(object), typeof(Redirect2), Priority.High) { Universal = true };

            blocks.AddRange(CreateBlocks(this.GetResponseBehaviorInfos(redirectBehavior)));

            var outputs = this.GetOutputBehaviorInfos().ToList();

            if (outputs.Count > 0)
            {
                blocks.AddRange(CreateBlocks(outputs));
            }
            else
            {
                var writeViewBlock = new PipelineBlock();
                writeViewBlock.Add(typeof(WriteView).GetMethod("Impl", BindingFlags.Static | BindingFlags.Public));
                blocks.Add(writeViewBlock);
            }

            if (typeof(IDisposable).IsAssignableFrom(this.handlerType))
            {
                var disposeBlock = new PipelineBlock();
                disposeBlock.Add(typeof(Disposable).GetMethod("Impl", BindingFlags.Static | BindingFlags.Public));
                blocks.Add(disposeBlock);
            }

            var call = this.BuildCallExpression(blocks);

            var createHandler = this.BuildCreateHandlerExpression();

            var lambdaBlock = Expression.Block(new[] { this.handler }, new[] { createHandler, call });

            var lambda = Expression.Lambda(lambdaBlock, this.context, this.handlerInfoVariable);
            return (Func<IContext, HandlerInfo, Task>)lambda.Compile();
        }

        private Expression BuildCreateHandlerExpression()
        {
            var factory = Expression.Constant(HandlerFactory.Instance);
            var createScopedHandler = Expression.Assign(this.scopedHandler, Expression.Call(factory, HandlerFactory.GetHandlerMethod, this.handlerInfoVariable));
            var assignHandler = Expression.Assign(this.handler, Expression.Convert(Expression.Property(this.scopedHandler, "Handler"), this.handlerType));

            return Expression.Block(
                new[] { this.scopedHandler },
                new Expression[] { createScopedHandler, assignHandler });
        }

        private static IEnumerable<PipelineBlock> CreateBlocks(IEnumerable<BehaviorInfo> behaviorInfos)
        {
            var pipelineBlock = new PipelineBlock();

            foreach (var behaviorInfo in behaviorInfos)
            {
                var method = behaviorInfo.GetMethod();

                pipelineBlock.Add(method);

                if (method.ReturnType != typeof(void))
                {
                    yield return pipelineBlock;
                    pipelineBlock = new PipelineBlock();
                }
            }

            if (pipelineBlock.Any)
            {
                yield return pipelineBlock;
            }
        }

        private Expression BuildCallExpression(IEnumerable<object> blocks)
        {
            var expressions = new List<Expression>();

            var typeTcs = typeof(TaskCompletionSource<>).MakeGenericType(typeof(bool));
            var varTcs = Expression.Variable(typeTcs, "tcs");
            var varHandlerType = Expression.Variable(typeof(Type), "handlerType");
            var varContext = Expression.Variable(typeof(IContext), "context");

            expressions.AddRange(
                new[]
                    {
                        Expression.Assign(varTcs, Expression.New(typeTcs)),
                        Expression.Assign(varHandlerType, Expression.Constant(this.handlerType)),
                        Expression.Assign(varContext, this.context)
                    });

            foreach (var block in blocks)
            {
                if (block is HandlerBlock)
                {
                    expressions.Add((block as HandlerBlock).Generate());
                }
                else if (block is PipelineBlock)
                {
                    expressions.AddRange((block as PipelineBlock).Generate(varHandlerType, varContext));
                }
            }

            return Expression.Block(new[] { varTcs, varHandlerType, varContext }, expressions);
        }

        //private Expression BuildCallHandlerExpression(object block)
        //{
        //    var handlerBlock = (HandlerBlock)block;
        //    var runMethod = handlerBlock.Generate();

        //    return Expression.Call(
        //        AsyncPipeline.ContinueWithHandlerMethod(this.handlerType),
        //        call,
        //        Expression.Constant(runMethod),
        //        this.context,
        //        this.handler);
        //}

        private MethodInfo GetRunMethod(string httpMethod)
        {
            return this.handlerType.GetInterfaces()
                               .Where(HttpMethodAttribute.IsAppliedTo)
                               .Select(t => HttpMethodAttribute.GetMethod(t, httpMethod))
                               .Single(a => a != null);
        }

        private IEnumerable<BehaviorInfo> GetSetupBehaviorInfos()
        {
            // ReSharper disable once ConvertClosureToMethodGroup
            return RequestBehaviorInfo.GetInPriorityOrder().Where(x => this.HandlerHasBehavior(x)).ToArray();
        }

        private IEnumerable<BehaviorInfo> GetResponseBehaviorInfos(params ResponseBehaviorInfo[] defaults)
        {
            return ResponseBehaviorInfo.GetInPriorityOrder(defaults).Where(b => b.Universal || this.HandlerHasBehavior(b));
        }

        private IEnumerable<BehaviorInfo> GetOutputBehaviorInfos()
        {
            return OutputBehaviorInfo.GetInPriorityOrder().Where(this.HandlerHasBehavior);
        }

        private bool HandlerHasBehavior(BehaviorInfo behaviorInfo)
        {
            if (behaviorInfo.BehaviorType.IsGenericType)
            {
                if (this.handlerType.GetInterface(behaviorInfo.BehaviorType.FullName) != null)
                {
                    return true;
                }
            }

            return behaviorInfo.BehaviorType.IsAssignableFrom(this.handlerType);
        }
    }
}