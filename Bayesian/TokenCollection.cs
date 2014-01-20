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
        /// Serializable data
        /// </summary>
        public class TokenData
        {
            /// <summary>
            /// The sum of all token counts.
            /// </summary>
            [ScriptIgnore]
            public int sum { get { return tokens.Values.Sum(); } }

            /// <summary>
            /// Token counter.
            /// </summary>
            public Dictionary<string, int> tokens { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public TokenData()
            {
                tokens = new Dictionary<string, int>();
            }

            /// <summary>
            /// Copy constructor
            /// </summary>
            public TokenData(TokenData pData)
            {
                tokens = new Dictionary<string, int>(pData.tokens);
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public TokenData(Dictionary<string, int> pTokens)
            {
                tokens = pTokens;
            }
        }

        /// <summary>
        /// Converts JSON into a TokenCollection object.
        /// </summary>
        public static TokenCollection deserialize(string pJSON)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, int> tokens = serializer.Deserialize<Dictionary<string, int>>(pJSON);
            return new TokenCollection(tokens);
        }

        /// <summary>
        /// Converts a TokenCollection object into JSON
        /// </summary>
        public static string serialize(TokenCollection pCollection)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(pCollection._data.tokens);
        }

        /// <summary>
        /// Subtracts the counts from a collection.
        /// 
        /// Only tokens that exist in the destination will be subtracted.
        /// </summary>
        public static TokenCollection subtract(TokenCollection pDest, TokenCollection pSrc)
        {
            if (pDest.sum < pSrc.sum)
            {
                throw new System.ArgumentException("Can not subtract a larger collection");
            }

            pSrc = TokenCollection.exclude(pDest._data.tokens.Keys, pSrc);

            TokenData copy = new TokenData(pDest._data);
            foreach (string token in pSrc._data.tokens.Keys)
            {
                copy.tokens[token] -= pSrc._data.tokens[token];
                if (copy.tokens[token] <= 0)
                {
                    copy.tokens.Remove(token);
                }
            }

            return new TokenCollection(copy);
        }

        /// <summary>
        /// Adds the counts from a collection.
        /// 
        /// Only tokens that exist in the destination will be added.
        /// </summary>
        public static TokenCollection add(TokenCollection pDest, TokenCollection pSrc)
        {
            pSrc = exclude(pDest._data.tokens.Keys, pSrc);

            TokenData copy = new TokenData(pDest._data);
            foreach (KeyValuePair<string, int> pair in pSrc._data.tokens)
            {
                copy.tokens[pair.Key] += pSrc._data.tokens[pair.Key];
            }

            return new TokenCollection(copy);
        }

        /// <summary>
        /// Merges all the tokens from both collections. Adding their counts together.
        /// </summary>
        public static TokenCollection merge(TokenCollection pDest, TokenCollection pSrc)
        {
            TokenData copy = new TokenData(pDest._data);
            foreach (string token in pSrc._data.tokens.Keys)
            {
                if (!copy.tokens.ContainsKey(token))
                {
                    copy.tokens.Add(token, 0);
                }
                copy.tokens[token] += pSrc._data.tokens[token];
            }

            return new TokenCollection(copy);
        }

        private static TokenCollection exclude(IEnumerable<string> pExclude, TokenCollection pSrc)
        {
            // remove tokens from the source that aren't in the destination.
            TokenCollection tmp = new TokenCollection(pSrc._data);
            string[] excludeKeys = pSrc._data.tokens.Keys.Except(pExclude).ToArray();
            foreach (string exclude in excludeKeys)
            {
                tmp.remove(exclude);
            }
            return tmp;
        }

        /// <summary>
        /// The tokens
        /// </summary>
        private readonly TokenData _data;

        /// <summary>
        /// The sum of all token counts.
        /// </summary>
        public int sum { get { return _data.sum; } }

        /// <summary>
        /// How many tokens.
        /// </summary>
        public int count { get { return _data.tokens.Count; } }

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
        /// Returns how many times the token exists in this collection.
        /// </summary>
        public int get(string pToken)
        {
            return _data.tokens.ContainsKey(pToken) ? _data.tokens[pToken] : 0;
        }

        /// <summary>
        /// Adds a token to the collection.
        /// </summary>
        public void add(string pToken)
        {
            if (!_data.tokens.ContainsKey(pToken))
            {
                _data.tokens.Add(pToken, 0);
            }
            _data.tokens[pToken]++;
        }

        /// <summary>
        /// Removes a token from the collection.
        /// </summary>
        private void remove(string pToken)
        {
            if (_data.tokens.ContainsKey(pToken))
            {
                _data.tokens.Remove(pToken);
            }
        }

        /// <summary>
        /// Checks if a token exists in the collection.
        /// </summary>
        public bool contains(string pToken)
        {
            return _data.tokens.ContainsKey(pToken);
        }
    }
}
