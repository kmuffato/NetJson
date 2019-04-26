/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace NetJson.Reflection
{
    public static class ReflectiveSerializer
    {
        /// <summary>
        /// The generics-version of Serialize.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="Instance">The instance.</param>
        /// <returns></returns>
        public static JsonObject Serialize<T>(T Instance)
        {
            return Serialize(typeof(T), Instance);
        }

        /// <summary>
        /// Wraps a value into NetJson types and containers.
        /// </summary>
        /// <param name="Key">The key name.</param>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        public static object WrapValue(string Key, object Value)
        {
            // Prevent null-reference exceptions
            if (Value == null)
                return null;

            // Is a primitive
            if (ReflectiveFiller.IsPrimitive(Value.GetType()))
                return Value;
            // Is a container
            // Is an array
            else if (Value.GetType().IsArray)
            {
                var Array = new JsonArray();
                int Index = 0;
                foreach (var Item in (Array)Value)
                {
                    Array.Add(WrapValue(Key + "[" + Index.ToString() + "]", Item));
                    Index++;
                }
                return Array;
            }
            // Implements IEnumerable<>
            else if (Value.GetType().GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var Array = new JsonArray();
                var Container = Value as System.Collections.IEnumerable;
                int Index = 0;
                foreach (var Item in Container)
                {
                    Array.Add(WrapValue(Key + "[" + Index.ToString() + "]", Item));
                    Index++;
                }
                return Array;
            }
            // Serializes another child class
            else
                return Serialize(Value.GetType(), Value);
        }
        
        /// <summary>
        /// Fills class fields marked with the JsonField-attribute into a JSON object.
        /// </summary>
        /// <param name="T">The desired type.</param>
        /// <param name="Instance">The instance.</param>
        /// <returns></returns>
        public static JsonObject Serialize(Type T, object Instance)
        {
            if (Instance == null)
                return null;
            var Object = new JsonObject();
            foreach (var Field in T.GetFields())
            {
                // Check if the field is a JSON-field
                var Key = "";
                var IsJson = false;
                foreach (var Attribute in Field.GetCustomAttributes(true))
                {
                    if (Attribute == null)
                        continue;
                    if (Attribute is JsonField JsonField)
                    {
                        Key = JsonField.Name;
                        IsJson = true;
                        break;
                    }
                }
                if (!IsJson)
                    continue;

                // Check if a custom name was set and if the key exists
                if (Key == "")
                    Key = Field.Name;

                Object[Key] = WrapValue(Key, Field.GetValue(Instance));
            }
            return Object;
        }
    }
}