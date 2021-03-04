using SecretProject.Class.StageFolder;
using System.Collections.Generic;
using XMLData.RouteStuff;

namespace SecretProject.Class.PathFinding
{
    public class Graph
    {
        public List<StagesEnum>[] childNodes;
        public int Size { get { return childNodes.Length; } }
        public Graph(int size)
        {
            childNodes = new List<StagesEnum>[size];
            for (int i = 0; i < size; i++)
            {
                childNodes[i] = new List<StagesEnum>();
            }
        }

        public Graph(List<StagesEnum>[] childNodes)
        {
            this.childNodes = childNodes;
        }

        public void AddEdge(StagesEnum baseNode, StagesEnum nodeToConnect)
        {
            childNodes[(int)baseNode].Add(nodeToConnect);
        }

        public void RemoveEdge(StagesEnum baseNode, StagesEnum nodeToConnect)
        {
            childNodes[(int)baseNode].Remove(nodeToConnect);
        }

        public bool HasEdge(StagesEnum baseNode, StagesEnum nodeToConnect)
        {
            bool hasEdge = childNodes[(int)baseNode].Contains(nodeToConnect);
            return hasEdge;
        }

        public IList<StagesEnum> GetSuccessors(int v)

        {
            return childNodes[v];
        }


    }
}
