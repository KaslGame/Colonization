using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResourceAmountText : MonoBehaviour
{
    [SerializeField] private WareHouse _wareHouse;
    [SerializeField] private string _description;

    private TMP_Text _amountText;

    private void OnEnable()
    {
        _wareHouse.ResourceAmountChanged += OnAmountChange;
    }

    private void OnDisable()
    {
        _wareHouse.ResourceAmountChanged -= OnAmountChange;
    }

    private void Awake()
    {
        _amountText = GetComponent<TMP_Text>();
    }

    private void OnAmountChange(int amount)
    {
        _amountText.text = _description + amount.ToString();
    }
}
