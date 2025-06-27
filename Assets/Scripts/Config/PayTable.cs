using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pay Table", menuName = "Slot Machine/Pay Table")]
public class PayTable : ScriptableObject
{
    public List<SlotCombination> combinations;
}

[Serializable]
public class SlotCombination
{
    public List<Symbol> symbols;
    public int frequency;
    public int reward;
}