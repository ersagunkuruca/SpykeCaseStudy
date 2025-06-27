using System.Collections.Generic;

public static class PermutationGenerator
{
    
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
            /*for (int j = 0; j < symbolFrequencies.Length; j++)
            {
                blockMap[i][j] = i * symbolFrequencies[j] / total;
            }*/
        }

        for (int i = 0; i < symbolFrequencies.Length; i++)
        {
            var frequency = symbolFrequencies[i];
            for (int j = 0; j < frequency; j++)
            {
                var start = total * j / frequency;
                var end = total * (j + 1) / frequency;
                var count = 0;
                for (int k = start; k < end; k++)
                {
                    blockMap[k][i] = j;
                }
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