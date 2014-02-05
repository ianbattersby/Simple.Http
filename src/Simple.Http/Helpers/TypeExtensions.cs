// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the TypeExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System;

    public static class TypeExtensions
    {
        public static bool IsJsonPrimitive(this Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type.IsEnum
                   || type.IsNullable();
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}