using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Bayesian
{
    /// <summary>
    /// Keeps a count of the number of occurrences of a token.
    /// </summary>
    public class TokenCollection
    {
        /// <summary>
        /// The tokens
        /// </summary>
        private readonly TokenData _data;

        /// <summary>
        /// How many tokens.
        /// </summary>
        public int Count
        {
            get { return _data.Tokens.Count; }
        }

        /// <summary>
        /// The sum of all token counts.
        /// </summary>
        public int Sum
        {
            get { return _data.Sum; }
        }

        private static TokenCollection Exclude(IEnumerable<string> pExclude, TokenCollection pSrc)
        {
            // remove tokens from the source that aren't in the destination.
            TokenCollection tmp = new TokenCollection(pSrc._data);
            string[] excludeKeys = pSrc._data.Tokens.Keys.Except(pExclude).ToArray();
            foreach (string exclude in excludeKeys)
            {
                tmp.Remove(exclude);
            }
            return tmp;
        }

        /// <summary>
        /// Removes a token from the collection.
        /// </summary>
        private void Remove(string pToken)
        {
            if (_data.Tokens.ContainsKey(pToken))
            {
                _data.Tokens.Remove(pToken);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenCollection()
        {
            _data = new TokenData();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenCollection(Dictionary<string, int> pTokens)
        {
            _data = new TokenData(pTokens);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pData"></param>
        private TokenCollection(TokenData pData)
        {
            _data = pData;
        }

        /// <summary>
        /// Adds the counts from a collection.
        /// Only tokens that exist in the destination will be added.
        /// </summary>
        public static TokenCollection Add(TokenCollection pDest, TokenCollection pSrc)
        {
            pSrc = Exclude(pDest._data.Tokens.Keys, pSrc);

            TokenData copy = new TokenData(pDest._data);
            foreach (KeyValuePair<string, int> pair in pSrc._data.Tokens)
            {
                copy.Tokens[pair.Key] += pSrc._data.Tokens[pair.Key];
            }

            return new TokenCollection(copy);
        }

        /// <summary>
        /// Converts JSON into a TokenCollection object.
        /// </summary>
        public static TokenCollection Deserialize(string pJSON)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, int> tokens = serializer.Deserialize<Dictionary<string, int>>(pJSON);
            return new TokenCollection(tokens);
        }

        /// <summary>
        /// Merges all the tokens from both collections. Adding their counts together.
        /// </summary>
        public static TokenCollection Merge(TokenCollection pDest, TokenCollection pSrc)
        {
            TokenData copy = new TokenData(pDest._data);
            foreach (string token in pSrc._data.Tokens.Keys)
            {
                if (!copy.Tokens.ContainsKey(token))
                {
                    copy.Tokens.Add(token, 0);
                }
                copy.Tokens[token] += pSrc._data.Tokens[token];
            }

            return new TokenCollection(copy);
        }

        /// <summary>
        /// Converts a TokenCollection object into JSON
        /// </summary>
        public static string Serialize(TokenCollection pCollection)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(pCollection._data.Tokens);
        }

        /// <summary>
        /// Subtracts the counts from a collection.
        /// Only tokens that exist in the destination will be subtracted.
        /// </summary>
        public static TokenCollection Subtract(TokenCollection pDest, TokenCollection pSrc)
        {
            if (pDest.Sum < pSrc.Sum)
            {
                throw new ArgumentException("Can not subtract a larger collection");
            }

            pSrc = Exclude(pDest._data.Tokens.Keys, pSrc);

            TokenData copy = new TokenData(pDest._data);
            foreach (string token in pSrc._data.Tokens.Keys)
            {
                copy.Tokens[token] -= pSrc._data.Tokens[token];
                if (copy.Tokens[token] <= 0)
                {
                    copy.Tokens.Remove(token);
                }
            }

            return new TokenCollection(copy);
        }

        /// <summary>
        /// Adds a token to the collection.
        /// </summary>
        public void Add(string pToken)
        {
            if (!_data.Tokens.ContainsKey(pToken))
            {
                _data.Tokens.Add(pToken, 0);
            }
            _data.Tokens[pToken]++;
        }

        /// <summary>
        /// Checks if a token exists in the collection.
        /// </summary>
        public bool Contains(string pToken)
        {
            return _data.Tokens.ContainsKey(pToken);
        }

        /// <summary>
        /// Returns how many times the token exists in this collection.
        /// </summary>
        public int get(string pToken)
        {
            return _data.Tokens.ContainsKey(pToken) ? _data.Tokens[pToken] : 0;
        }

        /// <summary>
        /// Serializable data
        /// </summary>
        public class TokenData
        {
            /// <summary>
            /// The sum of all token counts.
            /// </summary>
            [ScriptIgnore]
            public int Sum
            {
                get { return Tokens.Values.Sum(); }
            }

            /// <summary>
            /// Token counter.
            /// </summary>
            public Dictionary<string, int> Tokens { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public TokenData()
            {
                Tokens = new Dictionary<string, int>();
            }

            /// <summary>
            /// Copy constructor
            /// </summary>
            public TokenData(TokenData pData)
            {
                Tokens = new Dictionary<string, int>(pData.Tokens);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public TokenData(Dictionary<string, int> pTokens)
            {
                Tokens = pTokens;
            }
        }
    }
}