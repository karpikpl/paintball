using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KattisSolution.IO;

namespace KattisSolution
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedPositiveIntReader(stdin);
            // uncomment when you need more advanced reader
            // scanner = new Scanner(stdin);
            // scanner = new LineReader(stdin);
            var writer = new BufferedStdoutWriter(stdout);

            // players
            var n = scanner.NextInt();

            // connections
            var m = scanner.NextInt();

            Dictionary<int, HashSet<int>> connections = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < m; i++)
            {
                var a = scanner.NextInt();
                var b = scanner.NextInt();

                if (!connections.ContainsKey(a))
                {
                    connections.Add(a, new HashSet<int>());
                }
                connections[a].Add(b);

                if (!connections.ContainsKey(b))
                {
                    connections.Add(b, new HashSet<int>());
                }
                connections[b].Add(a);
            }

            // early exit
            if (connections.Count != n)
            {
                writer.Write("Impossible\n");
                writer.Flush();
                return;
            }

            var root = connections.OrderBy(c => c.Value.Count).First().Key;

            var solution = TraverseTree(root, new Tuple<HashSet<int>, HashSet<int>>(new HashSet<int>(), new HashSet<int>()), connections);

            if (solution.Item1.Count == n)
            {
                foreach (var node in solution.Item1)
                {
                    writer.Write(node);
                    writer.Write("\n");
                }
            }
            else
            {
                writer.Write("Impossible\n");
            }

            writer.Flush();
        }

        private static Tuple<HashSet<int>, HashSet<int>> TraverseTree(int node, Tuple<HashSet<int>, HashSet<int>> shotsFired, Dictionary<int, HashSet<int>> connections)
        {
            // Tuple.Item1 -> players that used their one bullet
            // Tuple.Item2 -> players that are dead

            if (shotsFired.Item1.Count == connections.Count)
            {
                return shotsFired;
            }

            if (shotsFired.Item1.Contains(node))
            {
                // it means player doesn't have any bullets
                return shotsFired;
            }

            // find possible target in the ones still alive
            foreach (var possibleTarget in connections[node].Where(b => !shotsFired.Item2.Contains(b)))
            {
                // assume the possibility that node shoots "player"
                var copy = new Tuple<HashSet<int>, HashSet<int>>(new HashSet<int>(shotsFired.Item1), new HashSet<int>(shotsFired.Item2));

                Debug.Assert(!copy.Item1.Contains(node));
                Debug.Assert(!copy.Item2.Contains(possibleTarget));

                // player "node" is shooting player "possible target"
                copy.Item1.Add(node);
                copy.Item2.Add(possibleTarget);

                var result = TraverseTree(possibleTarget, copy, connections);

                if (result.Item1.Count == connections.Count)
                    return result;
            }

            return shotsFired;
        }
    }
}