using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tests
{
    public class Tree
    {
        public Tree(string tree)
        {
            var nodes = new List<Node>();
            var matches = Regex.Matches(tree, @"(?<node>(> )*([0-9]+)(\r\n)?)", RegexOptions.ExplicitCapture);
            var list = matches.Select(x => x.Groups[0].Captures[0].Value).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var parent = new Stack<Node>();
            var lastLevel = 0;
            foreach (var e in list)
            {
                var splits = e.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var level = splits.Count();
                var number = int.Parse(splits.Last());
                while (lastLevel >= level)
                {
                    lastLevel--;
                    parent.Pop();

                }
                if (lastLevel == 0)
                {
                    var node = new Node() { Id = number };
                    nodes.Add(node);
                    parent.Push(node);
                }

                else if (lastLevel + 1 == level)
                {
                    var node = new Node() { Id = number, Parent_Id = parent.Peek().Id, Parent = parent.Peek() };
                    parent.Peek().Children.Add(node);
                    nodes.Add(node);
                    parent.Push(node);
                }

                lastLevel = level;
            }

            Nodes = nodes.ToDictionary(x => x.Id, x => x);
        }

        public Dictionary<int, Node> Nodes { get; }

        public Tree SetLR()
        {
            int v = 0;
            foreach (var n in Nodes.Where(x => x.Value.Parent == null))
            {
                v = SetRL(n.Value, v);
            }
            return this;
        }

        private int SetRL(Node n, int v)
        {
            n.L = ++v;
            foreach (var child in n.Children)
            {
                v = SetRL(child, v);
            }
            n.R = ++v;
            n.IsLeaf = !n.Children.Any();
            return v;
        }
    }
}
