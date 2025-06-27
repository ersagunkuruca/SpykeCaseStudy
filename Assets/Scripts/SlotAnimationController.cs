using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SlotAnimationController : MonoBehaviour
{
    [SerializeField] private SlotColumnRenderer columnRenderer;
    [SerializeField] private SymbolList symbolList;
    [SerializeField] private float blurSpeed;

    [Serializable]
    public class AnimationConfig
    {
        public float duration;
        public float endDuration;
        public AnimationCurve startCurve;
        public AnimationCurve stopCurve;
        public float stopOffset = 1f;
        public int turnCount = 5;
    }
    // These are just serialized for viewing purposes, ideally these values should be just shown via Odin or custom editor, not serialized
    [SerializeField] private float startDelay;
    [SerializeField] private AnimationConfig animationConfig;
    [SerializeField] private int targetSymbolIndex;
    public async Task StartAnimation(Symbol symbol, float startDelay, AnimationConfig config)
    {
        animationConfig = config;
        this.startDelay = startDelay;
        targetSymbolIndex = symbolList.symbols.IndexOf(symbol);
        await AnimateCoroutine();
    }

    private async Task AnimateCoroutine()
    {
        float startTime = Time.time;
        var initialPosition = Mathf.Repeat(columnRenderer.position, symbolList.symbols.Count);
        var lastPosition = initialPosition;

        var delayEndTime = startDelay;
        var fastSpinEndTime = animationConfig.duration - animationConfig.endDuration;
        var fastSpinEndPosition = (animationConfig.turnCount * symbolList.symbols.Count) + targetSymbolIndex -
                                  animationConfig.stopOffset;
        var endAnimationEndTime = animationConfig.duration;
        var animationEndPosition = (animationConfig.turnCount * symbolList.symbols.Count) + targetSymbolIndex;
        while (true)
        {
            float newPosition = lastPosition;
            float time = Time.time - startTime;
            if (time < delayEndTime)
            {
                newPosition = initialPosition;
                 
            }
            else if (time < fastSpinEndTime)
            {
                newPosition = RemapWithCurve(
                    animationConfig.startCurve,
                    delayEndTime,
                    fastSpinEndTime,
                    initialPosition,
                    fastSpinEndPosition,
                    time
                    );
            }
            else //if (time < endAnimationEndTime)
            {
                newPosition = RemapWithCurve(
                    animationConfig.stopCurve,
                    fastSpinEndTime,
                    endAnimationEndTime,
                    fastSpinEndPosition,
                    animationEndPosition,
                    time
                );;
            }

            columnRenderer.position = newPosition;
            float speed = (newPosition - lastPosition) / Time.deltaTime;
            columnRenderer.blurred = speed > blurSpeed && time < endAnimationEndTime;
            columnRenderer.UpdateRenderer();
            
            lastPosition = columnRenderer.position;

            if (time >= endAnimationEndTime)
            {
                break;
            }
            await UniTask.WaitForEndOfFrame(this);
        }
    }

    private float RemapWithCurve(AnimationCurve curve, float a, float b, float c, float d, float t)
    {
        return Mathf.Lerp(c, d, curve.Evaluate(Mathf.InverseLerp(a, b, t)));
    }
    
}