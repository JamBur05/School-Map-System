using System.Collections.Generic;
using System.Windows.Shapes;

namespace SchoolMapSystem.Models
{
    class NodeDictionary
    {
        private Dictionary<int, Rectangle> nodes;

        public NodeDictionary(Rectangle nodeA1, Rectangle nodeA2, Rectangle nodeA3, Rectangle nodeA4, Rectangle nodeA5, Rectangle nodeA6, Rectangle nodeA7, Rectangle nodeA8, Rectangle nodeA9, Rectangle nodeA10, Rectangle nodeA11, Rectangle nodeA12, Rectangle nodeA13, Rectangle nodeA14, Rectangle nodeA15, Rectangle nodeA16, Rectangle nodeA17, Rectangle nodeA18, Rectangle nodeA19, Rectangle nodeA20, Rectangle nodeA21, Rectangle nodeA22, Rectangle nodeA23)
        {
            nodes = new Dictionary<int, Rectangle>();
            nodes.Add(0, nodeA1);
            nodes.Add(1, nodeA2);
            nodes.Add(2, nodeA3);
            nodes.Add(3, nodeA4);
            nodes.Add(4, nodeA5);
            nodes.Add(5, nodeA6);
            nodes.Add(6, nodeA7);
            nodes.Add(7, nodeA8);
            nodes.Add(8, nodeA9);
            nodes.Add(9, nodeA10);
            nodes.Add(10, nodeA11);
            nodes.Add(11, nodeA12);
            nodes.Add(12, nodeA13);
            nodes.Add(13, nodeA14);
            nodes.Add(14, nodeA15);
            nodes.Add(15, nodeA16);
            nodes.Add(16, nodeA17);
            nodes.Add(17, nodeA18);
            nodes.Add(18, nodeA19);
            nodes.Add(19, nodeA20);
            nodes.Add(20, nodeA21);
            nodes.Add(21, nodeA22);
            nodes.Add(22, nodeA23);
        }

        public void Add(int nodeNumber, Rectangle node) // Add node to dictionary
        {
            nodes.Add(nodeNumber, node);
        }

        public Rectangle Get(int nodeNumber) // Getter, returns the node at a certain key
        {
            return nodes[nodeNumber];
        }
    }
}
