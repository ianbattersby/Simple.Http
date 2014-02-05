// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostedFile.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides a Simple.Http wrapper around an HttpPostedFile from HttpContext
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System.IO;
    using System.Web;

    /// <summary>
    /// Provides a Simple.Http wrapper around an HttpPostedFile from HttpContext
    /// </summary>
    internal class PostedFile : IPostedFile
    {
        private readonly HttpPostedFile file;

        public PostedFile(HttpPostedFile file)
        {
            this.file = file;
        }

        /// <summary>
        /// Gets the fully qualified name of the file on the client.
        /// </summary>
        /// <returns>
        /// The name of the client's file, including the directory path.
        /// </returns>
        public string FileName
        {
            get { return this.file.FileName; }
        }

        /// <summary>
        /// Gets the MIME content type of a file sent by a client.
        /// </summary>
        /// <returns>
        /// The MIME content type of the uploaded file.
        /// </returns>
        public string ContentType
        {
            get { return this.file.ContentType; }
        }

        /// <summary>
        /// Gets the size of an uploaded file, in bytes.
        /// </summary>
        /// <returns>
        /// The file length, in bytes.
        /// </returns>
        public int ContentLength
        {
            get { return this.file.ContentLength; }
        }

        /// <summary>
        /// Gets a <see cref="T:System.IO.Stream"/> object that points to an uploaded file to prepare for reading the contents of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IO.Stream"/> pointing to a file.
        /// </returns>
        public Stream InputStream
        {
            get { return this.file.InputStream; }
        }

        /// <summary>
        /// Saves the contents of an uploaded file.
        /// </summary>
        /// <param name="filename">The name of the saved file. </param><exception cref="T:System.Web.HttpException">The <see cref="P:System.Web.Configuration.HttpRuntimeSection.RequireRootedSaveAsPath"/> property of the <see cref="T:System.Web.Configuration.HttpRuntimeSection"/> object is set to true, but <paramref name="filename"/> is not an absolute path.</exception>
        public void SaveAs(string filename)
        {
            this.file.SaveAs(filename);
        }
    }
}