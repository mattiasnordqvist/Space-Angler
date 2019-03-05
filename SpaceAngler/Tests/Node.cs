using System.Collections.Generic;

namespace Tests
{
    public class Node
    {
        public int Id { get; set; }
        public int? Parent_Id { get; set; }
        public int? L { get; set; }
        public int? R { get; set; }
        public bool? IsLeaf { get; set; }

        public Node Parent { get; set; }

        public List<Node> Children { get; } = new List<Node>();
    }
}
