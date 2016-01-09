using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gothandy.Tree.Extensions;

namespace Gothandy.Tree.Test
{
    [TestClass]
    public class StringTreeTests
    {
        [TestMethod]
        public void RootOnly()
        {
            var root = new StringTree("Root");
            Assert.AreEqual("Root", root.Item);
        }

        [TestMethod]
        public void AddChild()
        {
            var root = new StringTree("Root")
            {
                new StringTree("Child")
            };

            Assert.IsInstanceOfType((root[0]), typeof(StringTree));
            Assert.AreEqual("Child", root[0].Item);
            Assert.AreEqual("Root", root[0].Parent.Item);
        }

        [TestMethod]
        public void AddChildren()
        {
            var root = addChildren();
            Assert.AreEqual(3, root.Count);
        }

        [TestMethod]
        public void AddChildrenParent()
        {
            var root = addChildren();
            Assert.AreEqual(3, root[0].Parent.Count);
        }

        [TestMethod]
        public void AddChildrenAncestors()
        {
            var root = addChildren();
            Assert.AreEqual("Root",
                string.Join(",",
                root[1].GetAncestors()
            ));
        }

        [TestMethod]
        public void AddChildrenDescendants()
        {
            var root = addChildren();
            Assert.AreEqual("Root,Child1,Child2,Child3",
                string.Join(",",
                root.GetDescendants()
            ));
        }

        [TestMethod]
        public void AddChildrenDescendantsFind()
        {
            var root = addChildren();
            Assert.AreEqual("Child2",
                root.GetDescendants().Find(e => e.Item == "Child2").Item
            );
        }

        [TestMethod]
        public void AddChildrenSet()
        {
            var root = addChildren();

            root[1] = new StringTree("NewChild2");

            Assert.AreEqual("NewChild2", root[1].Item);
            Assert.AreEqual("Root", root[1].Parent.Item);
        }

        private static StringTree addChildren()
        {
            return new StringTree("Root")
            {
                new StringTree("Child1"),
                new StringTree("Child2"),
                new StringTree("Child3")
            };
        }
    }
}
