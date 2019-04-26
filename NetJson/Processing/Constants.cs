/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System.Collections.Generic;

namespace NetJson.Processing
{
    /// <summary>
    /// Collection of JSON-parser constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Static unmutable array of whitespace characters.
        /// </summary>
        public static readonly char[] Whitespace = new char[]
        {
            ' ', '\n', '\r', '\t'
        };
        
        /// <summary>
        /// Static unmutable array of decimal characters.
        /// </summary>
        public static readonly char[] Decimal = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Static unmutable array of hexadecimal characters.
        /// </summary>
        public static readonly char[] Hexadecimal = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f',
            'A', 'B', 'C', 'D', 'E', 'F'
        };

        /// <summary>
        /// Static unmutable array of decimal characters prefixed with the negative-sign.
        /// </summary>
        public static readonly string[] NegativeDecimal = new string[]
        {
            "-0", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9"
        };

        /// <summary>
        /// Static unmutable array of JSON one-word values.
        /// </summary>
        public static readonly string[] Words = new string[]
        {
            "true", "false", "null"
        };

        /// <summary>
        /// Static unmutable array of characters that break the reading of a string.
        /// </summary>
        public static readonly char[] StringControl = new char[]
        {
            '"', '\\'
        };
        
        /// <summary>
        /// Static unmutable dictionary of JSON-reserved escape characters. (Unicode escape isn't included here; it's in the parser code)
        /// </summary>
        public static readonly Dictionary<char, char> ReservedEscapes = new Dictionary<char, char>
        {
            { 'n', '\n'  }, // LF (Line feed)
            { 'r', '\r'  }, // CR (Carriage return)
            { 'f', '\f'  }, // Formfeed
            { 'b', '\b'  }, // Backspace
            { 't', '\t'  }, // Tab
            { '\\', '\\' }, // Reverse-solidus
            { '/', '/'   }, // Solidus 
            { '"', '\"'  }  // Double-quote
        };
    }
}