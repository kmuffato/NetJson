/*
    +==========================================+
    +             NetJsonTest v1.0             +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

namespace NetJsonTest
{
    /// <summary>
    /// Contains a parser-test.
    /// </summary>
    public class ParserTest
    {
        /// <summary>
        /// The test name.
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The test content.
        /// </summary>
        public string Content;

        /// <summary>
        /// The expected result.
        /// </summary>
        public ValidationResult Expected;

        /// <summary>
        /// The output of the validator.
        /// </summary>
        public ValidatorOutput Output;

        /// <summary>
        /// Initializes a new ParserTest.
        /// </summary>
        /// <param name="Category">The test category.</param>
        /// <param name="Description">The test description.</param>
        /// <param name="Expected">The expected result.</param>
        /// <param name="Content">The test content.</param>
        public ParserTest(string Name, ValidationResult Expected, string Content)
        {
            this.Name = Name;
            this.Content = Content;
            this.Expected = Expected;
        }
    }
}