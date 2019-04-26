/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace NetJson.Analysis
{
    /// <summary>
    /// Helper-class to sequentially read and parse a string.
    /// </summary>
    public class StringReader
    {
        #region "Properties"
        /// <summary>
        /// This gets the full string.
        /// </summary>
        public string Data
        {
            get;
            private set;
        }

        /// <summary>
        /// This gets the current position.
        /// </summary>
        public int Position
        {
            get;
            private set;
        }

        /// <summary>
        /// This gets the current row.
        /// </summary>
        public int Row
        {
            get
            {
                int CurrentRow = 1;
                for (int i = 0; i < Position; i++)
                    if (Data[i] == '\n')
                        CurrentRow++;
                return CurrentRow;
            }
        }

        /// <summary>
        /// This gets the current column.
        /// </summary>
        public int Column
        {
            get
            {
                int CurrentColumn = 1;
                for (int i = 0; i < Position; i++)
                {
                    if (Data[i] == '\n')
                    {
                        CurrentColumn = 1;
                    }
                    else
                    {
                        if (Data[i] != '\r')
                            CurrentColumn++;
                    }
                }
                return CurrentColumn;
            }
        }

        /// <summary>
        /// This gets the current position as an info struct.
        /// </summary>
        public PositionInfo PositionInfo
        {
            get
            {
                return new PositionInfo(Position, Row, Column);
            }
        }
        #endregion

        /// <summary>
        /// This initializes a new StringReader.
        /// </summary>
        /// <param name="Data">The string.</param>
        /// <param name="Position">The optional position.</param>
        public StringReader(string Data, int Position = 0)
        {
            this.Data = Data;
            this.Position = Position;
        }

        #region "Utility functions"
        /// <summary>
        /// This gets if the position exceeds the string length.
        /// </summary>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public bool IsEof(int Offset = 0)
        {
            return (Position + Offset > Data.Length - 1);
        }

        /// <summary>
        /// Resets the position.
        /// </summary>
        public void Reset()
        {
            this.Position = 0;
        }
        #endregion

        #region "Skip(...) overloads"
        /// <summary>
        /// This overload increments the position by N-amount of characters.
        /// (Negative values can be used to decrement!)
        /// </summary>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public void Skip(int Offset = 1)
        {
            this.Position += Offset;
        }

        /// <summary>
        /// This overload skips all given characters until it reaches a character that is not in the collection.
        /// </summary>
        /// <param name="Characters">The collection of characters.</param>
        public void Skip(IEnumerable<char> Characters)
        {
            while (!IsEof())
            {
                if (!Characters.Contains(Data[Position]))
                    break;
                Position++;
            }
        }
        #endregion

        #region "Peek(...) overloads"
        /// <summary>
        /// This overload gets if the current character matches the given one.
        /// </summary>
        /// <param name="Character">The given character.</param>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public bool Peek(char Character, int Offset = 0)
        {
            if (IsEof(Offset))
                return false;
            return (Data[Position + Offset] == Character);
        }
        
        /// <summary>
        /// This overload gets if the current character-sequence matches the given string.
        /// </summary>
        /// <param name="String">The given string.</param>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public bool Peek(string String, int Offset = 0)
        {
            // Handle edge-cases
            if (String == null)
                return false;
            if (String.Length == 0)
                return true;
            if (IsEof(Offset + String.Length - 1))
                return false;

            // Main use
            for (int i = 0; i < String.Length; i++)
            {
                if (Data[Position + Offset + i] != String[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This overload gets if any of the given characters match the current.
        /// </summary>
        /// <param name="Characters">The given characters.</param>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public bool Peek(IEnumerable<char> Characters, int Offset = 0)
        {
            foreach (var Character in Characters)
                if (Peek(Character, Offset))
                    return true;
            return false;
        }

        /// <summary>
        /// This overload gets if any of the given strings match to the current character-sequence.
        /// </summary>
        /// <param name="Strings">The given strings.</param>
        /// <param name="Offset">The optional offset.</param>
        /// <returns></returns>
        public bool Peek(IEnumerable<string> Strings, int Offset = 0)
        {
            foreach (var String in Strings)
                if (Peek(String, Offset))
                    return true;
            return false;
        }
        #endregion

        #region "IsNext(...) overloads"
        /// <summary>
        /// This overload gets if the current character matches the given one.
        /// If it does, it increments the position.
        /// </summary>
        /// <param name="Character">The given character.</param>
        /// <returns></returns>
        public bool IsNext(char Character)
        {
            if (IsEof())
                return false;
            if (Data[Position] == Character)
            {
                Position++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This overload gets if the current character-sequence matches the given string.
        /// If it does, it increments the position by the string length.
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public bool IsNext(string String)
        {
            // Handle edge-cases
            if (String == null)
                return false;
            if (String.Length == 0)
                return true;
            if (IsEof(String.Length - 1))
                return false;

            // Main use
            for (int i = 0; i < String.Length; i++)
            {
                if (Data[Position + i] != String[i])
                    return false;
            }
            Position += String.Length;
            return true;
        }

        /// <summary>
        /// This overload gets if any of the given characters match the current.
        /// </summary>
        /// <param name="Characters">The given characters.</param>
        /// <returns></returns>
        [Obsolete("This overload of IsNext(...) doesn't increment the position; use PeekNext(...) instead.")]
        public bool IsNext(IEnumerable<char> Characters)
        {
            return Peek(Characters, 0);
        }

        /// <summary>
        /// This overload gets if any of the given strings match to the current character-sequence.
        /// </summary>
        /// <param name="Strings">The given strings.</param>
        /// <returns></returns>
        [Obsolete("This overload of IsNext(...) doesn't increment the position; use PeekNext(...) instead.")]
        public bool IsNext(IEnumerable<string> Strings)
        {
            return Peek(Strings, 0);
        }
        #endregion

        #region "Basic reading"
        /// <summary>
        /// Reads a string of given length.
        /// If the end is reached, the buffer is returned.
        /// </summary>
        /// <param name="Length">The optional length.</param>
        /// <returns></returns>
        public string Read(int Length = 1)
        {
            // Edge-cases
            if (Length < 1)
                return "";
            if (IsEof())
                return "";
            if (Length == 1)
                return Data[Position++].ToString();

            // Main use
            string Buffer = "";
            for (int i = 0; i < Length; i++)
            {
                if (IsEof())
                    return Buffer;
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }

        /// <summary>
        /// Gets the rest of the string, starting from the current position.
        /// </summary>
        /// <returns></returns>
        public string ReadToEnd()
        {
            if (IsEof())
                return "";
            var Buffer = Data.Substring(Position);
            Position = Data.Length;
            return Buffer;
        }
        #endregion

        #region "Conditional reading"
        /// <summary>
        /// This overload reads the string until the given character or the end is reached.
        /// </summary>
        /// <param name="Character">The given character.</param>
        /// <returns></returns>
        public string ReadUntil(char Character)
        {
            string Buffer = "";
            while (!IsEof())
            {
                if (Data[Position] == Character)
                {
                    Position++;
                    return Buffer;
                }
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }

        /// <summary>
        /// This overload reads the string until a given substring or the end is reached.
        /// </summary>
        /// <param name="String">The given substring.</param>
        /// <returns></returns>
        public string ReadUntil(string String)
        {
            string Buffer = "";
            while (!IsEof())
            {
                if (IsNext(String))
                    return Buffer;
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }

        /// <summary>
        /// This overload reads the string until any given character or the end is reached.
        /// </summary>
        /// <param name="Characters">Given characters.</param>
        /// <returns></returns>
        public string ReadUntil(IEnumerable<char> Characters)
        {
            string Buffer = "";
            while (!IsEof())
            {
                if (Characters.Contains(Data[Position]))
                    return Buffer;
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }

        /// <summary>
        /// This overload reads the string until any given substring or the end is reached.
        /// </summary>
        /// <param name="Strings">Given strings.</param>
        /// <returns></returns>
        public string ReadUntil(IEnumerable<string> Strings)
        {
            string Buffer = "";
            while (!IsEof())
            {
                if (Peek(Strings))
                    return Buffer;
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }

        /// <summary>
        /// Reads the string until the next character is NOT in the given character array.
        /// </summary>
        /// <param name="Characters">The given character array.</param>
        /// <returns></returns>
        public string ReadUntilNot(IEnumerable<char> Characters)
        {
            string Buffer = "";
            while (!IsEof())
            {
                if (!Characters.Contains(Data[Position]))
                    return Buffer;
                Buffer += Data[Position];
                Position++;
            }
            return Buffer;
        }
        #endregion
    }
}