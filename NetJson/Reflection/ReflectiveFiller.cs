/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NetJson.Reflection
{
    /// <summary>
    /// This class handles the reflective filling of classes.
    /// </summary>
    public static class ReflectiveFiller
    {
        /// <summary>
        /// The generics-version of FillObject.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="Object">The desired object.</param>
        /// <returns></returns>
        public static T FillObject<T>(JsonObject Object)
        {
            return (T)FillObject(typeof(T), Object);
        }

        /// <summary>
        /// Checks if the given type is considered a primitive for the filler.
        /// </summary>
        /// <param name="T">The type.</param>
        /// <returns></returns>
        public static bool IsPrimitive(Type T)
        {
            return (T == typeof(string) || T == typeof(byte) || T == typeof(ushort) || T == typeof(SByte) || T == typeof(short) || T == typeof(int) || T == typeof(float) || T == typeof(double) || T == typeof(bool));
        }

        /// <summary>
        /// Unwraps / casts a value of a type into the given field type.
        /// </summary>
        /// <param name="Key">The key name.</param>
        /// <param name="FieldType">The field type.</param>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        public static object UnwrapValue(string Key, Type FieldType, object Value)
        {
            // Null reference
            if (Value == null)
            {
                if (IsPrimitive(FieldType))
                    throw new FieldTypeException(Key, FieldType, typeof(Object), "Type mismatch on '" + Key + "'.");
                return null;
            }

            // Field is enum
            else if (FieldType.IsEnum)
            {
                if (Value == null)
                    return null;
                if (!(Value is string))
                    throw new FieldTypeException(Key, FieldType, typeof(Object), "Type mismatch on '" + Key + "'.");
                return Enum.Parse(FieldType, (string)Value);
            }

            // Primitives
            else if (IsPrimitive(Value.GetType()))
            {
                // Float / fouble
                if (FieldType == typeof(double) && Value is float)
                    return (double)((float)Value);
                else if (FieldType == typeof(float) && Value is double)
                    return (float)((double)Value);

                // Integer types
                else if (FieldType == typeof(byte) && Value is int)
                {
                    if ((int)Value < byte.MinValue || (int)Value > byte.MaxValue)
                        throw new FieldTypeException(Key, FieldType, Value.GetType(), "Size mismatch on '" + Key + "'.");
                    return (byte)((int)Value);
                }
                else if (FieldType == typeof(ushort) && Value is int)
                {
                    if ((int)Value < ushort.MinValue || (int)Value > ushort.MaxValue)
                        throw new FieldTypeException(Key, FieldType, Value.GetType(), "Size mismatch on '" + Key + "'.");
                    return (byte)((ushort)Value);
                }
                else if (FieldType == typeof(SByte) && Value is int)
                {
                    if ((int)Value < SByte.MinValue || (int)Value > SByte.MaxValue)
                        throw new FieldTypeException(Key, FieldType, Value.GetType(), "Size mismatch on '" + Key + "'.");
                    return (byte)((SByte)Value);
                }
                else if (FieldType == typeof(short) && Value is int)
                {
                    if ((int)Value < short.MinValue || (int)Value > short.MaxValue)
                        throw new FieldTypeException(Key, FieldType, Value.GetType(), "Size mismatch on '" + Key + "'.");
                    return (byte)((short)Value);
                }

                // Force direct match
                else
                {
                    if (FieldType != Value.GetType())
                        throw new FieldTypeException(Key, FieldType, Value.GetType(), "Type mismatch on '" + Key + "'.");
                    return Value;
                }
            }

            // Other classes / types
            else if (Value is JsonObject)
            {
                if (IsPrimitive(FieldType))
                    throw new FieldTypeException(Key, FieldType, Value.GetType(), "Type mismatch on '" + Key + "'.");
                return FillObject(FieldType, (JsonObject)Value);
            }

            // Arrays
            else if (Value is JsonArray)
            {
                if (IsPrimitive(FieldType))
                    throw new FieldTypeException(Key, FieldType, Value.GetType(), "Type mismatch on '" + Key + "'.");

                // The field inherits ICollection<>
                if (FieldType.IsArray)
                {
                    var ElementType = FieldType.GetElementType();
                    var Collection = Array.CreateInstance(ElementType, ((JsonArray)Value).Length);
                    
                    // Setting elements
                    int Index = 0;
                    foreach (var Item in (JsonArray)Value)
                    {
                        Collection.SetValue(UnwrapValue(Key + "[" + Index.ToString() + "]", ElementType, Item), Index);
                        Index++;
                    }
                    return Collection;
                }
                else if (FieldType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    var Collection = Activator.CreateInstance(FieldType);
                    var GenericType = FieldType.GetGenericArguments()[0];
                        
                    // Invoking Add(...) method on the Collection<>
                    var Method = FieldType.GetMethod("Add");
                    int Index = 0;
                    foreach (var Item in (JsonArray)Value)
                    {
                        Method.Invoke(Collection, new object[] { UnwrapValue(Key + "[" + Index.ToString() + "]", GenericType, Item) });
                        Index++;
                    }

                    return Collection;
                }
                else
                    throw new JsonTypeException("Unknown container type of '" + Key + "'.");
            }
            else
                throw new JsonTypeException("Invalid type in JSON container.");
        }

        /// <summary>
        /// Fills a JSON object into a class by casting types and creating collections.
        /// </summary>
        /// <param name="T">The desired type.</param>
        /// <param name="Object">The desired object.</param>
        /// <returns></returns>
        public static object FillObject(Type T, JsonObject Object)
        {
            var Instance = FormatterServices.GetSafeUninitializedObject(T);
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
                if (!Object.Contains(Key))
                    throw new JsonKeyException(Object, "The key '" + Key + "' doesn't exist.");

                // Fill in the value based on type in the collection
                Field.SetValue(Instance, UnwrapValue(Key, Field.FieldType, Object[Key]));
            }
            return Instance;
        }
    }
}
