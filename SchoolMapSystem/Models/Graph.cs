using System;
using System.IO;

namespace SchoolMapSystem
{
    class Graph
    {
        private int[,] AdjMatrix = new int[23, 23];
        private string filename = "CSVAdjMatrix.txt";

        public int[,] GetGraph() // Reads data from a txt file and generates an adjacency matrix from it
        {
            // Read from the file
            using (StreamReader sr = new StreamReader(filename, true))
            {
                string line;
                int i = 0;
                // Parse the data and create the adjacency matrix
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    for (int j = 0; j < values.Length; j++)
                    {
                        AdjMatrix[i, j] = int.Parse(values[j]);
                    }

                    i++;
                }
            }
            // Return the adjacency matrix
            return AdjMatrix;
        }

        public void PrintMatrix() // Prints the adjacency matrix
        {
            for (int i = 0; i <= 20; i++)
            {
                for (int j = 0; j <= 20; j++)
                {
                    Console.Write(AdjMatrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}

