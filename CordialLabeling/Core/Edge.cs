using System;
using System.Collections.Generic;
using System.Text;

namespace CordialLabeling.Core
{
    public class Edge
    {
        public int Index { get; private set; }
        public int Label => Math.Abs(A.Label - B.Label);
        public Vertex A { get; private set; }
        public Vertex B { get; private set; }

        public Edge(int index, Vertex a, Vertex b)
        {
            Index = index;
            A = a;
            B = b;
        }
    }
}
