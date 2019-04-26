/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

namespace NetJson.Analysis
{
    /// <summary>
    /// This struct holds a file position info.
    /// </summary>
    public struct PositionInfo
    {
        /// <summary>
        /// This gets the null-based raw position.
        /// </summary>
        public int Position
        {
            get;
            private set;
        }

        /// <summary>
        /// This gets the row.
        /// </summary>
        public int Row
        {
            get;
            private set;
        }

        /// <summary>
        /// This gets the column.
        /// </summary>
        public int Column
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new position info.
        /// </summary>
        /// <param name="Position">The null-based raw position.</param>
        /// <param name="Row">The row.</param>
        /// <param name="Column">The column.</param>
        public PositionInfo(int Position, int Row, int Column)
        {
            this.Position = Position;
            this.Row = Row;
            this.Column = Column;
        }

        /// <summary>
        /// This gets the string representation of the position.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + Row.ToString() + ", " + Column.ToString() + ")";
        }
    }
}