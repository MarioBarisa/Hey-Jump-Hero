using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int _coins;

    private void Start() => UpdateUI();

    public void AddCoin()
    {
        _coins++;
        UpdateUI();
    }

    private void UpdateUI() => coinText.text = $"Coins: {_coins}";
}