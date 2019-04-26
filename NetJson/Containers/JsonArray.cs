/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System.Collections;
using System.Collections.Generic;

namespace NetJson
{
    /// <summary>
    /// Represents an array in JSON.
    /// </summary>
    public class JsonArray
    {
        private List<object> m_Body;

        /// <summary>
        /// Initializes a new JSON array.
        /// </summary>
        public JsonArray()
        {
            m_Body = new List<object>();
        }
        
        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        public int Length
        {
            get => m_Body.Count;
        }

        /// <summary>
        /// Gets or sets an item in the array.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public object this[int Index]
        {
            get
            {
                if (Index < 0 || Index > m_Body.Count - 1)
                    throw new JsonIndexException(this, "The given index is out of array range.");
                return m_Body[Index];
            }
            set
            {
                if (value != null)
                    if (!(value is string || value is int || value is byte || value is sbyte || value is ushort || value is short || value is float || value is double || value is bool || value is JsonArray || value is JsonObject))
                        throw new JsonTypeException("The given value's type isn't allowed in this container.");
                if (Index < 0 || Index > m_Body.Count - 1)
                    throw new JsonIndexException(this, "The given index is out of array range.");
                m_Body[Index] = value;
            }
        }

        /// <summary>
        /// Adds an item into the array.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void Add(object Item)
        {
            if (Item != null)
                if (!(Item is string || Item is int || Item is byte || Item is sbyte || Item is ushort || Item is short || Item is float || Item is double || Item is bool || Item is JsonArray || Item is JsonObject))
                    throw new JsonTypeException("The given value's type isn't allowed in this container.");
            m_Body.Add(Item);
        }

        /// <summary>
        /// Removes an object from the array by index.
        /// </summary>
        /// <param name="Index">The index.</param>
        public void Remove(int Index)
        {
            if (Index < 0 || Index > m_Body.Count - 1)
                throw new JsonIndexException(this, "The given index is out of array range.");
            m_Body.RemoveAt(Index);
        }

        /// <summary>
        /// Removes an object from the array.
        /// </summary>
        /// <param name="Item">The item.</param>
        public void RemoveItem(object Item)
        {
            if (!m_Body.Contains(Item))
                throw new JsonItemException(this, "The given item doesn't exist in the array.");
            m_Body.Remove(Item);
        }

        /// <summary>
        /// Gets the enumerator of the wrapped list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object> GetEnumerator()
        {
            return m_Body.GetEnumerator();
        }
    }
}