/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;

namespace NetJson.Processing
{
    /// <summary>
    /// The JSON serializer.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes a JSON object.
        /// </summary>
        /// <param name="Object">The JSON object.</param>
        /// <returns></returns>
        public static string SerializeType(object Object)
        {
            // Prevent null-references
            if (Object == null)
                return "null";
            else if (Object is JsonObject JsonObject)
            {
                string Buffer = "{";

                // Loop through all keys
                var Keys = JsonObject.Keys;
                for (int i = 0; i < Keys.Length; i++)
                {
                    Buffer += SerializeType(Keys[i]) + ":" + SerializeType(JsonObject[Keys[i]]);
                    if (i != JsonObject.Length - 1)
                        Buffer += ",";
                }

                // End the object
                return Buffer + "}";
            }
            else if (Object is JsonArray JsonArray)
            {
                string Buffer = "[";
                
                // Loop through all items
                for (int i = 0; i < JsonArray.Length; i++)
                {
                    Buffer += SerializeType(JsonArray[i]);
                    if (i != JsonArray.Length - 1)
                        Buffer += ",";
                }

                // End the object
                return Buffer + "]";
            }
            else if (Object is string String)
            {
                // Loop though characters and using escaped versions of reserved or unicode characters
                string Buffer = "\"";
                foreach (var Character in String)
                {
                    bool IsReserved = false;

                    // Check for reserved characters, except of the solidus.
                    if (Character != '/')
                    {
                        foreach (var Reserve in Constants.ReservedEscapes)
                        {
                            if (Reserve.Value == Character)
                            {
                                Buffer += "\\" + Reserve.Key;
                                IsReserved = true;
                                break;
                            }
                        }
                    }

                    // Check for character range
                    if (!IsReserved)
                    {
                        int Code = (int)Character;

                        // Out of meaningful ASCII range
                        if (Code < 32 || Code > 126)
                            Buffer += "\\u" + Code.ToString("X4");
                        else
                            Buffer += Character;
                    }
                }

                // End string and return;
                return Buffer + "\"";
            }
            else if (Object is float || Object is double || Object is int || Object is byte || Object is sbyte || Object is ushort || Object is short)
            {
                // .NET likes to use the regional decimal character.
                if (Object is float Float)        { return Float.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is double Double) { return Double.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is int Int)       { return Int.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is byte Byte)     { return Byte.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is sbyte SByte)   { return SByte.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is ushort UShort) { return UShort.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is short Short)   { return Short.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else
                    throw new JsonTypeException("The type '" + Object.GetType().Name + "' can't be serialized.");
            }
            else if (Object is bool Bool)
                return (bool)Object ? "true" : "false";
            else
                throw new JsonTypeException("The type '" + Object.GetType().Name + "' can't be serialized.");
        }

        /// <summary>
        /// Serializes a JSON object and indents it to look nice.
        /// </summary>
        /// <param name="Object">The JSON object.</param>
        /// <param name="Indent">The beginning indent level.</param>
        /// <returns></returns>
        public static string SerializeIndented(object Object, int Indent = 0)
        {
            // Prevent null-references
            if (Object == null)
                return "null";

            else if (Object is JsonObject JsonObject)
            {
                // Don't indent for empty containers
                if (JsonObject.Length == 0)
                    return "{}";

                // Loop through all keys, indenting them
                string Buffer = "{\n"; 
                var Keys = JsonObject.Keys;
                for (int i = 0; i < Keys.Length; i++)
                {
                    Buffer += ((Indent < 0) ? "" : new string(' ', (Indent + 1) * 4)) + SerializeIndented(Keys[i], Indent + 1) + ": " + SerializeIndented(JsonObject[Keys[i]], Indent + 1);
                    if (i != JsonObject.Length - 1)
                        Buffer += ",\n";
                }

                // End the object, indenting to the previous indent level
                return Buffer + "\n" + new string(' ', (Indent) * 4) + "}";
            }
            else if (Object is JsonArray JsonArray)
            {
                // Don't indent for empty containers
                if (JsonArray.Length == 0)
                    return "[]";
                
                // Loop through all items, indenting them
                string Buffer = "[\n";
                for (int i = 0; i < JsonArray.Length; i++)
                {
                    Buffer += ((Indent < 0) ? "" : new string(' ', (Indent + 1) * 4)) + SerializeIndented(JsonArray[i], Indent + 1);
                    if (i != JsonArray.Length - 1)
                        Buffer += "," + ((Indent < 0) ? "" : "\n");
                }

                // End the array, indenting to the previous indent level
                return Buffer + "\n" + new string(' ', (Indent) * 4) + "]";
            }
            else if (Object is string String)
            {
                // Loop though characters and using escaped versions of reserved or unicode characters
                string Buffer = "\"";
                foreach (var Character in String)
                {
                    bool IsReserved = false;

                    // Check for reserved characters, except of the solidus.
                    if (Character != '/')
                    {
                        foreach (var Reserve in Constants.ReservedEscapes)
                        {
                            if (Reserve.Value == Character)
                            {
                                Buffer += "\\" + Reserve.Key;
                                IsReserved = true;
                                break;
                            }
                        }
                    }

                    // Check for character range
                    if (!IsReserved)
                    {
                        int Code = (int)Character;

                        // Out of meaningful ASCII range
                        if (Code < 32 || Code > 126)
                            Buffer += "\\u" + Code.ToString("X4");
                        else
                            Buffer += Character;
                    }
                }

                // End string and return;
                return Buffer + "\"";
            }
            else if (Object is float || Object is double || Object is int || Object is byte || Object is sbyte || Object is ushort || Object is short)
            {
                // .NET likes to use the regional decimal character.
                if (Object is float Float)        { return Float.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is double Double) { return Double.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is int Int)       { return Int.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is byte Byte)     { return Byte.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is sbyte SByte)   { return SByte.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is ushort UShort) { return UShort.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else if (Object is short Short)   { return Short.ToString(System.Globalization.CultureInfo.InvariantCulture); }
                else
                    throw new JsonTypeException("The type '" + Object.GetType().Name + "' can't be serialized.");
            }
            else if (Object is bool Bool)
                return (bool)Object ? "true" : "false";
            else
                throw new JsonTypeException("The type '" + Object.GetType().Name + "' can't be serialized.");
        }
    }
}