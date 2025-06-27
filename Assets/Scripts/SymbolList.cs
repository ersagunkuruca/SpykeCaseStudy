using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Symbol List", menuName="Slot Machine/Symbol List")]
public class SymbolList : ScriptableObject
{
    public List<Symbol> symbols;
}