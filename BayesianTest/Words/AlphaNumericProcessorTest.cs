using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class AlphaNumericProcessorTest
    {
        /// <summary>
        /// A list of strings that should be accepted.
        /// </summary>
        private string[] good = {
                                    "price",
                                    "sales",
                                    "something",
                                    "Asd2392",
                                    "a2s2s2"
                                };

        /// <summary>
        /// A list of strings that should be rejected.
        /// </summary>
        private string[] bad = {
                                   " ",
                                   "A$@SD#",
                                   "123asdn",
                                   "$123.39",
                                   "one two three"
                               };

        [TestMethod]
        public void Test_Good_Strings()
        {
            AlphaNumericProcessor proc = new AlphaNumericProcessor();
            foreach (string str in good)
            {
                Assert.IsNotNull(proc.process(str));
            }
        }

        [TestMethod]
        public void Test_Bad_Strings()
        {
            AlphaNumericProcessor proc = new AlphaNumericProcessor();
            foreach (string str in bad)
            {
                Assert.IsNull(proc.process(str));
            }
        }
    }
}
