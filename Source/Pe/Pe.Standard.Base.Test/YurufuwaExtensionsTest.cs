using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class IEnumerableExtensionsTest
    {
        [Fact]
        public void CountingTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting().ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i, counting[i].Number);
            }
        }

        [Fact]
        public void Counting_Base_Test()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting(99).ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i + 99, counting[i].Number);
            }
        }

        [Theory]
        [InlineData("a,b,c", new[] { "a", "b", "c" }, ",")]
        [InlineData("abc", new[] { "a", "b", "c" }, "")]
        [InlineData("abc", new[] { "a", "b", "c" }, null)]
        public void JoinStringTest(string expected, IEnumerable<string> source, string? separator)
        {
            var actual = source.JoinString(separator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, new[] { "a", "b", "c" }, Order.Ascending)]
        [InlineData(new[] { "a", "b", "c" }, new[] { "c", "b", "a" }, Order.Ascending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "a", "b", "c" }, Order.Descending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "c", "b", "a" }, Order.Descending)]
        public void OrderByTest(IEnumerable<string> expected, IEnumerable<string> source, Order order)
        {
            var actual = source.OrderBy(order, a => a);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, new object[] {1, 2, 3 })]
        [InlineData(false, new object[] { "a", "b", "c" })]
        [InlineData(true, new object[] { })]
        [InlineData(true, new object[] { "a" })]
        [InlineData(true, new object[] { "a", "a" })]
        [InlineData(false, new object?[] { "a", null })]
        [InlineData(true, new object[] { 1 })]
        [InlineData(true, new object[] { 1, 1 })]
        [InlineData(false, new object?[] { 1, null })]
        [InlineData(true, new object?[] { null })]
        [InlineData(true, new object?[] { null, null })]
        [InlineData(false, new object?[] { null, "str" })]
        [InlineData(false, new object?[] { null, 1 })]
        public void AllEqualsTest<T>(bool expected, IEnumerable<object?> source)
        {
            var actual = source.AllEquals();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllEquals_throw_Test()
        {
            IEnumerable<object> source = null!;
            Assert.Throws<ArgumentNullException>(() => source.AllEquals());
        }
    }

    public class CollectionExtensionsTest
    {
        #region function

        [Fact]
        public void SetRange_List_Test()
        {
            var test = new List<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void SetRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void AddRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.AddRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 1, 2, 3, 10, 11, 12 }, test);
        }

        #endregion
    }

    public class IReadOnlyCollectionExtensionsTest
    {
        [Fact]
        public void IndexOf_IReadOnlyList_Test()
        {
            IReadOnlyList<int> items = new[] { 10, 20, 30 }.ToList();
            var actual = items.IndexOf(20);
            Assert.Equal(1, actual);
            Assert.Equal(-1, items.IndexOf(40));
        }

        private class Test_IndexOf_IReadOnlyCollection: IReadOnlyCollection<int>
        {
            private static int[] Items { get; } = new[] { 10, 20, 30 };

            public int Count => Items.Length;

            public IEnumerator<int> GetEnumerator()
            {
                return Items.AsEnumerable().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Fact]
        public void IndexOf_IReadOnlyCollection_Test()
        {
            IReadOnlyCollection<int> items = new Test_IndexOf_IReadOnlyCollection();
            var actual = items.IndexOf(20);
            Assert.Equal(1, actual);

            Assert.Equal(-1, items.IndexOf(40));
        }
    }

    public class IEnumerableNonGenericsExtensionsTest
    {
        #region function

        [Fact]
        public void NonGenericsAny_normal_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny();
            Assert.True(actual);
        }

        [Fact]
        public void NonGenericsAny_empty_Test()
        {
            var input = Array.Empty<int>();
            var actual = input.NonGenericsAny();
            Assert.False(actual);
        }

        [Fact]
        public void NonGenericsAny_predicate_30_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny(a => 20 < (int)a!);
            Assert.True(actual);
        }

        [Fact]
        public void NonGenericsAny_predicate_empty_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny(a => (int)a! <= 0);
            Assert.False(actual);
        }

        #endregion
    }
}
