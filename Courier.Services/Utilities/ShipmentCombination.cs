using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier.Services.Utilities
{
    public static class ShipmentCombination
    {
        public static List<List<Package>> GetAllCombinationsUpToWeight(List<Package> packages, double maxWeight = 200)
        {
            var result = new List<List<Package>>();
            var currentCombination = new List<Package>();

            GenerateCombinations(packages, 0, currentCombination, result, maxWeight);

            return result;
        }

        private static void GenerateCombinations(
            List<Package> packages,
            int startIndex,
            List<Package> currentCombination,
            List<List<Package>> result,
            double maxWeight)
        {
            // Calculate current weight
            double currentWeight = currentCombination.Sum(p => p.Weight);

            // Add current combination if it has packages and weight is within limit
            if (currentCombination.Count > 0 && currentWeight <= maxWeight)
            {
                result.Add(new List<Package>(currentCombination));
            }

            // Try adding each remaining package
            for (int i = startIndex; i < packages.Count; i++)
            {
                // Only add if it doesn't exceed max weight
                if (currentWeight + packages[i].Weight <= maxWeight)
                {
                    currentCombination.Add(packages[i]);
                    GenerateCombinations(packages, i + 1, currentCombination, result, maxWeight);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }
    }
}
