// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingTableBuilder.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Factory class for building routing tables.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Simple.Http.Helpers;
    using Simple.Http.Hosting;

    /// <summary>
    /// Factory class for building routing tables.
    /// </summary>
    internal sealed class RoutingTableBuilder
    {
        private readonly string hostedPath;

        private readonly IList<Type> handlerBaseTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingTableBuilder"/> class.
        /// </summary>
        /// <param name="hostedPath">HostedPath which the host is bound to and UriTemplate suffix.</param>
        /// <param name="handlerBaseTypes">The handler base types.</param>
        public RoutingTableBuilder(string hostedPath, params Type[] handlerBaseTypes)
        {
            this.hostedPath = (hostedPath ?? string.Empty).Trim('/');
            this.handlerBaseTypes = handlerBaseTypes;
        }

        /// <summary>
        /// Builds the routing table.
        /// </summary>
        /// <returns>The routing table for the provided base types.</returns>
        public RoutingTable BuildRoutingTable()
        {
            var routingTable = new RoutingTable();

            this.PopulateRoutingTableWithHandlers(routingTable);

            return routingTable;
        }

        private void PopulateRoutingTableWithHandlers(RoutingTable routingTable)
        {
            PopulateRoutingTableWithHandlers(routingTable, ExportedTypeHelper.FromCurrentAppDomain(this.TypeIsHandler), this.hostedPath);
        }

        private static void PopulateRoutingTableWithHandlers(RoutingTable routingTable, IEnumerable<Type> handlerTypes, string hostedPath)
        {
            foreach (var exportedType in handlerTypes)
            {
                foreach (var uriTemplate in UriTemplateAttribute.GetAllTemplates(exportedType).Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => string.Format("/{0}/{1}", hostedPath, u.TrimStart('/'))))
                {
                    if (exportedType.IsGenericTypeDefinition)
                    {
                        BuildRoutesForGenericHandlerType(routingTable, exportedType, uriTemplate);
                    }
                    else
                    {
                        routingTable.Add(uriTemplate, new HandlerTypeInfo(exportedType));
                    }
                }
            }
        }

        private static void BuildRoutesForGenericHandlerType(
            RoutingTable routingTable,
            Type exportedType,
            string uriTemplate)
        {
            var genericArgument = exportedType.GetGenericArguments().Single();
            var genericParameterAttributes = genericArgument.GenericParameterAttributes &
                                             GenericParameterAttributes.SpecialConstraintMask;
            var constraints = genericArgument.GetGenericParameterConstraints();
            var templatePart = "{" + genericArgument.Name + "}";

            if (!uriTemplate.Contains(templatePart))
            {
                return;
            }

            var genericResolver =
                Attribute.GetCustomAttribute(exportedType, typeof(GenericResolverAttribute)) as
                GenericResolverAttribute;

            IEnumerable<Type> candidateTypes;
            Func<Type, IEnumerable<string>> getNames;

            if (genericResolver != null)
            {
                candidateTypes = genericResolver.GetTypes();
                getNames = genericResolver.GetNames;
            }
            else
            {
                candidateTypes = ExportedTypeHelper.FromCurrentAppDomain(t => true);
                getNames = t => new[] { t.Name };
            }

            foreach (var validType in candidateTypes)
            {
                if (!MatchesConstraints(genericParameterAttributes, constraints, validType))
                {
                    continue;
                }

                foreach (var templateName in getNames(validType))
                {
                    var withTemplate = uriTemplate.Replace(templatePart, templateName);

                    routingTable.Add(
                        withTemplate,
                        new HandlerTypeInfo(exportedType.MakeGenericType(validType)));
                }
            }
        }

        private static bool MatchesConstraints(GenericParameterAttributes attributes, Type[] constraints, Type target)
        {
            if (constraints.Length == 0 && attributes == GenericParameterAttributes.None)
            {
                return true;
            }

            for (var i = 0; i < constraints.Length; i++)
            {
                if (!constraints[i].IsAssignableFrom(target))
                {
                    return false;
                }
            }

            if (attributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
            {
                if (target.GetConstructor(new Type[0]) == null)
                {
                    return false;
                }
            }

            if (attributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
            {
                if (!(target.IsClass || target.IsInterface))
                {
                    return false;
                }
            }

            if (attributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
            {
                if (!(target.IsValueType && !target.IsNullable()))
                {
                    return false;
                }
            }

            return true;
        }

        private bool TypeIsHandler(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return false;
            }

            return this.handlerBaseTypes.Any(t => t.IsAssignableFrom(type));
        }

        internal RoutingTable BuildRoutingTable(IEnumerable<Type> handlerTypes)
        {
            var routingTable = new RoutingTable();

            PopulateRoutingTableWithHandlers(routingTable, handlerTypes, this.hostedPath);

            return routingTable;
        }
    }
}