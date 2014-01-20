using System.Text.RegularExpressions;

namespace Bayesian.Words
{
    /// <summary>
    /// Only allows words that start with a letter, and then contains only letters
    /// and numbers. If there are any other characters, then it will be rejected.
    /// </summary>
    public class AlphaNumericProcessor : iWordProcessor
    {
        /// <summary>
        /// ReGex pattern for words that don't start with a number
        /// </summary>
        private const string _PATTERN = @"^[a-zA-Z][a-zA-Z0-9]*$";

        /// <summary>
        /// Compiled ReGex.
        /// </summary>
        private readonly Regex _reg;

        /// <summary>
        /// Constructor
        /// </summary>
        public AlphaNumericProcessor()
        {
            _reg = new Regex(_PATTERN);
        }

        /// <summary>
        /// Perform the test.
        /// </summary>
        public string process(string pStr)
        {
            return _reg.IsMatch(pStr) ? pStr : null;
        }
    }
}
