using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.PathFinding
{
    public class GraphTraverser
    {
        public Graph Graph { get; set; }
        public bool[] visited;
        public GraphTraverser(Graph graph)
        {
            this.Graph = graph;
             visited = new bool[this.Graph.Size];
        }

       
        public void TraverseDFS(int v)

        {

            if (!visited[v])

            {

                Console.Write(v + " ");

                visited[v] = true;

                foreach (int child in Graph.GetSuccessors(v))

                {

                    TraverseDFS(child);

                }

            }

        }
    }
}
