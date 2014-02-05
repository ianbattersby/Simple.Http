// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILinkBuilder.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the ILinkBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Links
{
    using System.Collections.Generic;

    internal interface ILinkBuilder
    {
        ICollection<Link> LinksForModel(object model);

        Link CanonicalForModel(object model);
    }
}