using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bayesian.Words;

namespace Bayesian
{
    /// <summary>
    /// 
    /// </summary>
    public class Tokens : ICollection<string>
    {
        /// <summary>
        /// ReGex pattern for words that don't start with a number
        /// </summary>
        private const string _TOKEN_PATTERN = @"(?si:)\b(?<val>[a-zA-Z][a-zA-Z0-9]{1,50})\b";

        /// <summary>
        /// How many tokens.
        /// </summary>
        public int Count { get { return _tokens.Count; } }

        /// <summary>
        /// Collection
        /// </summary>
        private readonly List<string> _tokens;

        /// <summary>
        /// Process the tokens before they are added.
        /// </summary>
        private readonly Processors _processors;

        /// <summary>
        /// Constructor
        /// </summary>
        private Tokens(Processors pProc)
        {
            _tokens = new List<string>();
            _processors = pProc;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDocument"></param>
        /// <param name="pProc"></param>
        public Tokens(string pDocument, Processors pProc)
            : this(pProc)
        {
            Regex r = new Regex(_TOKEN_PATTERN);
            Match m = r.Match(pDocument);
            while (m.Success)
            {
                Add(m.Groups["val"].Value);
                m = m.NextMatch();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTokens"></param>
        /// <param name="pProc"></param>
        public Tokens(IEnumerable<string> pTokens, Processors pProc)
            : this(pProc)
        {
            foreach (string token in pTokens.Where(pToken => Regex.IsMatch(pToken, _TOKEN_PATTERN)))
            {
                Add(token);
            }
        }

        /// <summary>
        /// Adds a token.
        /// </summary>
        public void Add(string pToken)
        {
            string token = _processors.process(pToken);
            if (token != null)
            {
                _tokens.Add(token);
            }
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            _tokens.Clear();
        }

        /// <summary>
        /// Check if a token was already added.
        /// </summary>
        public bool Contains(string pItem)
        {
            return _tokens.Contains(pItem.ToLower());
        }

        /// <summary>
        /// Copies to an array.
        /// </summary>
        public void CopyTo(string[] pArray, int pArrayIndex)
        {
            _tokens.CopyTo(pArray, pArrayIndex);
        }

        /// <summary>
        /// Not read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes a token.
        /// </summary>
        public bool Remove(string pItem)
        {
            return _tokens.Remove(pItem);
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }
    }
}
