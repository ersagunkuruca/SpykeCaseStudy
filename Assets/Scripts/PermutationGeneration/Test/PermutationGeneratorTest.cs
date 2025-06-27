using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class PermutationGeneratorTest 
{

    [Test]
    public void TestWithGivenArray()
    {
        var frequencies = new int[] { 13, 13, 13, 13, 13, 9, 8, 7, 6, 5 };
        TestWithArrayOfFrequencies(frequencies);
    }
    void TestWithArrayOfFrequencies(int[] frequencies)
    {
        var results = PermutationGenerator.GeneratePermutationsNew(frequencies);
        Assert.True(results.Length == 1, "We should get a single element array");
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log(string.Join(",", results[i].Select(x=>x.ToString())));
        }

        int total = 0;
        for (int i = 0; i < frequencies.Length; i++)
        {
            total += frequencies[i];
        }
        for (var i = 0; i < frequencies.Length; i++)
        {
            
            var frequency = frequencies[i];
            for (var j = 0; j < frequency; j++)
            {
                var start = total * j / frequency;
                var end = total * (j + 1) / frequency;
                var count = 0;
                for (int k = start; k < end; k++)
                {
                    if (results[0][k] == i)
                    {
                        count++;
                    }
                }

                Assert.True(count == 1, $"Symbol {i} appears {count} times in block {j}");
            }
        }
    }

}
