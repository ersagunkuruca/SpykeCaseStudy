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
        var results = PermutationGenerator.GeneratePermutations(frequencies);
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log(string.Join(",", results[i].Select(x=>x.ToString())));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
