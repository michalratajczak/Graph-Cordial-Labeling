using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace CordialLabeling.Core
{
    public class Graph
    {
        public int N { get; private set; }
        public int K { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; private set; }
        public int[][] AdjacencyMatrix { get; private set; }

        public Graph(int n, int k, int[][] adjacencyMatrix)
        {
            N = n;
            K = k;
            Vertices = new List<Vertex>(n);
            Edges = new List<Edge>(k);
            AdjacencyMatrix = adjacencyMatrix;

            for (int i = 0; i < n; i++)
            {
                Vertices.Add(new Vertex(i));
            }

            CalculateVertices();
            CalculateEdges();
        }

        private Graph(int n)
        {
            Vertices = new List<Vertex>(n);
            Edges = new List<Edge>(n * (n - 1) / 2);
            AdjacencyMatrix = new int[n][];
            N = n;

            for (int i = 0; i < n; i++)
            {
                AdjacencyMatrix[i] = new int[n];
                Vertices.Add(new Vertex(i));
            }
        }

        private Graph(int n, int k)
        {
            Vertices = new List<Vertex>(n);
            Edges = new List<Edge>(k);
            AdjacencyMatrix = new int[n][];
            N = n;
            K = k;

            for (int i = 0; i < n; i++)
            {
                AdjacencyMatrix[i] = new int[n];
                Vertices.Add(new Vertex(i));
            }
        }

        private void CalculateVertices()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (AdjacencyMatrix[i][j] == 1)
                    {
                        Vertices[i].Neighbors.Add(Vertices[j]);
                    }
                }
            }
        }

        private void CalculateEdges()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (AdjacencyMatrix[i][j] == 1)
                    {
                        var edge = new Edge(Edges.Count, Vertices[i], Vertices[j]);
                        Edges.Add(edge);
                        Vertices[i].Edges.Add(edge);
                        Vertices[j].Edges.Add(edge);
                    }
                }
            }
        }

        public bool IsGraphConnected()
        {
            List<Vertex> visited = new List<Vertex>();
            CheckNextVertex(Vertices.First(), visited);
            return visited.Count == Vertices.Count;
        }

        private void CheckNextVertex(Vertex vertex, List<Vertex> visited)
        {
            if (!visited.Contains(vertex))
            {
                visited.Add(vertex);
            }
            foreach (var v in vertex.Neighbors)
            {
                if (!visited.Contains(v))
                {
                    CheckNextVertex(v, visited);
                }
            }
        }

        public static Graph Generate(int n, double p, bool isConnected)
        {
            Random random = new Random();
            Graph graph;
            do
            {
                graph = new Graph(n);

                for (int i = 0; i < n; i++)
                {
                    graph.AdjacencyMatrix[i][i] = 0;

                    for (int j = i + 1; j < n; j++)
                    {
                        if (random.NextDouble() < p)
                        {
                            graph.AdjacencyMatrix[i][j] = 1;
                            graph.AdjacencyMatrix[j][i] = 1;
                        }
                        else
                        {
                            graph.AdjacencyMatrix[i][j] = 0;
                            graph.AdjacencyMatrix[j][i] = 0;
                        }
                    }
                }

                graph.CalculateVertices();
                graph.CalculateEdges();
                graph.K = graph.Edges.Count;
            }
            while (!graph.IsGraphConnected() && isConnected);

            return graph;
        }

        public static Graph Generate(int n, int k, bool isConnected)
        {
            Random random = new Random();
            Graph graph;
            do
            {
                graph = new Graph(n, k);

                int index = 0;

                while(index < k)
                {
                    int i = random.Next(0, n);
                    int j = random.Next(0, n);

                    if(graph.AdjacencyMatrix[i][j] != 1)
                    {
                        graph.AdjacencyMatrix[i][j] = 1;
                        graph.AdjacencyMatrix[j][i] = 1;
                        index++;
                    }
                }

                graph.CalculateVertices();
                graph.CalculateEdges();
            }
            while (!graph.IsGraphConnected() && isConnected);

            return graph;
        }

        public static Graph ReadFromMatrix(int[][] adjacencyMatrix)
        {
            int n = adjacencyMatrix.Length;
            int k = 0;
            for(int i = 0; i < n; i++)
            {
                for(int j = i; j < n; j++)
                {
                    k += adjacencyMatrix[i][j];
                }
            }

            return new Graph(n, k, adjacencyMatrix);
        }

        public static Graph ReadFromMatrixFile(string fileName)
        {
            var data = File.ReadAllLines(fileName);

            Regex regex = new Regex(@"(\d)");
            var matches = new Match[data.Length][];

            for(int i = 0; i < data.Length; i++)
            {
                matches[i] = regex.Matches(data[i]).ToArray();
            }

            return ReadFromMatrix(matches.Select(m => m.Select(e => int.Parse(e.Value)).ToArray()).ToArray());
        }

        public void UpdateVerticesLabel(int[] labels)
        {
            for(int i = 0; i < labels.Length; i++)
            {
                Vertices[i].Label = labels[i];
            }
        }

        public void UpdateVerticesLabel()
        {
            var data = File.ReadAllText("output.txt");
            Regex regex = new Regex(@"(\d)");
            var matches = regex.Matches(data).ToArray();

            for(int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Label = int.Parse(matches[i].Value);
                Debug.WriteLine(matches[i].Value);
            }           
        }

        public void ExportToMiniZincFile()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"n = {N};");
            builder.AppendLine($"k = {K};");
            builder.Append($"edges = [");

            foreach(var edge in Edges)
            {
                builder.Append($"|{edge.A.Index},{edge.B.Index}");
            }

            builder.Append($"|];");

            File.WriteAllText("data.dzn", builder.ToString());
        }

        public void ExportToGraphvizFile()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("// Cordial Labeling");
            s.AppendLine("graph {");
            foreach(var v in Vertices)
            {
                s.AppendLine($"\t{v.Index} [label=\"[{v.Index}]: {v.Label}\"]");
            }
            foreach(var e in Edges)
            {
                s.AppendLine($"\t{e.A.Index} -- {e.B.Index} [label={e.Label} constraint=true]");
            }
            s.AppendLine("}");

            File.WriteAllText("graph.gv", s.ToString());
        }

        public string ExecuteMiniZinc()
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ".\\MiniZinc\\minizinc.exe",
                    Arguments = "--solver Gecode .\\CordialLabelingWithParameters.mzn .\\data.dzn",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string result = proc.StandardOutput.ReadLine();
            proc.WaitForExit();
            proc.Close();

            return result;
        }
    }
}
