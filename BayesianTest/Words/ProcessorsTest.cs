using Bayesian.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianTest.Words
{
    [TestClass]
    public class ProcessorsTest
    {
        public class MockProcessor : iWordProcessor
        {
            public int Count = 0;
            public string process(string pStr)
            {
                Count++;
                return pStr;
            }
        }

        public class TestProcessor : iWordProcessor
        {
            public int Count = 0;
            public string process(string pStr)
            {
                Count++;
                return "test";
            }
        }

        public class NullProcessor : iWordProcessor
        {
            public int Count = 0;
            public string process(string pStr)
            {
                Count++;
                return null;
            }
        }

        [TestMethod]
        public void Test_Empty()
        {
            Processors proc = new Processors();
            Assert.AreEqual("something", proc.process("something"));
        }

        [TestMethod]
        public void Test_Mock()
        {
            MockProcessor mock = new MockProcessor();
            Processors proc = new Processors();
            proc.Add(mock);

            Assert.AreEqual("something", proc.process("something"));
            Assert.AreEqual(1, mock.Count);

            Assert.AreEqual("something", proc.process("something"));
            Assert.AreEqual(2, mock.Count);
        }

        [TestMethod]
        public void Test_Test()
        {
            TestProcessor test = new TestProcessor();
            Processors proc = new Processors();
            proc.Add(test);

            Assert.AreEqual("test", proc.process("something"));
            Assert.AreEqual(1, test.Count);

            Assert.AreEqual("test", proc.process("something"));
            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void Test_Null()
        {
            MockProcessor mock = new MockProcessor();
            Processors proc = new Processors();
            proc.Add(new MockProcessor());
            proc.Add(new TestProcessor());
            proc.Add(new NullProcessor());
            proc.Add(mock);

            Assert.IsNull(proc.process("something"));
            Assert.AreEqual(0, mock.Count);
        }

    }
}
