using System.Collections.Generic;

namespace WebCrawler.Console.Model
{
    public class Node
    {
        private readonly string _key;
        private readonly HashSet<string> _adjacencyKeySet = new HashSet<string>();

        public Node(string key)
        {
            _key = key;
        }

        public string GetKey()
        {
            return _key;
        }

        public void AddEdge(string key)
        {
            _adjacencyKeySet.Add(key);
        }

        public List<string> GetAdjacentKeys()
        {
            var list = new List<string>(_adjacencyKeySet);
            list.Sort();
            return list;
        }
    }
}
