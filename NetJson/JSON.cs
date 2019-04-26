/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using NetJson.Processing;
using NetJson.Analysis;
using NetJson.Reflection;

namespace NetJson
{
    /// <summary>
    /// The easy-access class.
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// Parses JSON to an object.
        /// </summary>
        /// <param name="JSON">The input.</param>
        /// <returns></returns>
        public static object Parse(string JSON)
        {
            var Reader = new StringReader(JSON);
            var Value = Parser.ParseNext(Reader);
            Reader.Skip(Constants.Whitespace);
            if (!Reader.IsEof())
                throw new JsonParseException(Reader.PositionInfo, "Unexpected extra data at the end.");
            return Value;
        }

        /// <summary>
        /// Turns an object into a JSON string.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="Indent">The optional indenting.</param>
        /// <returns></returns>
        public static string Stringify(object Object, bool Indent = false)
        {
            if (Indent)
                return Serializer.SerializeIndented(Object);
            else
                return Serializer.SerializeType(Object);
        }

        /// <summary>
        /// Parses JSON and assigns the values to any type reflectively.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="JSON">The input.</param>
        /// <returns></returns>
        public static T ParseTo<T>(string JSON)
        {
            return ReflectiveFiller.FillObject<T>((JsonObject)Parse(JSON));
        }

        /// <summary>
        /// Turns an object into a JSON string using reflection.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="Object">The object.</param>
        /// <param name="Indent">The optional indenting.</param>
        /// <returns></returns>
        public static string StringifyFrom<T>(T Object, bool Indent = false)
        {
            var Wrapped = ReflectiveSerializer.Serialize<T>(Object);
            return Stringify(Wrapped, Indent);
        }
    }
}