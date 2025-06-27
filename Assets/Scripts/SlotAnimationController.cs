using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotAnimationController : MonoBehaviour
{
    [SerializeField] private SlotColumnRenderer columnRenderer;
    public int targetSymbolIndex;
    [SerializeField] private AnimationConfig animationConfig;
    [SerializeField] private SymbolList symbolList;
    [SerializeField] private float blurSpeed;

    [Serializable]
    public class AnimationConfig
    {
        public float duration;
        public float startDelay;
        public float endDuration;
        public AnimationCurve startCurve;
        public AnimationCurve stopCurve;
        public float stopOffset = 1f;
        public int turnCount = 5;
        public float startDuration => duration - endDuration - startDelay;
    }

    void Start()
    {
        StartAnimation();
    }

    public void StartAnimation()
    {
        StartCoroutine(AnimateCoroutine());
    }

    private IEnumerator AnimateCoroutine()
    {
        float startTime = Time.time;
        var initialPosition = Mathf.Repeat(columnRenderer.position, symbolList.symbols.Count);
        var lastPosition = initialPosition;

        var delayEndTime = animationConfig.startDelay;
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
            columnRenderer.blurred = speed > blurSpeed;

            lastPosition = columnRenderer.position;

            if (time >= endAnimationEndTime)
            {
                break;
            }
            yield return null;
        }
    }

    private float RemapWithCurve(AnimationCurve curve, float a, float b, float c, float d, float t)
    {
        return Mathf.Lerp(c, d, curve.Evaluate(Mathf.InverseLerp(a, b, t)));
    }
    
}