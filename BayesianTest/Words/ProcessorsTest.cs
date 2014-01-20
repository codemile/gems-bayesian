using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class ProcessorsTest
    {
        [TestMethod]
        public void Test_Empty()
        {
            Processors proc = new Processors();
            Assert.AreEqual("something", proc.Process("something"));
        }

        [TestMethod]
        public void Test_Mock()
        {
            MockProcessor mock = new MockProcessor();
            Processors proc = new Processors {mock};

            Assert.AreEqual("something", proc.Process("something"));
            Assert.AreEqual(1, mock.Count);

            Assert.AreEqual("something", proc.Process("something"));
            Assert.AreEqual(2, mock.Count);
        }

        [TestMethod]
        public void Test_Null()
        {
            MockProcessor mock = new MockProcessor();
            Processors proc = new Processors {new MockProcessor(), new TestProcessor(), new NullProcessor(), mock};

            Assert.IsNull(proc.Process("something"));
            Assert.AreEqual(0, mock.Count);
        }

        [TestMethod]
        public void Test_Test()
        {
            TestProcessor test = new TestProcessor();
            Processors proc = new Processors {test};

            Assert.AreEqual("test", proc.Process("something"));
            Assert.AreEqual(1, test.Count);

            Assert.AreEqual("test", proc.Process("something"));
            Assert.AreEqual(2, test.Count);
        }

        private class MockProcessor : iWordProcessor
        {
            public int Count;

            public string Process(string pStr)
            {
                Count++;
                return pStr;
            }
        }

        private class NullProcessor : iWordProcessor
        {
            private int _count;

            public string Process(string pStr)
            {
                _count++;
                return null;
            }
        }

        private class TestProcessor : iWordProcessor
        {
            public int Count;

            public string Process(string pStr)
            {
                Count++;
                return "test";
            }
        }
    }
}