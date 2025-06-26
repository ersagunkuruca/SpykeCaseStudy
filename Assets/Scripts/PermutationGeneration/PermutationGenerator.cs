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

        var result = BacktrackBlocks(current, total, 0, 0, symbolFrequencies, results, true);
        return results.ToArray();
    }


    private static bool BacktrackBlocks(int[] currentPermutation, int total, int symbolIndex, int blockIndex,
        int[] symbolFrequencies, List<int[]> results, bool findOne)
    {
        var frequency = symbolFrequencies[symbolIndex];
        var blockSize = (total + frequency - 1) / frequency;
        var blockStart = (total * blockIndex) / frequency;
        var blockEnd = (total * (blockIndex + 1)) / frequency;
        if (blockEnd >= total) blockEnd = total;
        var actualBlockSize = blockEnd - blockStart;
        var foundOne = false;
        for (int k = blockStart; k < blockEnd; k++)
        {
            if (currentPermutation[k] != -1)
            {
                continue;
            }

            currentPermutation[k] = symbolIndex;
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

            currentPermutation[k] = -1;
        }

        return foundOne;
    }


    public static int[][] GeneratePermutationsNew(int[] symbolFrequencies)
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

        var blocksFilled = new int[symbolFrequencies.Length];
        for (int i = 0; i < blocksFilled.Length; i++)
        {
            blocksFilled[i] = 0;
        }

        var blockMap = new int[total][];
        for (int i = 0; i < total; i++)
        {
            blockMap[i] = new int[symbolFrequencies.Length];
            for (int j = 0; j < symbolFrequencies.Length; j++)
            {
                blockMap[i][j] = i * symbolFrequencies[j] / total;
            }
        }

        var results = new List<int[]>();

        var result = BacktrackSlots(current, blocksFilled, blockMap, total, 0, symbolFrequencies, results, true);
        return results.ToArray();
    }


    private static bool BacktrackSlots(int[] currentPermutation, int[] blocksFilled, int[][] blockMap, int total,
        int index, int[] symbolFrequencies, List<int[]> results, bool findOne)
    {
        if (index == total)
        {
            var newResult = new int[total];
            for (int i = 0; i < total; i++)
            {
                newResult[i] = currentPermutation[i];
            }
            results.Add(newResult);
            return true;
        }
        var currentBlocks = blockMap[index];
        for (int i = 0; i < symbolFrequencies.Length; i++)
        {
            if (currentBlocks[i] > blocksFilled[i])
            {
                return false;
            }
        }

        var foundOne = false;
        for (int j = 0; j < symbolFrequencies.Length; j++)
        {
            if (blocksFilled[j] == currentBlocks[j])
            {
                currentPermutation[index] = j;
                blocksFilled[j]++;

                if (BacktrackSlots(currentPermutation, blocksFilled, blockMap, total, index + 1, symbolFrequencies,
                        results, findOne))
                {
                    foundOne = true;
                    if (findOne)
                    {
                        return true;
                    }
                }
                
                currentPermutation[index] = -1;
                blocksFilled[j]--;
            }
        }

        return foundOne;
    }
}