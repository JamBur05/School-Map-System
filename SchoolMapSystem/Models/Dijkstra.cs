namespace SchoolMapSystem
{
    class Dijkstra
    {
        private readonly int NO_PARENT = -1;
        private Stack NodeStack;
        private Graph grp;
        private int[,] adjMatrix;
        private int totalTime = 0;

        public Dijkstra() // Constructor for the Dijkstra class
        {
            // Create a new graph object and stack to store nodes
            grp = new Graph();
            NodeStack = new Stack(24);
            adjMatrix = grp.GetGraph();
        }

        public int GetTime()    // Mutator method for totalTime variable.
        {
            return totalTime;
        }

        public Stack GetStack() // GetStack method to return the NodeStack object
        {
            return NodeStack;
        }

        public void RunDijkstra(int startNode, int endNode) // This method runs the Dijkstra algorithm to find the shortest path from startNode to endNode
        {

            int nNodes = adjMatrix.GetLength(0);
            bool[] found = new bool[nNodes];                // found will be true if the current node is
                                                            // contained in the path
            int[] shortestDistance = new int[nNodes];       // This array will contain the distance 
                                                            //from the source to the current node

            for (int nodeIndex = 0; nodeIndex < nNodes; nodeIndex++)    // Loop through all nodes and set their distance to the
            {                                                           // max integer value and set found to false to clear the path
                shortestDistance[nodeIndex] = int.MaxValue;
                found[nodeIndex] = false;
            }

            shortestDistance[startNode] = 0;    // Source node distance from itself is 0
            int[] pathList = new int[nNodes];   // Store the path itself
            pathList[startNode] = NO_PARENT;    // The starting vertex doesnt have a parent


            // Loop through all nodes and find the shortest path
            for (int i = 1; i < nNodes - 1; i++)
            {
                int closestNode = -1;
                int distanceShort = int.MaxValue;
                // Loop through all nodes to find the closest node
                for (int nodeIndex = 0; nodeIndex < nNodes; nodeIndex++)
                {
                    if (shortestDistance[nodeIndex] < distanceShort && found[nodeIndex] == false)
                    {
                        closestNode = nodeIndex;
                        distanceShort = shortestDistance[nodeIndex];
                    }
                }

                found[closestNode] = true;

                // Update the distances and path for each node
                for (int nodeIndex = 0; nodeIndex < nNodes; nodeIndex++)
                {
                    int edgeScore = adjMatrix[closestNode, nodeIndex];

                    if (edgeScore > 0 && ((distanceShort + edgeScore) < shortestDistance[nodeIndex]))
                    {
                        pathList[nodeIndex] = closestNode;
                        shortestDistance[nodeIndex] = distanceShort + edgeScore;
                    }
                }
            }
            totalTime = shortestDistance[endNode];
            DisplayPath(startNode, shortestDistance, pathList, endNode); // Display the shortest path
        }

        public void DisplayPath(int startNode, int[] shortestDistance, int[] pathList, int endNode) // Function to sort through path
        {
            int nVertices = shortestDistance.Length;

            // If the end node is not equal to the starting node, create the neighbor list for the end node
            if (endNode != startNode)
            {
                // Create neighbor list
                CreateNeighbourList(endNode, pathList, 100);
            }
        }

        public void CreateNeighbourList(int endNode, int[] pathList, int depth)
        {
            // Check if depth is less than or equal to 0
            if (depth <= 0)
            {
                return;
            }
            
            // Check if endNode is the starting node or has no parent
            if (endNode == NO_PARENT)
            {
                return;
            }
            // Recursively call CreateNeighbourList with parent node and decremented depth
            CreateNeighbourList(pathList[endNode], pathList, depth - 1);
            // Push current node onto NodeStack
            NodeStack.Push(endNode);
        }
    }
}
