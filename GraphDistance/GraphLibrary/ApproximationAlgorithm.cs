﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public static class ApproximationAlgorithm
    {
        static Random Random = new Random();

        /// <summary>
        /// Finding potential maximum clique of a graph
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="k">number of samples</param>
        /// <returns>List of vertices of clique</returns>
        public static List<int> FindMaximumClique(Graph graph, int k)
        {
            List<int> maximumClique = new List<int>();

            int n = graph.VerticesCount();

            for(int sample = 0; sample < k; sample++)
            {
                // Current Clique
                List<int> CC = new List<int>();
                // Candidates and degree
                Dictionary<int, int> degPA = new Dictionary<int, int>();

                int v = Random.Next() % n;
                CC.Add(v);

                // Initialize PA set
                for (int i = 0; i < n; i++)
                {
                    if (graph.Edges[i, v] > 0)
                    {
                        degPA.Add(i, 0);
                    }
                }

                CalculateDegree(degPA, graph);

                // Begin algorithm
                while (degPA.Any())
                {
                    int bestCandidate = GetBestCandidate(degPA);

                    // Add new vertex to clique
                    CC.Add(bestCandidate);

                    degPA.Remove(bestCandidate);

                    var neighbors = GetNeighbors(bestCandidate, graph);

                    degPA = GetUpdatedPA(degPA, neighbors, graph);
                }

                if(CC.Count > maximumClique.Count)
                {
                    maximumClique = new List<int>(CC);
                }
            }

            return maximumClique;
        }

        private static Dictionary<int, int> GetUpdatedPA(Dictionary<int,int> degPA, List<int> neighbors, Graph graph)
        {
            Dictionary<int, int> newDegPA = new Dictionary<int, int>();

            foreach(var neighbor in neighbors)
            {
                if (degPA.ContainsKey(neighbor))
                {
                    newDegPA.Add(neighbor, 0);
                }
            }

            CalculateDegree(newDegPA, graph);

            return newDegPA;
        }

        private static void CalculateDegree(Dictionary<int, int> degPA, Graph compatibilityGraph)
        {
            foreach(var vertex in degPA)
            {
                foreach(var neighbor in degPA)
                {
                    if(compatibilityGraph.Edges[neighbor.Key, vertex.Key] > 0)
                    {
                        degPA[vertex.Key]++;
                    }
                }
            }
        }

        private static List<int> GetNeighbors(int bestCandidate, Graph graph)
        {
            List<int> neighbors = new List<int>();

            for(int i = 0; i < graph.VerticesCount(); i++)
            {
                if(graph.Edges[i, bestCandidate] > 0)
                {
                    neighbors.Add(i);
                }
            }

            return neighbors;
        }

        private static int GetBestCandidate(Dictionary<int,int> candidates)
        {
            int bestCandidate = -1;
            int maxDeg = -1;
            
            foreach (var candidate in candidates)
            {
                if(candidate.Value > maxDeg)
                {
                    maxDeg = candidate.Value;
                    bestCandidate = candidate.Key;
                } 
            }

            return bestCandidate;
        }
    }
}
