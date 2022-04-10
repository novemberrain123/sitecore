using System;

namespace a
{
    public static class Extensions
    {
        //Find if string is palindrome
        public static bool IsPalindrome(this string str, string trash)
        {
            char[] trashArray = trash.ToCharArray();
            int n = str.Length - 1;
            int i = 0;
            while (n > i)
            {
                while (trashArray.Contains(str[i]))
                {
                    i++;
                }
                while (trashArray.Contains(str[n]))
                {
                    n--;
                    if (i >= n)
                    {
                        goto exit;
                    }
                }

                if (!str[i].Equals(str[n]))
                {
                    return false;
                }

                i++;
                n--;
            }
        exit:
            return true;
        }

        //Generate edges for vertices
        public static List<Tuple<int, int>> generateEdges(List<int> vertices)
        {
            var edges = new List<Tuple<int, int>>();
            var added = new List<Tuple<int, int>>();
            foreach (int i in vertices)
            {
                int[] possibleNeighbor = new int[] { i - 1, i + 1, i - 11, i - 10, i - 9, i + 9, i + 10, i + 11 };
                foreach (int j in vertices)
                {
                    if (possibleNeighbor.Contains(j))
                    {
                        if (added.Contains(Tuple.Create(j, i)))
                        {
                            continue;
                        }
                        else
                        {
                            edges.Add(Tuple.Create(i, j));
                            added.Add(Tuple.Create(j, i));
                        }
                    }

                }
            }
            return edges;
        }


    }
    public class Graph<T>
    {
        public Graph() { }
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }
    }
    public class Algorithms
    {
        //breadth-first search based path-finding function to cross minefield.
        public Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v =>
            {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter 1 for Palindrome, 2 for Minefield(-999 to exit): ");
                string s = Console.ReadLine();
                switch (s)
                {
                    case "-999":
                        break;
                    case "1": //Problem 2
                        while (true)
                        {
                            Console.WriteLine("Enter InputString(-999 to exit): ");
                            string input = Console.ReadLine().ToLower();
                            if (input.Equals("-999"))
                            {
                                break;
                            }
                            Console.WriteLine("Enter TrashString: ");
                            string trash = Console.ReadLine().ToLower();
                            Console.WriteLine(input.IsPalindrome(trash));
                        }
                        break;

                    case "2": //Problem 3
                        while (true)
                        {
                            Console.WriteLine("Enter minefield length then height: ");
                            int length = Int16.Parse(Console.ReadLine());
                            if (length == -999)
                            {
                                break;
                            }
                            int height = Int16.Parse(Console.ReadLine());

                            Console.WriteLine("Enter 1 for mine and 0 for no mine for listed positions (height,length).");

                            List<int> vertices = new List<int>();
                            for (int i = 1; i < height + 1; i++)
                            {
                                for (int j = 1; j < length + 1; j++)
                                {

                                    Console.WriteLine(String.Format("Enter for position {0},{1}", i, j));
                                    if (Console.ReadLine() == "0")
                                    {
                                        vertices.Add((i * 10) + j);
                                    }
                                }
                            }

                            var edges = Extensions.generateEdges(vertices);
                            var graph = new Graph<int>(vertices, edges);
                            var algorithms = new Algorithms();

                            List<int> startVertex = new List<int>();
                            List<int> endVertex = new List<int>();
                            while (true)
                            {
                                Console.WriteLine("Enter the Starting vertexes(-999 to finish)");
                                string input = Console.ReadLine();
                                if (input == "-999")
                                {
                                    break;
                                }
                                else
                                {
                                    startVertex.Add(Int16.Parse(input));
                                }
                            }

                            while (true)
                            {
                                Console.WriteLine("Enter the Ending vertexes(-999 to finish)");
                                string input = Console.ReadLine();
                                if (input == "-999")
                                {
                                    break;
                                }
                                else
                                {
                                    endVertex.Add(Int16.Parse(input));
                                }
                            }

                            foreach (int start in startVertex)
                            {
                                var shortestPath = algorithms.ShortestPathFunction(graph, start);
                                foreach (var end in endVertex)
                                {
                                    try
                                    {
                                        Console.WriteLine("shortest path to {0,2} from {1,2} : {2}",
                                                end, start, string.Join(", ", shortestPath(end)));
                                    }
                                    catch (KeyNotFoundException e)
                                    {
                                        Console.WriteLine("No possible path for end vertex {0,2}",
                                             end);

                                    }
                                }
                            }

                            Console.WriteLine("Good job! Totoshka and Ally crossed the minefield!");
                        }
                        break;
                }

            }

        }
    }
}