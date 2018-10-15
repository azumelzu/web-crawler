using System.Collections.Generic;
using System.Linq;
using WebCrawler.Console.Extensions;

namespace WebCrawler.Console.Model
{
    public class HyperLinksGraph
    {
        private readonly List<Node> _nodes = new List<Node>();

        public void AddNode(string key, List<string> edges)
        {
            if (!ContainsNode(key))
            {
                _nodes.Add(new Node(key));

                if (!edges.IsNullOrEmpty())
                {
                    edges.ForEach(e =>
                    {
                        AddEdge(key, e);
                    });
                }
            }
        }

        public void AddEdge(string key1, string key2)
        {
            _nodes.First(n => n.GetKey() == key1).AddEdge(key2);
        }

        public List<string> GetAdjacentKeys(string v)
        {
            var n = _nodes.FirstOrDefault(x => x.GetKey() == v);
            return n != null ? n.GetAdjacentKeys() : new List<string>();
        }

        public bool ContainsNode(string key)
        {
            return _nodes.Any(n => n.GetKey() == key);
        }

        public Node First()
        {
            return _nodes.First();
        }
    }
}