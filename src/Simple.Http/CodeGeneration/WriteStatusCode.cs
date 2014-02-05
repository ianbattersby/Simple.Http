// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteStatusCode.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the WriteStatusCode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using Protocol;

    internal static class WriteStatusCode
    {
        internal static void Impl(Status status, IContext context)
        {
            context.Response.Status = status;
        }
    }
}