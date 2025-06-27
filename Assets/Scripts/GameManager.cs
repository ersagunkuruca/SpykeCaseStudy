using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PayTable payTable;
    [SerializeField] private Button spinButton;
    [SerializeField] private SlotMachineController slotMachineController;
    
    private int[] _frequencies;
    private SpinSequence _sequence;
    private int _spinIndex = 0;

    void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
    }

    void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
    }

    private void OnSpinClicked()
    {
        var symbols = _sequence.combinationSequence[_spinIndex].symbols;
        Debug.Log("Spinning " + string.Join(",", symbols.Select(a=>a.name)) );
        slotMachineController.Spin(symbols);
        _spinIndex++;
        _spinIndex %= _sequence.combinationSequence.Count;
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