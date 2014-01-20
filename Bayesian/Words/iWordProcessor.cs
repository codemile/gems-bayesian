namespace Bayesian.Words
{
    /// <summary>
    /// Processors perform changes to the words that represent tokens.
    /// </summary>
    public interface iWordProcessor
    {
        /// <summary>
        /// Called to have a word modified or verified.
        /// </summary>
        /// <param name="pStr">The word.</param>
        /// <returns>The new word or Null if it is rejected.</returns>
        string Process(string pStr);
    }
}