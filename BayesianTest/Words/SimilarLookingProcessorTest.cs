using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class SimilarLookingProcessorTest
    {
        private static void AreEqual(string pA, StringBuilder pB)
        {
            Assert.IsNotNull(pB);
            Assert.AreEqual(pA, pB.ToString());
        }

        private static List<StringBuilder> ToStringBuilder(IEnumerable<string> pStrs)
        {
            return pStrs.Select(pStr=>new StringBuilder(pStr)).ToList();
        }

        [TestMethod]
        public void Test_BuildIndex()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.BuildIndex(letters, index,
                ToStringBuilder(new[] {"B83", "CO", "DO", "EB", "GbO", "LlI1t"}));

            Assert.AreEqual('B', letters[index['B']]);
            Assert.AreEqual('B', letters[index['8']]);
            Assert.AreEqual('B', letters[index['3']]);
            Assert.AreEqual('B', letters[index['E']]);
            Assert.AreEqual('C', letters[index['O']]);
            Assert.AreEqual('C', letters[index['D']]);
        }

        [TestMethod]
        public void Test_BuildIndex_One_Letter()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.BuildIndex(letters, index, ToStringBuilder(new[] {"A", "B", "C"}));

            Assert.AreEqual(0, letters.Count);
            Assert.AreEqual(0, index.Count);
        }

        [TestMethod]
        public void Test_BuildIndex_Small()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.BuildIndex(letters, index, ToStringBuilder(new[] {"B8", "EB"}));

            Assert.AreEqual(1, letters.Count);
            Assert.AreEqual(3, index.Count);

            Assert.AreEqual('B', letters[index['B']]);
            Assert.AreEqual('B', letters[index['8']]);
            Assert.AreEqual('B', letters[index['E']]);
        }

        [TestMethod]
        public void Test_BuildIndex_Small_Divided()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.BuildIndex(letters, index, ToStringBuilder(new[] {"B8", "CO", "EB"}));

            Assert.AreEqual(2, letters.Count);
            Assert.AreEqual(5, index.Count);

            Assert.AreEqual('B', letters[index['B']]);
            Assert.AreEqual('B', letters[index['8']]);
            Assert.AreEqual('B', letters[index['E']]);
            Assert.AreEqual('C', letters[index['C']]);
            Assert.AreEqual('C', letters[index['O']]);
        }

        [TestMethod]
        public void Test_Compact()
        {
            List<StringBuilder> result =
                new List<StringBuilder>(SimilarLookingProcessor.CompactSet(new[] {"abc", "xyz"}));
            Assert.AreEqual(2, result.Count);
            AreEqual("abc", result[0]);
            AreEqual("xyz", result[1]);

            result = new List<StringBuilder>(SimilarLookingProcessor.CompactSet(new[] {"abc", "xa"}));
            Assert.AreEqual(1, result.Count);
            AreEqual("abcx", result[0]);

            result =
                new List<StringBuilder>(
                    SimilarLookingProcessor.CompactSet(new[] {"B83", "CO", "DO", "EB", "GbO", "LlI1t"}));
            Assert.AreEqual(3, result.Count);
            AreEqual("B83E", result[0]);
            AreEqual("CODGb", result[1]);
            AreEqual("LlI1t", result[2]);
        }

        /// <summary>
        /// This test is difficult to predict because the set is large, and could
        /// be changed by the programmer for tuning reasons.
        /// </summary>
        [TestMethod]
        public void Test_Default_Process()
        {
            SimilarLookingProcessor proc = new SimilarLookingProcessor();

            Assert.AreEqual("BIBBIB", proc.Process("Bl83LE"));
        }

        [TestMethod]
        public void Test_FindFirstShared()
        {
            List<StringBuilder> set = ToStringBuilder(new[] {"abc", "def", "ghi"});
            string[] test = {"a", "b", "c", "ab", "adef"};

            foreach (StringBuilder sb in test.Select(pT=>SimilarLookingProcessor.FindFirstShared(set, pT)))
            {
                AreEqual("abc", sb);
            }

            Assert.IsNull(SimilarLookingProcessor.FindFirstShared(set, "z"));
            AreEqual("def", SimilarLookingProcessor.FindFirstShared(set, "zxydwtq"));
        }

        [TestMethod]
        public void Test_Find_And_Merge()
        {
            const string test = "xyzd";
            IEnumerable<StringBuilder> set = ToStringBuilder(new[] {"abc", "def", "ghi"});
            StringBuilder sb = SimilarLookingProcessor.FindFirstShared(set, test);
            Assert.IsNotNull(sb);
            AreEqual("def", sb);

            AreEqual("defxyz", SimilarLookingProcessor.Merge(sb, test));
        }

        [TestMethod]
        public void Test_Map()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.Map(letters, index, 'x', 'x');
            SimilarLookingProcessor.Map(letters, index, 'x', 'y');
            SimilarLookingProcessor.Map(letters, index, 'x', 'y');
            SimilarLookingProcessor.Map(letters, index, 'x', 'z');
            SimilarLookingProcessor.Map(letters, index, 'q', 'x');

            Assert.AreEqual(1, letters.Count);
            Assert.IsTrue(letters.Contains('x'));

            Assert.AreEqual(4, index.Count);
            Assert.IsTrue(index.ContainsKey('x'));
            Assert.IsTrue(index.ContainsKey('y'));
            Assert.IsTrue(index.ContainsKey('z'));
            Assert.IsTrue(index.ContainsKey('q'));

            Assert.AreEqual('x', letters[index['x']]);
            Assert.AreEqual('x', letters[index['y']]);
            Assert.AreEqual('x', letters[index['z']]);
            Assert.AreEqual('x', letters[index['q']]);

            SimilarLookingProcessor.Map(letters, index, 'a', 'b');
            SimilarLookingProcessor.Map(letters, index, 'b', 'c');

            Assert.AreEqual(2, letters.Count);
            Assert.IsTrue(letters.Contains('x'));
            Assert.IsTrue(letters.Contains('a'));
        }

        [TestMethod]
        public void Test_Map_B_E()
        {
            List<char> letters = new List<char>();
            Dictionary<char, int> index = new Dictionary<char, int>();

            SimilarLookingProcessor.Map(letters, index, 'B', '8');
            SimilarLookingProcessor.Map(letters, index, 'E', 'B');

            Assert.AreEqual(1, letters.Count);
            Assert.IsTrue(letters.Contains('B'));

            Assert.AreEqual(3, index.Count);
            Assert.IsTrue(index.ContainsKey('B'));
            Assert.IsTrue(index.ContainsKey('8'));
            Assert.IsTrue(index.ContainsKey('E'));

            Assert.AreEqual('B', letters[index['B']]);
            Assert.AreEqual('B', letters[index['8']]);
            Assert.AreEqual('B', letters[index['E']]);
        }

        [TestMethod]
        public void Test_Merge()
        {
            AreEqual("abcdef", SimilarLookingProcessor.Merge(new StringBuilder(), "abcdef"));
            AreEqual("abcdef", SimilarLookingProcessor.Merge(new StringBuilder("abcdef"), ""));

            AreEqual("abcdef", SimilarLookingProcessor.Merge(new StringBuilder("abc"), "def"));
            AreEqual("abcdef", SimilarLookingProcessor.Merge(new StringBuilder("abcdef"), "def"));
            AreEqual("abcd", SimilarLookingProcessor.Merge(new StringBuilder("abc"), "ddd"));
            AreEqual("abcde", SimilarLookingProcessor.Merge(new StringBuilder("abcd"), "eddede"));
        }

        [TestMethod]
        public void Test_Small_Process()
        {
            SimilarLookingProcessor proc = new SimilarLookingProcessor(new[] {"B83", "OD0"});

            Assert.AreEqual("BIBBLE", proc.Process("BI83LE"));
            Assert.AreEqual("BIBBLE", proc.Process("BIBBLE"));
            Assert.AreEqual("GOOO", proc.Process("G0OD"));
            Assert.AreEqual("GOOO", proc.Process("GOOD"));
            Assert.AreEqual("GOOO", proc.Process("G0DO"));

            Assert.AreEqual("Chicken", proc.Process("Chicken"));
            Assert.AreEqual("nothing", proc.Process("nothing"));
        }
    }
}