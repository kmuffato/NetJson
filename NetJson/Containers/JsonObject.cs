/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System.Collections.Generic;

namespace NetJson
{
    /// <summary>
    /// Represents an object in JSON.
    /// </summary>
    public class JsonObject
    {
        private Dictionary<string, object> m_Body;

        /// <summary>
        /// Initializes a new JSON object.
        /// </summary>
        public JsonObject()
        {
            m_Body = new Dictionary<string, object>();
        }
        
        /// <summary>
        /// Gets the number of the key-value pairs.
        /// </summary>
        public int Length
        {
            get => m_Body.Count;
        }

        /// <summary>
        /// Gets the dictionary keys.
        /// </summary>
        public string[] Keys
        {
            get
            {
                string[] Keys = new string[m_Body.Count];
                int Index = 0;
                foreach (var Key in m_Body.Keys)
                {
                    Keys[Index] = Key;
                    Index++;
                }
                return Keys;
            }
        }

        /// <summary>
        /// Gets or sets an item in the object.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public object this[string Key]
        {
            get
            {
                if (!m_Body.ContainsKey(Key))
                    throw new JsonKeyException(this, "The key doesn't exist.");
                return m_Body[Key];
            }
            set
            {
                if (value != null)
                    if (!(value is string || value is int || value is byte || value is sbyte || value is ushort || value is short || value is float || value is double || value is bool || value is JsonArray || value is JsonObject))
                        throw new JsonTypeException("The given value's type isn't allowed in this container.");
                m_Body[Key] = value;
            }
        }

        /// <summary>
        /// Gets if a key exists.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public bool Contains(string Key)
        {
            return m_Body.ContainsKey(Key);
        }
        
        /// <summary>
        /// Removes a key from the object.
        /// </summary>
        /// <param name="Key">The key.</param>
        public void Remove(string Key)
        {
            if (!m_Body.ContainsKey(Key))
                throw new JsonKeyException(this, "The key doesn't exist.");
            m_Body.Remove(Key);
        }

        /// <summary>
        /// Gets the enumerator of the wrapped dictionary's keys.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var Key in m_Body.Keys)
                yield return Key;
        }
    }
}