// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Status.Redirect.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the Status type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    public partial struct Status
    {
        // ReSharper disable MemberHidesStaticFromOuterClass
        public static class Redirect
        {
            public static readonly Status NotModified = new Status(304, "Not Modified");

            public static Status MovedPermanently(string location)
            {
                return new Status(301, "Moved Permanently", location);
            }

            public static Status Found(string location)
            {
                return new Status(302, "Found", location);
            }

            public static Status SeeOther(string location)
            {
                return new Status(303, "See Other", location);
            }

            public static Status UseProxy(string location)
            {
                return new Status(305, "Use Proxy", location);
            }

            public static Status TemporaryRedirect(string location)
            {
                return new Status(307, "Temporary Redirect", location);
            }
            // ReSharper restore MemberHidesStaticFromOuterClass
        }
    }
}