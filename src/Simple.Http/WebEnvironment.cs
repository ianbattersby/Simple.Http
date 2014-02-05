// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebEnvironment.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Default implementation of <see cref="IWebEnvironment" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Helpers;

    /// <summary>
    /// Default implementation of <see cref="IWebEnvironment"/>
    /// </summary>
    public sealed class WebEnvironment : IWebEnvironment
    {
        private static readonly IDictionary<string, string[]> ContentTypeLookup = new Dictionary<string, string[]>
                                                                                    {
                                                                                        { ".css", new[] { "text/css" } },
                                                                                        { ".js", new[] { "application/javascript", "text/javascript" } },
                                                                                    };

        private static readonly string BinBasedAppRoot =
            Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetPath()));

        private IPathUtility pathUtility;

        private readonly IFileUtility fileUtility = new FileUtility();

        /// <summary>
        /// Gets the root folder of the application in the host.
        /// </summary>
        public string AppRoot
        {
            get { return BinBasedAppRoot; }
        }

        /// <summary>
        /// Gets the path utility.
        /// </summary>
        public IPathUtility PathUtility
        {
            get
            {
                return this.pathUtility ??
                       (this.pathUtility =
                        ExportedTypeHelper.FromCurrentAppDomain(t => typeof(IPathUtility).IsAssignableFrom(t))
                            .Where(t => !(t.IsInterface || t.IsAbstract))
                            .Select(Activator.CreateInstance).Cast<IPathUtility>().FirstOrDefault());
            }
        }

        /// <summary>
        /// Gets the file utility.
        /// </summary>
        public IFileUtility FileUtility
        {
            get { return this.fileUtility; }
        }

        /// <summary>
        /// Gets the content type from a file extension.
        /// </summary>
        /// <param name="file">The filename.</param>
        /// <param name="acceptedTypes">The accepted types.</param>
        /// <returns>
        /// The acceptable type for the file.
        /// </returns>
        public string GetMediaTypeFromFileExtension(string file, IList<string> acceptedTypes)
        {
            var extension = Path.GetExtension(file);

            if (string.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            return ContentTypeLookup.ContainsKey(extension)
                       ? ContentTypeLookup[extension].FirstOrDefault(acceptedTypes.Contains)
                       : null;
        }
    }
}