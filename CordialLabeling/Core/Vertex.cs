using System;
using System.Collections.Generic;
using System.Text;

namespace CordialLabeling.Core
{
    public class Vertex
    {
        public int Index { get; private set; }
        public int Label { get; set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Neighbors { get; private set; }

        public Vertex(int index)
        {
            Index = index + 1;
            Edges = new List<Edge>();
            Neighbors = new List<Vertex>();
            Label = 0;
        }
    }
}
