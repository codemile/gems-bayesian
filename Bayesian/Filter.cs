
namespace Bayesian
{
    /// <summary>
    /// Defines a filter to be analyzed
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// The good words
        /// </summary>
        private TokenCollection _good;

        /// <summary>
        /// The bad words
        /// </summary>
        private TokenCollection _bad;

        /// <summary>
        /// Constructor
        /// </summary>
        public Filter(TokenCollection pGoodWords, TokenCollection pBadWords)
        {
            _good = pGoodWords;
            _bad = pBadWords;
        }
    }
}
