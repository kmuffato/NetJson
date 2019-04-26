/*
    +==========================================+
    +             NetJsonTest v1.0             +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Collections.Generic;

namespace JonasFX.CliShiv
{
    /// <summary>
    /// A helper class for creating padded tables in text-based interfaces.
    /// </summary>
    public class TextTable
    {
        private string[] m_Headers;
        private List<string[]> m_Rows;
        private int[] m_Lengths;
        private bool m_Indexed;

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Columns
        {
            get => m_Headers.Length;
        }

        /// <summary>
        /// Initializes a new table.
        /// </summary>
        /// <param name="Headers">The table headers.</param>
        public TextTable(string[] Headers, bool Indexed = false)
        {
            if (Indexed)
            {
                // Prefix a hidden, empty header in which the index is stored
                m_Headers = new string[Headers.Length + 1];
                m_Headers[0] = " ";
                Headers.CopyTo(m_Headers, 1);
            }
            else
                m_Headers = Headers;

            m_Rows = new List<string[]>();
            m_Lengths = new int[m_Headers.Length];
            m_Indexed = Indexed;

            // Fill in the header's base length
            for (int i = 0; i < m_Headers.Length; i++)
            {
                m_Lengths[i] = m_Headers[i].Length;
            }
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="Values">The row values.</param>
        public void AddRow(object[] Values)
        {
            if (m_Indexed)
            {
                if (m_Indexed && Values.Length != m_Headers.Length - 1)
                    throw new ArgumentException("The number of the values must be equal to the number of columns.");

                // Insert the index
                var n_Values = new object[Values.Length + 1];
                n_Values[0] = m_Rows.Count;
                Values.CopyTo(n_Values, 1);
                Values = n_Values;
            }
            else
            {
                if (!m_Indexed && Values.Length != m_Headers.Length)
                    throw new ArgumentException("The number of the values must be equal to the number of columns.");
            }

            string[] Row = new string[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                string String = "";

                // Stringify the object
                if (Values[i] == null)
                    String = "null";
                else
                    String = Values[i].ToString();

                // Update length if necessary
                if (String.Length > m_Lengths[i])
                    m_Lengths[i] = String.Length;

                Row[i] = String;
            }

            m_Rows.Add(Row);
        }

        /// <summary>
        /// Adds a break line to the table.
        /// </summary>
        public void AddBreak()
        {
            m_Rows.Add(null);
        }

        /// <summary>
        /// Pads a string to a given length.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <param name="Length">The length.</param>
        /// <returns></returns>
        public static string Pad(string Text, int Length)
        {
            if (Text.Length > Length)
                return Text.Substring(0, Length);
            return Text + new string(' ', Length - Text.Length);
        }

        /// <summary>
        /// Renders the table and returns it.
        /// </summary>
        /// <returns></returns>
        public string GetString(bool Framed = true)
        {
            string Output = "";

            // Render the top-frame
            if (Framed)
            {
                Output += "┌─";
                for (int i = 0; i < m_Lengths.Length; i++)
                {
                    Output += new string('─', m_Lengths[i]);
                    if (i != m_Lengths.Length - 1)
                        Output += "─┬─";
                }
                Output += "─┐\n";
            }

            // Render the header section
            if (Framed) Output += "│ "; // Left frame
            else Output += " "; // Left spacing
            for (int i = 0; i < m_Headers.Length; i++)
            {
                Output += Pad(m_Headers[i], m_Lengths[i]);
                if (i != m_Headers.Length - 1)
                    Output += " │ ";
            }
            if (Framed) Output += " │"; // Right frame
            else Output += " "; // Right spacing
            Output += "\n";

            // Render the "header to data" break section
            if (Framed) Output += "├─"; // Left frame
            else Output += "─"; // Left spacing
            for (int i = 0; i < m_Lengths.Length; i++)
            {
                Output += new string('─', m_Lengths[i]);
                if (i != m_Lengths.Length - 1)
                    Output += "─┼─";
            }
            if (Framed) Output += "─┤"; // Right frame
            else Output += "─"; // Right spacing
            Output += "\n";

            // Render the data section
            for (int r = 0; r < m_Rows.Count; r++)
            {
                var Row = m_Rows[r];
                if (Row == null)
                {
                    if (Framed) Output += (r == m_Rows.Count - 1) ? "└─" : "├─"; // Left frame, is ending line?
                    else Output += "─"; // Left spacing
                    // Row is null, so it's a break section
                    for (int i = 0; i < m_Lengths.Length; i++)
                    {
                        Output += new string('─', m_Lengths[i]);
                        if (i != m_Lengths.Length - 1)
                            // Is ending line?
                            Output += (r == m_Rows.Count - 1) ? "─┴─" : "─┼─";
                    }
                    if (Framed) Output += (r == m_Rows.Count - 1) ? "─┘" : "─┤"; // Right frame, is ending line?
                    else Output += "─"; // Right spacing
                }
                else
                {
                    if (Framed) Output += "│ "; // Left frame
                    else Output += " "; // Left spacing
                    // Row has data, render data
                    for (int i = 0; i < Row.Length; i++)
                    {
                        Output += Pad(Row[i], m_Lengths[i]);
                        if (i != Row.Length - 1)
                            Output += " │ ";
                    }
                    if (Framed) Output += " │"; // Right frame
                    else Output += " "; // Right spacing
                }
                if (r != m_Rows.Count - 1 || Framed)
                    Output += "\n";
            }

            // Render bottom-frame
            if (Framed)
            {
                Output += "└─";
                for (int i = 0; i < m_Lengths.Length; i++)
                {
                    Output += new string('─', m_Lengths[i]);
                    if (i != m_Lengths.Length - 1)
                        Output += "─┴─";
                }
                Output += "─┘";
            }

            return Output;
        }
    }
}