using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private List<SlotAnimationController> columns;
    [SerializeField] private AnimationConfigSet fastAnimation;
    [SerializeField] private AnimationConfigSet mediumAnimation;
    [SerializeField] private AnimationConfigSet slowAnimation;
    [SerializeField] float delayRange;

    [Serializable]
    public class AnimationConfigSet
    {
        public List<SlotAnimationController.AnimationConfig> animations;
    }
    
    public async Task Spin(List<Symbol> symbols)
    {
        AnimationConfigSet animationSet = null;
        if (symbols[0] != symbols[1])
        {
            animationSet = fastAnimation;
        }
        else
        {
            animationSet = Random.Range(0f, 1f) < 0.5f ? slowAnimation : mediumAnimation;
        }

        float delay1 = Random.Range(0f, delayRange);
        float delay2 = delay1 + Random.Range(0f, delayRange);
        float[] delays = new[] { 0f, delay1, delay2 };
        var tasks = new List<Task>();
        for (int i = 0; i < symbols.Count && i < columns.Count; i++)
        {
            tasks.Add(columns[i].StartAnimation(symbols[i], delays[i], animationSet.animations[i]));
        }

        await Task.WhenAll(tasks);
    }

}
