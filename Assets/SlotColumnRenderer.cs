using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class SlotColumnRenderer : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private SymbolList symbolList;
    [SerializeField] private float rowHeight;
    public float position;
    
    void Update()
    {
        var indexOffset = Mathf.RoundToInt(position);
        var positionOffset = position - indexOffset;
        int i = 0;
        foreach (var spriteRenderer in spriteRenderers)
        {
            var symbol = symbolList.symbols[mod(indexOffset + i - 1, symbolList.symbols.Count)];
            spriteRenderer.sprite = symbol.sprite;
            spriteRenderer.transform.localPosition = Vector3.up * ((-1f + i - positionOffset) * rowHeight);
            i++;
        }
    }

    int mod(int a, int b)
    {
        return ((a % b) + b) % b;
    }
}
