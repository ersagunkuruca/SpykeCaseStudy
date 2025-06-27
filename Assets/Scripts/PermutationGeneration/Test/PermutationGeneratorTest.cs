using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PermutationGeneratorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var frequencies = new int[] { 13, 13, 13, 13, 13, 9, 8, 7, 6, 5 };
        var results = PermutationGenerator.GeneratePermutationsNew(frequencies);
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

                Debug.Assert(count == 1,
                    $"Symbol {i} appears {count} times in block {j}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
