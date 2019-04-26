/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Linq;
using NetJson.Analysis;

namespace NetJson.Processing
{
    /// <summary>
    /// The main parser class.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Gets or sets if the range \uD800 ~ \uDFFF (UTF-16 reserved chars) produces an exception or is simply ignored.
        /// </summary>
        public static bool IgnoreUnicodeD800
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Gets or sets the maximum amount of nested elements.
        /// </summary>
        public static int NestedElementLimit
        {
            get;
            set;
        } = 127;

        /// <summary>
        /// Parses the next number.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <returns></returns>
        public static object ParseNumber(StringReader Reader)
        {
            // Skip prepending whitespace
            Reader.Skip(Constants.Whitespace);

            // Remember beginning position
            var Begin = Reader.PositionInfo;

            if (Reader.IsEof())
                throw new JsonParseException(Begin, "Unexpected end while parsing number.");

            // Collect number information
            bool IsNegative = Reader.Peek(Constants.NegativeDecimal);
            bool IsFloating = false;
            if (IsNegative)
                Reader.Skip(1);

            string Number = Reader.ReadUntilNot(Constants.Decimal);

            // Floating-point
            if (Reader.IsNext('.'))
            {
                Number += ".";
                if (Reader.IsEof())
                    throw new JsonParseException(Reader.PositionInfo, "Unexpected end while parsing floating-point number.");

                IsFloating = true;
                var Scan0 = Reader.ReadUntilNot(Constants.Decimal);
                if (Scan0.Length == 0)
                    throw new JsonParseException(Reader.PositionInfo, "Unexpected end of number after decimal sign.");

                Number += Scan0;
            }

            // Check for leading zero
            if (Number.StartsWith("0") && !Number.StartsWith("0.") && Number != "0" && Number != "-0")
                throw new JsonParseException(Begin, "Leading zero on number begin.");

            // Exponent sign
            if (Reader.Peek("E") || Reader.Peek("e"))
            {
                Number += Reader.Read(1);
                IsFloating = true;

                if (Reader.IsEof())
                    throw new JsonParseException(Reader.PositionInfo, "Unexpected end while parsing floating-point exponent sign.");

                if (Reader.Peek('+') || Reader.Peek('-'))
                    Number += Reader.Read(1);

                if (Reader.IsEof())
                    throw new JsonParseException(Reader.PositionInfo, "Unexpected end while parsing floating-point exponent number.");
                if (!Reader.Peek(Constants.Decimal))
                    throw new JsonParseException(Reader.PositionInfo, "Invalid exponent sign while parsing floating-point number.");

                Number += Reader.ReadUntilNot(Constants.Decimal);
            }

            // Parse number and apply negativity
            try
            {
                if (IsFloating)
                    return IsNegative ? -double.Parse(Number, System.Globalization.CultureInfo.InvariantCulture) : double.Parse(Number, System.Globalization.CultureInfo.InvariantCulture);
                else
                    return IsNegative ? -int.Parse(Number, System.Globalization.CultureInfo.InvariantCulture) : int.Parse(Number, System.Globalization.CultureInfo.InvariantCulture);
            } catch (FormatException)
            {
                throw new JsonParseException(Begin, "Number format error while parsing number.");
            } catch (OverflowException)
            {
                throw new JsonParseException(Begin, "Overflow error while parsing number.");
            }
        }

        /// <summary>
        /// Parses the next string considering reserved and unicode escapes.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <returns></returns>
        public static object ParseString(StringReader Reader)
        {
            // Skip prepending whitespace
            Reader.Skip(Constants.Whitespace);

            // Check for unexpected end
            if (Reader.IsEof())
                throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting string opening.");

            // Check and skip the string opening
            if (!Reader.IsNext('"'))
                throw new JsonParseException(Reader.PositionInfo, "Unexpected character while expecting string opening.");

            // Read string until terminated
            string Buffer = "";
            while (!Reader.IsEof())
            {
                string Scan0 = Reader.ReadUntil(Constants.StringControl);
                foreach (var Character in Scan0)
                    if ((int)Character < 32)
                        throw new JsonParseException(Reader.PositionInfo, "Unescaped control character.");
                Buffer += Scan0;
                if (Reader.IsNext('"'))
                    return Buffer;
                else if (Reader.IsNext('\\'))
                {
                    if (Reader.IsEof())
                        throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting escape character.");

                    char Escape = Reader.Read(1)[0];
                    if (Constants.ReservedEscapes.ContainsKey(Escape))
                    {
                        Buffer += Constants.ReservedEscapes[Escape];
                    } else if (Escape == 'u')
                    {
                        // Remember position
                        var Position = Reader.PositionInfo;

                        // Read unicode and check ending
                        string Unicode = Reader.Read(4);
                        if (Reader.IsEof() && Unicode.Length < 4)
                            throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting unicode escape character.");
                        if (Reader.IsEof())
                            throw new JsonParseException(Reader.PositionInfo, "String wasn't ended correctly.");

                        // Check code length
                        if (Unicode.Length != 4)
                            throw new JsonParseException(Position, "Invalid unicode escape length.");

                        // Check hex characters
                        foreach (var Character in Unicode)
                            if (!Constants.Hexadecimal.Contains(Character))
                                throw new JsonParseException(Position, "Invalid unicode escape number.");

                        // Check for \uD800 ~ \uDFFF
                        var Code = int.Parse(Unicode, System.Globalization.NumberStyles.HexNumber);
                        if (Code > 0xD7FF && Code < 0xE000)
                        {
                            if (IgnoreUnicodeD800)
                                continue;
                            throw new JsonParseException(Position, "Invalid unicode range. (D800 ~ DFFF is reserved for UTF-16)");
                        }

                        // Append character
                        Buffer += char.ConvertFromUtf32(Code);
                    }
                    else
                        throw new JsonParseException(Reader.PositionInfo, "Unexpected escape character while parsing string.");
                }
                else
                    throw new JsonParseException(Reader.PositionInfo, "Unexpected string control character while parsing string.");
            }

