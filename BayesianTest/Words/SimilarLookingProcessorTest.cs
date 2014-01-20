using System.Collections.Generic;
using System.Text;
using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class SimilarLookingProcessorTest
    {
        public static IEnumerable<StringBuilder> ToStringBuilder(IEnumerable<string> pStrs)
        {
            List<StringBuilder> set = new List<StringBuilder>();
            foreach (string str in pStrs)
            {
                set.Add(new StringBuilder(str));
            }
            return set;
        }

        public static void AreEqual(StringBuilder a, StringBuilder b)
        {
            Assert.IsNotNull(a);
            Assert.IsNotNull(b);
            Assert.AreEqual(a.ToString(), b.ToString());
        }

        public static void AreEqual(string a, StringBuilder b)
        {
            Assert.IsNotNull(b);
            Assert.AreEqual(a, b.ToString());
        }

        [TestMethod]
        public void Test_Merge()
        {
            AreEqual("abcdef", SimilarLookingProcessor.merge(new StringBuilder(), "abcdef"));
            AreEqual("abcdef", SimilarLookingProcessor.merge(new StringBuilder("abcdef"), ""));

            AreEqual("abcdef", SimilarLookingProcessor.merge(new StringBuilder("abc"), "def"));
            AreEqual("abcdef", SimilarLookingProcessor.merge(new StringBuilder("abcdef"), "def"));
            AreEqual("abcd", SimilarLookingProcessor.merge(new StringBuilder("abc"), "ddd"));
            AreEqual("abcde", SimilarLookingProcessor.merge(new StringBuilder("abcd"), "eddede"));
        }

        [TestMethod]
        public void Test_FindFirstShared()
        {
            IEnumerable<StringBuilder> set = ToStringBuilder(new string[] { "abc", "def", "ghi" });
            string[] test = { "a", "b", "c", "ab", "adef" };

            foreach (string t in test)
            {
                StringBuilder sb = SimilarLookingProcessor.findFirstShared(set, t);
                AreEqual("abc", sb);
            }

            Assert.IsNull(SimilarLookingProcessor.findFirstShared(set, "z"));
            AreEqual("def", SimilarLookingProcessor.findFirstShared(set, "zxydwtq"));
        }

        [TestMethod]
        public void Test_Find_And_Merge()
        {
            string test = "xyzd";
            IEnumerable<StringBuilder> set = ToStringBuilder(new string[] { "abc", "def", "ghi" });
            StringBuilder sb = SimilarLookingProcessor.findFirstShared(set, test);
            Assert.IsNotNull(sb);
            AreEqual("def", sb);

            AreEqual("defxyz", SimilarLookingProcessor.merge(sb, test));
        }

        [TestMethod]
        public void Test_Compact()
        {
            List<StringBuilder> result = new List<StringBuilder>(SimilarLookingProcessor.compactSet(new string[] { "abc", "xyz" }));
            Assert.AreEqual(2, result.Count);
            AreEqual("abc", result[0]);
            AreEqual("xyz", result[1]);

            result = new List<StringBuilder>(SimilarLookingProcessor.compactSet(new string[] { "abc", "xa" }));
            Assert.AreEqual(1, result.Count);
            AreEqual("abcx", result[0]);

            result = new List<StringBuilder>(SimilarLookingProcessor.compactSet(new string[] { "B83", "CO", "DO", "EB", "GbO", "LlI1t" }));
            Assert.AreEqual(3, result.Count);
            AreEqual("B83E", result[0]);
            AreEqual("CODGb", result[1]);
            AreEqual("LlI1t", result[2]);
        }

        [TestMethod]
        public void Test_Map()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.map(Letters, Index, 'x', 'x');
            SimilarLookingProcessor.map(Letters, Index, 'x', 'y');
            SimilarLookingProcessor.map(Letters, Index, 'x', 'y');
            SimilarLookingProcessor.map(Letters, Index, 'x', 'z');
            SimilarLookingProcessor.map(Letters, Index, 'q', 'x');

            Assert.AreEqual(1, Letters.Count);
            Assert.IsTrue(Letters.Contains('x'));

            Assert.AreEqual(4, Index.Count);
            Assert.IsTrue(Index.ContainsKey('x'));
            Assert.IsTrue(Index.ContainsKey('y'));
            Assert.IsTrue(Index.ContainsKey('z'));
            Assert.IsTrue(Index.ContainsKey('q'));

            Assert.AreEqual('x', Letters[Index['x']]);
            Assert.AreEqual('x', Letters[Index['y']]);
            Assert.AreEqual('x', Letters[Index['z']]);
            Assert.AreEqual('x', Letters[Index['q']]);

            SimilarLookingProcessor.map(Letters, Index, 'a', 'b');
            SimilarLookingProcessor.map(Letters, Index, 'b', 'c');

            Assert.AreEqual(2, Letters.Count);
            Assert.IsTrue(Letters.Contains('x'));
            Assert.IsTrue(Letters.Contains('a'));
        }

        [TestMethod]
        public void Test_Map_B_E()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.map(Letters, Index, 'B', '8');
            SimilarLookingProcessor.map(Letters, Index, 'E', 'B');

            Assert.AreEqual(1, Letters.Count);
            Assert.IsTrue(Letters.Contains('B'));

            Assert.AreEqual(3, Index.Count);
            Assert.IsTrue(Index.ContainsKey('B'));
            Assert.IsTrue(Index.ContainsKey('8'));
            Assert.IsTrue(Index.ContainsKey('E'));

            Assert.AreEqual('B', Letters[Index['B']]);
            Assert.AreEqual('B', Letters[Index['8']]);
            Assert.AreEqual('B', Letters[Index['E']]);
        }

        [TestMethod]
        public void Test_BuildIndex_Small()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.buildIndex(Letters, Index, ToStringBuilder(new string[] { "B8", "EB" }));

            Assert.AreEqual(1, Letters.Count);
            Assert.AreEqual(3, Index.Count);

            Assert.AreEqual('B', Letters[Index['B']]);
            Assert.AreEqual('B', Letters[Index['8']]);
            Assert.AreEqual('B', Letters[Index['E']]);
        }

        [TestMethod]
        public void Test_BuildIndex_Small_Divided()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.buildIndex(Letters, Index, ToStringBuilder(new string[] { "B8", "CO", "EB" }));

            Assert.AreEqual(2, Letters.Count);
            Assert.AreEqual(5, Index.Count);

            Assert.AreEqual('B', Letters[Index['B']]);
            Assert.AreEqual('B', Letters[Index['8']]);
            Assert.AreEqual('B', Letters[Index['E']]);
            Assert.AreEqual('C', Letters[Index['C']]);
            Assert.AreEqual('C', Letters[Index['O']]);
        }

        [TestMethod]
        public void Test_BuildIndex()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.buildIndex(Letters, Index, ToStringBuilder(new string[] { "B83", "CO", "DO", "EB", "GbO", "LlI1t" }));

            Assert.AreEqual('B', Letters[Index['B']]);
            Assert.AreEqual('B', Letters[Index['8']]);
            Assert.AreEqual('B', Letters[Index['3']]);
            Assert.AreEqual('B', Letters[Index['E']]);
            Assert.AreEqual('C', Letters[Index['O']]);
            Assert.AreEqual('C', Letters[Index['D']]);
        }

        [TestMethod]
        public void Test_BuildIndex_One_Letter()
        {
            List<char> Letters = new List<char>();
            Dictionary<char, int> Index = new Dictionary<char, int>();

            SimilarLookingProcessor.buildIndex(Letters, Index, ToStringBuilder(new string[] { "A", "B", "C" }));

            Assert.AreEqual(0, Letters.Count);
            Assert.AreEqual(0, Index.Count);
        }

        [TestMethod]
        public void Test_Small_Process()
        {
            SimilarLookingProcessor proc = new SimilarLookingProcessor(new string[] { "B83", "OD0" });

            Assert.AreEqual("BIBBLE", proc.process("BI83LE"));
            Assert.AreEqual("BIBBLE", proc.process("BIBBLE"));
            Assert.AreEqual("GOOO", proc.process("G0OD"));
            Assert.AreEqual("GOOO", proc.process("GOOD"));
            Assert.AreEqual("GOOO", proc.process("G0DO"));

            Assert.AreEqual("Chicken", proc.process("Chicken"));
            Assert.AreEqual("nothing", proc.process("nothing"));
        }

        /// <summary>
        /// This test is difficult to predict because the set is large, and could
        /// be changed by the programmer for tuning reasons.
        /// </summary>
        [TestMethod]
        public void Test_Default_Process()
        {
            SimilarLookingProcessor proc = new SimilarLookingProcessor();

            Assert.AreEqual("BIBBIB", proc.process("Bl83LE"));
        }
    }
}
