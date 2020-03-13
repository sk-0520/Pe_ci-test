using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class FixedQueueTest
    {
        #region function

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentException>(() => new FixedQueue<int>(0));
            Assert.ThrowsException<ArgumentException>(() => new FixedQueue<int>(-1));
            new FixedQueue<int>(1);
            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow(0, 1, 0)]
        [DataRow(1, 1, 1)]
        [DataRow(2, 2, 5)]
        [DataRow(5, 7, 5)]
        public void EnqueueTest(int result, int limit, int count)
        {
            var fx = new FixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(0, count)) {
                fx.Enqueue(i);
            }
            Assert.AreEqual(result, fx.Count);
        }

        #endregion
    }

    [TestClass]
    public class ConcurrentFixedQueueTest
    {
        #region function

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentException>(() => new ConcurrentFixedQueue<int>(0));
            Assert.ThrowsException<ArgumentException>(() => new ConcurrentFixedQueue<int>(-1));
            new ConcurrentFixedQueue<int>(1);
            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow(0, 1, 0)]
        [DataRow(1, 1, 1)]
        [DataRow(2, 2, 5)]
        [DataRow(5, 7, 5)]
        public void EnqueueTest(int result, int limit, int count)
        {
            var fx = new ConcurrentFixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(1, count)) {
                fx.Enqueue(i);
            }
            Assert.AreEqual(result, fx.Count);
        }

        [TestMethod]
        public void ConcurrentEnqueueTest()
        {
            var limit = 100;
            var fx = new ConcurrentFixedQueue<int>(limit);
            Parallel.ForEach(Enumerable.Range(0, ushort.MaxValue), i => { fx.Enqueue(i); });
            Assert.AreEqual(limit, fx.Count);
        }
        #endregion
    }

}