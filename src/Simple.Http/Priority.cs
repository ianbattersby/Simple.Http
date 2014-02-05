// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Priority.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Enumeration for things which need prioritising, such as Behaviours and URI resolution.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    /// <summary>
    /// Enumeration for things which need prioritising, such as Behaviours and URI resolution.
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// The highest priority. Things here happen first.
        /// </summary>
        Highest = -0x60000000,

        /// <summary>
        /// Higher than high, but not highest.
        /// </summary>
        Higher = -0x40000000,
        
        /// <summary>
        /// High priority.
        /// </summary>
        High = -0x20000000,
        
        /// <summary>
        /// Normal, the default level.
        /// </summary>
        Normal = 0,
        
        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 0x20000000,
        
        /// <summary>
        /// Lower than low, but not lowest.
        /// </summary>
        Lower = 0x40000000,
        
        /// <summary>
        /// The lowest priority. Things here happen last.
        /// </summary>
        Lowest = 0x60000000
    }
}