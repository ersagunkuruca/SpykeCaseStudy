using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PayTable payTable;
    [SerializeField] private Button spinButton;
    [SerializeField] private SlotMachineController slotMachineController;
    [SerializeField] private ParticleSystem particleSystem;
    
    private int[] _frequencies;
    private SpinSequence _sequence;
    private int _spinIndex = 0;
    private bool _spinning;

    void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
    }

    void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
    }

    private async void OnSpinClicked()
    {
        if (_spinning) return;
        _spinning = true;
        var spin = _sequence.combinationSequence[_spinIndex];
        var symbols = spin.symbols;
        var reward = spin.reward;
        Debug.Log($"Spinning {string.Join(",", symbols.Select(a => a.name))} reward: {reward}");
        await slotMachineController.Spin(symbols);
        if (reward > 0)
        {
            var particleSystemEmission = particleSystem.emission;
            particleSystemEmission.rateOverTime = (float)reward;
            particleSystem.Play();
            await UniTask.WaitForSeconds(particleSystem.main.duration);
        }
        _spinIndex++;
        _spinIndex %= _sequence.combinationSequence.Count;
        _spinning = false;
    }

    void Start()
    {
        SetupSequence();
    }

    void SetupSequence()
    {
        if (PlayerPrefs.HasKey("spinSequence"))
        {
            _sequence = JsonUtility.FromJson<SpinSequence>(PlayerPrefs.GetString("spinSequence"));
            _spinIndex = PlayerPrefs.GetInt("spinIndex", 0);
        }
        else
        {
            _frequencies ??= payTable.combinations.Select(combination => combination.frequency).ToArray();
            _sequence = new(PermutationGenerator.GeneratePermutationsNew(_frequencies)[0]);
            PlayerPrefs.SetString("spinSequence", JsonUtility.ToJson(_sequence));
            PlayerPrefs.SetInt("spinIndex", 0);
        }

        _sequence.CalculateSequence(payTable.combinations);
    }
}