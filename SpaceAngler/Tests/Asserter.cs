using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Tests
{
    public class Asserter
    {
        private Dictionary<int, Node> list;
        private readonly Dictionary<int, Node> expected;

        public Asserter(Dictionary<int, Node> list, string expected)
        {
            this.list = list;
            this.expected = new Tree(expected).SetLR().Nodes;
        }

       

        public void AssertAll()
        {
            var values = list.Values;
            var leaves = values.Where(x => x.IsLeaf.Value).ToList();
            NUnit.Framework.Assert.IsTrue(leaves.All(x => x.L != null && x.R != null), "There are nodes without L or R.");
            NUnit.Framework.Assert.IsTrue(leaves.All(x => x.L + 1 == x.R), "There are leaves where L + 1 not equals R");
            NUnit.Framework.Assert.IsTrue(values.All(x => !leaves.Any(y => y.Id == x.Parent_Id)), "There are regular nodes with IsLeaf = true");
            NUnit.Framework.Assert.IsTrue(values.All(x => x.L < x.R), "There are nodes where L is not less than R");
            var lrs = values.Select(x => x.L).Union(values.Select(x => x.R)).OrderBy(x => x).Select((x, i) => new { x, i }).ToArray();
            foreach (var lr in lrs.Skip(1).ToList())
            {
                NUnit.Framework.Assert.IsTrue(lrs[lr.i - 1].x < lr.x, "Ls and Rs are not unique");
            }
            foreach (var e in expected)
            {
                Assert(e.Key, e.Value.L.Value, e.Value.R.Value, e.Value.IsLeaf.Value);
            }
        }

        internal void Assert(int id, int left, int right, bool leaf)
        {
            var node = list[id];
            NUnit.Framework.Assert.AreEqual(left, node.L, "Node left value is not correct");
            NUnit.Framework.Assert.AreEqual(right, node.R, "Node right value is not correct");
            NUnit.Framework.Assert.AreEqual(leaf, node.IsLeaf, "Node leaf status is not correct");
        }
    }
}
