// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disposable.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the Disposable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using System;

    using Simple.Http.Protocol;

    public static class Disposable
    {
        public static void Impl(IDisposable disposable, IContext context)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}