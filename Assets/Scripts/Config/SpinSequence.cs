using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpinSequence
{
    [SerializeField] int[] sequence;
    public List<SlotCombination> combinationSequence;

    public SpinSequence(int[] sequence)
    {
        this.sequence = sequence;
    }
    
        
    public void CalculateSequence(List<SlotCombination> combinations)
    {
        combinationSequence = sequence.Select(symbol => combinations[symbol]).ToList();
    } 
}