using System.Collections.Generic;
using System.Linq;
using Bayesian.Words;

namespace Bayesian
{
    /// <summary>
    /// Used to process tokens.
    /// </summary>
    public class TokenProcessor : Processors
    {
        /// <summary>
        /// A list of the tokens, and their count.
        /// </summary>
        private readonly Dictionary<string, int> _tokens;

        /// <summary>
        /// The total number of tokens counted.
        /// </summary>
        public int sum { get { return _tokens.Values.Sum(); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenProcessor()
        {
            _tokens = new Dictionary<string, int>();
        }

        /// <summary>
        /// Adds a collection of tokens.
        /// </summary>
        public void add(IEnumerable<string> pTokens)
        {
            foreach (string token in pTokens)
            {
                add(token);
            }
        }

        /// <summary>
        /// Adds a token to the collection.
        /// </summary>
        private void add(string pToken)
        {
            string token = process(pToken);
            if (token == null)
            {
                return;
            }
            if (!_tokens.ContainsKey(token))
            {
                _tokens.Add(token, 0);
            }
            _tokens[token]++;
        }

        /// <summary>
        /// Converts the processor into a TokenCollection.
        /// </summary>
        /// <returns>The new TokenCollection</returns>
        public TokenCollection toCollection(int pMinCount)
        {
            Dictionary<string, int> tmp = _tokens.Where(pPair => pPair.Value > pMinCount).ToDictionary(pPair => pPair.Key, pPair => pPair.Value);
            return new TokenCollection(tmp);
        }

        /// <summary>
        /// Returns the count of how many tokens have the min count.
        /// </summary>
        /// <param name="pMinCount"></param>
        /// <returns></returns>
        public int count(int pMinCount)
        {
            return _tokens.Count(pPair => pPair.Value > pMinCount);
        }
    }
}
