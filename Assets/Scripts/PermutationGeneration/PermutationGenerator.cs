using System.Collections;
using System.Collections.Generic;

public static class PermutationGenerator
{
    public static int[][] GeneratePermutations(int[] symbolFrequencies)
    {
        var total = 0;
        for (var i = 0; i < symbolFrequencies.Length; i++)
        {
            var number = symbolFrequencies[i];
            total += number;
        }
        

        var current = new int[total];
        for (int i = 0; i < total; i++)
        {
            current[i] = -1;
        }
        var results = new List<int[]>();

        var result = BacktrackBlocks(current, total, 0,0, symbolFrequencies, results, true);
        return results.ToArray();
    }


    private static bool BacktrackBlocks(int[] currentPermutation, int total, int symbolIndex, int blockIndex,
        int[] symbolFrequencies, List<int[]> results, bool findOne)
    {
        var frequency = symbolFrequencies[symbolIndex];
        var blockSize = (total + frequency - 1) / frequency;
        var blockStart = blockIndex * blockSize;
        var blockEnd = (blockIndex + 1) * blockSize;
        if (blockEnd >= total) blockEnd = total;
        var actualBlockSize = blockEnd - blockStart;
        var foundOne = false;
        for (int k = 0; k < actualBlockSize; k++)
        {
            var currentIndex = blockStart + k;
            if (currentPermutation[currentIndex] != -1)
            {
                continue;
            }

            currentPermutation[currentIndex] = symbolIndex;
            if (blockIndex + 1 >= frequency)
            {
                if (symbolIndex + 1 >= symbolFrequencies.Length)
                {
                    var newResult = new int[currentPermutation.Length];
                    for (var i = 0; i < currentPermutation.Length; i++)
                    {
                        newResult[i] = currentPermutation[i];
                    }

                    results.Add(newResult);
                    return true;
                }
                else
                {
                    if (BacktrackBlocks(
                            currentPermutation,
                            total,
                            symbolIndex + 1,
                            0,
                            symbolFrequencies,
                            results,
                            findOne
                        ))
                    {
                        foundOne = true;
                    }
                }
            }
            else
            {
                if (BacktrackBlocks(
                        currentPermutation,
                        total, symbolIndex,
                        blockIndex + 1,
                        symbolFrequencies,
                        results,
                        findOne
                    ))
                {
                    foundOne = true;
                }
            }

            if (findOne && foundOne)
            {
                // early return if we want to find only one example
                return true;
            }
            currentPermutation[currentIndex] = -1;
        }

        return foundOne;
    }
}