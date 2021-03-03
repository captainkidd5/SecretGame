using System.Collections.Generic;
using XMLData.RouteStuff;

namespace SecretProject.Class.PathFinding
{
    public class Graph
    {
        public List<Stages>[] childNodes;
        public int Size { get { return childNodes.Length; } }
        public Graph(int size)
        {
            childNodes = new List<Stages>[size];
            for (int i = 0; i < size; i++)
            {
                childNodes[i] = new List<Stages>();
            }
        }

        public Graph(List<Stages>[] childNodes)
        {
            this.childNodes = childNodes;
        }

        public void AddEdge(Stages baseNode, Stages nodeToConnect)
        {
            childNodes[(int)baseNode].Add(nodeToConnect);
        }

        public void RemoveEdge(Stages baseNode, Stages nodeToConnect)
        {
            childNodes[(int)baseNode].Remove(nodeToConnect);
        }

        public bool HasEdge(Stages baseNode, Stages nodeToConnect)
        {
            bool hasEdge = childNodes[(int)baseNode].Contains(nodeToConnect);
            return hasEdge;
        }

        public IList<Stages> GetSuccessors(int v)

        {
            return childNodes[v];
        }


    }
}