            // The loop broke, this isn't allowed because the string didn't end correctly
            throw new JsonParseException(Reader.PositionInfo, "String wasn't ended correctly.");
        }

        /// <summary>
        /// Parses the next word.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <returns></returns>
        public static object ParseWord(StringReader Reader)
        {
            // Skip prepending whitespace
            Reader.Skip(Constants.Whitespace);

            // Check for unexpected end
            if (Reader.IsEof())
                throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting word.");

            // Check for words
            if (Reader.IsNext("true"))
                return true;
            else if (Reader.IsNext("false"))
                return false;
            else if (Reader.IsNext("null"))
                return null;
            else
                throw new JsonParseException(Reader.PositionInfo, "Unexpected word.");
        }

        /// <summary>
        /// Parses the next JSON object.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <param name="Depth">The current recursion depth.</param>
        /// <returns></returns>
        public static object ParseObject(StringReader Reader, int Depth = 0)
        {
            // Recursion depth limit
            if (Depth > NestedElementLimit)
                throw new JsonParseException(Reader.PositionInfo, "Maximum nesting depth (" + NestedElementLimit.ToString() + ") exceeded.");

            Reader.Skip(Constants.Whitespace);
            if (!Reader.IsNext('{'))
                throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting object opening.");
            var Object = new JsonObject();
            Reader.Skip(Constants.Whitespace);
            if (Reader.IsNext('}'))
                return Object;

            while (!Reader.IsEof())
            {
                // Read key
                Reader.Skip(Constants.Whitespace);
                string Key = ParseString(Reader) as string;

                // Check for ':'
                Reader.Skip(Constants.Whitespace);
                if (!Reader.IsNext(':'))
                    throw new JsonParseException(Reader.PositionInfo, "Invalid character. (expected ':')");
                Reader.Skip(Constants.Whitespace);

                // Read value
                var Value = ParseNext(Reader, Depth + 1);
                Object[Key] = Value;

                // Check how the object continues
                Reader.Skip(Constants.Whitespace);
                if (Reader.IsNext(','))
                    continue;
                else if (Reader.IsNext('}'))
                    return Object;
                else
                    throw new JsonParseException(Reader.PositionInfo, "Invalid character. (expected ',' or '}')");
            }

            throw new JsonParseException(Reader.PositionInfo, "Unexpected end while parsing object.");
        }

        /// <summary>
        /// Parses the next JSON array.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <param name="Depth">The current recursion depth.</param>
        /// <returns></returns>
        public static object ParseArray(StringReader Reader, int Depth = 0)
        {
            // Recursion depth limit
            if (Depth > NestedElementLimit)
                throw new JsonParseException(Reader.PositionInfo, "Maximum nesting depth (" + NestedElementLimit.ToString() + ") exceeded.");

            Reader.Skip(Constants.Whitespace);
            if (!Reader.IsNext('['))
                throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting array opening.");
            var Array = new JsonArray();
            Reader.Skip(Constants.Whitespace);
            if (Reader.IsNext(']'))
                return Array;

            while (!Reader.IsEof())
            {
                Reader.Skip(Constants.Whitespace);

                // Read value
                var Value = ParseNext(Reader, Depth + 1);
                Array.Add(Value);

                // Check how the array continues
                Reader.Skip(Constants.Whitespace);
                if (Reader.IsNext(','))
                    continue;
                else if (Reader.IsNext(']'))
                    return Array;
                else
                    throw new JsonParseException(Reader.PositionInfo, "Invalid character. (expected ',' or ']')");
            }

            throw new JsonParseException(Reader.PositionInfo, "Unexpected end while parsing array.");
        }

        /// <summary>
        /// Parses the next value.
        /// </summary>
        /// <param name="Reader">The string reader.</param>
        /// <param name="Depth">The current recursion depth.</param>
        /// <returns></returns>
        public static object ParseNext(StringReader Reader, int Depth = 0)
        {
            if (Reader.IsEof())
                throw new JsonParseException(Reader.PositionInfo, "Unexpected end while expecting next value.");
            Reader.Skip(Constants.Whitespace);
            if (Reader.Peek(Constants.Words))
                return ParseWord(Reader);
            else if (Reader.Peek('"'))
                return ParseString(Reader);
            else if (Reader.Peek(Constants.NegativeDecimal) || Reader.Peek(Constants.Decimal))
                return ParseNumber(Reader);
            else if (Reader.Peek('{'))
                return ParseObject(Reader, Depth);
            else if (Reader.Peek('['))
                return ParseArray(Reader, Depth);
            else
                throw new JsonParseException(Reader.PositionInfo, "Unable to determine next value type.");
        }
    }
}