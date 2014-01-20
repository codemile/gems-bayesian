using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class AlphaNumericProcessorTest
    {
        /// <summary>
        /// A list of strings that should be rejected.
        /// </summary>
        private readonly string[] _bad =
        {
            " ",
            "A$@SD#",
            "123asdn",
            "$123.39",
            "one two three"
        };

        /// <summary>
        /// A list of strings that should be accepted.
        /// </summary>
        private readonly string[] _good =
        {
            "price",
            "sales",
            "something",
            "Asd2392",
            "a2s2s2"
        };

        [TestMethod]
        public void Test_Bad_Strings()
        {
            AlphaNumericProcessor proc = new AlphaNumericProcessor();
            foreach (string str in _bad)
            {
                Assert.IsNull(proc.Process(str));
            }
        }

        [TestMethod]
        public void Test_Good_Strings()
        {
            AlphaNumericProcessor proc = new AlphaNumericProcessor();
            foreach (string str in _good)
            {
                Assert.IsNotNull(proc.Process(str));
            }
        }
    }
}