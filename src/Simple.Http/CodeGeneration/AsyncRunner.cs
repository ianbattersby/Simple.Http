// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncRunner.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Runs asynchronous handlers. Should only be used from hosting code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Threading.Tasks;
    using Protocol;

    /// <summary>
    /// Runs asynchronous handlers. Should only be used from hosting code.
    /// </summary>
    public class AsyncRunner
    {
        private readonly Func<object, IContext, Task<Status>> start;
        private readonly Action<object, IContext, Status> end;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRunner"/> class.
        /// </summary>
        /// <param name="start">The start function.</param>
        /// <param name="end">The end action.</param>
        public AsyncRunner(Func<object, IContext, Task<Status>> start, Action<object, IContext, Status> end)
        {
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Gets the end action.
        /// </summary>
        public Action<object, IContext, Status> End
        {
            get { return this.end; }
        }

        /// <summary>
        /// Gets the start function.
        /// </summary>
        public Func<object, IContext, Task<Status>> Start
        {
            get { return this.start; }
        }
    }
}