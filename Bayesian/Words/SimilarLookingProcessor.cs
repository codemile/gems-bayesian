using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bayesian.Words
{
    /// <summary>
    /// The OCR will often read a word and generate the wrong letters, because that
    /// letter looks similar to other letters.
    /// 
    /// For example; "Example" might be read as "3xomp1e".
    /// 
    /// This processor uses a list of similar looking letters and replaces those
    /// letters in the word with one common letter for that set.
    /// 
    /// When this is done for all words. The resulting list of words will be more
    /// consistent despite the random behavior of the OCR.
    /// </summary>
    public class SimilarLookingProcessor : iWordProcessor
    {
        /// <summary>
        /// A list of letters and the other letters they look like.
        /// </summary>
        private static readonly string[] _defaultSets = {
                                          "A",
                                          "B83",
                                          "CO",
                                          "DO",
                                          "EB",
                                          "FP",
                                          "GbO",
                                          "H#",
                                          "Il1",
                                          "J",
                                          "K",
                                          "LlI1t",
                                          "M",
                                          "N",
                                          "O0",
                                          "PR",
                                          "QO0",
                                          "RP",
                                          "S8$",
                                          "TI",
                                          "UV",
                                          "VU",
                                          "WV",
                                          "X",
                                          "YV",
                                          "Z2",
                                          "aoq",
                                          "b6",
                                          "co",
                                          "d",
                                          "eo",
                                          "f",
                                          "g9",
                                          "h",
                                          "i1",
                                          "ji",
                                          "k",
                                          "l1I",
                                          "mn",
                                          "nm",
                                          "o0",
                                          "p",
                                          "q9",
                                          "r",
                                          "s",
                                          "tf",
                                          "uv",
                                          "vu",
                                          "wv",
                                          "x*",
                                          "yV",
                                          "z2"
                                      };

        /// <summary>
        /// A collection of letters.
        /// </summary>
        private readonly List<char> _letters;

        /// <summary>
        /// A mapping of a char to it's replacement in _letters.
        /// </summary>
        private readonly Dictionary<char, int> _index;

        /// <summary>
        /// Merges two strings so that no characters are duplicated.
        /// </summary>
        public static StringBuilder merge(StringBuilder pA, string pB)
        {
            foreach (char c in pB.Where(pC => pA.ToString().IndexOf((char)pC) == -1))
            {
                pA.Append(c);
            }
            return pA;
        }

        /// <summary>
        /// Finds the first string in a collection that contains some of the letters
        /// from str.
        /// </summary>
        /// <param name="pSet">The collection to search.</param>
        /// <param name="pStr">The letters to find.</param>
        /// <returns>The string found, or Null.</returns>
        public static StringBuilder findFirstShared(IEnumerable<StringBuilder> pSet, string pStr)
        {
            char[] c = pStr.ToCharArray();
            return pSet.FirstOrDefault(pA => pA.ToString().IndexOfAny(c) != -1);
        }

        /// <summary>
        /// Compact the list of letter sets so that no letters
        /// are duplicated in more then one set.
        /// </summary>
        public static IEnumerable<StringBuilder> compactSet(IEnumerable<string> pSet)
        {
            List<StringBuilder> compact = new List<StringBuilder>();
            foreach (string a in pSet)
            {
                StringBuilder str = findFirstShared(compact, a);
                if (str == null)
                {
                    compact.Add(new StringBuilder(a));
                }
                else
                {
                    merge(str, a);
                }
            }
            return compact;
        }

        /// <summary>
        /// Maps a character to a key character.
        /// </summary>
        public static void map(List<char> pLetters, Dictionary<char, int> pIndex, char pKey, char pChar)
        {
            // no need to map
            if (pIndex.ContainsKey(pKey) && pIndex.ContainsKey(pChar))
            {
                return;
            }

            // reverse the order
            if (!pIndex.ContainsKey(pKey) && pIndex.ContainsKey(pChar))
            {
                char c = pKey;
                pKey = pChar;
                pChar = c;
            }

            // key is already mapped
            if (pIndex.ContainsKey(pKey))
            {
                pKey = pLetters[pIndex[pKey]];
            }

            // is a new key
            if (pLetters.IndexOf(pKey) == -1)
            {
                pLetters.Add(pKey);
            }

            // add the mapping for both characters
            foreach (char c in new char[] { pKey, pChar }.Where(pC => !pIndex.ContainsKey(pC)))
            {
                pIndex.Add(c, pLetters.IndexOf(pKey));
            }
        }

        /// <summary>
        /// Populates the lookup index and letters cache from a set of strings.
        /// </summary>
        public static void buildIndex(List<char> pLetters, Dictionary<char, int> pIndex, IEnumerable<StringBuilder> pSets)
        {
            foreach (StringBuilder sb in pSets)
            {
                string str = sb.ToString();
                if (str.Length <= 1)
                {
                    continue;
                }
                char first = str[0];
                for (int i = 1; i < str.Length; i++)
                {
                    map(pLetters, pIndex, first, str[i]);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimilarLookingProcessor()
            : this(_defaultSets)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimilarLookingProcessor(IEnumerable<string> pSets)
        {
            _letters = new List<char>();
            _index = new Dictionary<char, int>();

            // We need to convert the SETS array into an optimized look up
            // of a single character to a string of possible characters.
            // 
            // It doesn't matter what the single character is. So the first
            // look up that doesn't find key in _mapping will be used to
            // create a new key.
            IEnumerable<StringBuilder> compact = compactSet(pSets);
            buildIndex(_letters, _index, compact);
        }

        /// <summary>
        /// Convert the letters of the word into their common letters.
        /// </summary>
        public string process(string pStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in pStr)
            {
                sb.Append(_index.ContainsKey(c) ? _letters[_index[c]] : c);
            }
            return sb.ToString();
        }
    }
}
