/*
    +==========================================+
    +             NetJsonTest v1.0             +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using NetJson;

namespace NetJsonTest
{
    /// <summary>
    /// A validator result.
    /// </summary>
    public enum ValidationResult
    {
        Pass,
        Failure,
        Irrelevant
    }

    /// <summary>
    /// The output of the validator.
    /// </summary>
    public class ValidatorOutput
    {
        /// <summary>
        /// The result.
        /// </summary>
        public ValidationResult Result
        {
            get;
            private set;
        }

        /// <summary>
        /// The error message. (if any)
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new ValidatorOutput.
        /// </summary>
        /// <param name="Result">The result.</param>
        /// <param name="Message">The error message. (if any)</param>
        public ValidatorOutput(ValidationResult Result, string Message)
        {
            this.Result = Result;
            this.Message = Message;
        }
    }

    /// <summary>
    /// Uses the parser's exceptions to validate a JSON string.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Uses the parser's exceptions to validate the JSON string.
        /// </summary>
        /// <param name="Data">The JSON string.</param>
        /// <returns></returns>
        public static ValidatorOutput Validate(string Data)
        {
            try
            {
                JSON.Parse(Data);
                return new ValidatorOutput(ValidationResult.Pass, "");
            } catch (Exception ex)
            {
                return new ValidatorOutput(ValidationResult.Failure, ex.Message);
            }
        }

        /// <summary>
        /// Returns true if the result was expected or if there was no expected result.
        /// </summary>
        /// <param name="Expected">The expected result.</param>
        /// <param name="Current">The current result.</param>
        /// <returns></returns>
        public static bool HasPassed(ValidationResult Expected, ValidationResult Current)
        {
            if (Expected == Current)
                return true;
            if (Expected == ValidationResult.Irrelevant)
                return true;
            return false;
        }
    }
}