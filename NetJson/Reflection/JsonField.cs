/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;

namespace NetJson
{
    /// <summary>
    /// Marks a field as JSON-compatible.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class JsonField : Attribute
    {
        /// <summary>
        /// The original key name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Marks a field as JSON-compatible.
        /// </summary>
        /// <param name="Name">The original key name.</param>
        public JsonField(string Name = "")
        {
            this.Name = Name;
        }
    }
}