using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int _coins;

    //private void Start() => UpdateUI(); OVO TREBA UNCOMMENT AKO IKADA BUDEMO DODALI UI ELEMENT ZA PRIKAZ BROJA NOVCICA

    public void AddCoin()
    {
        _coins++;
       // UpdateUI();
    }

   // private void UpdateUI() => coinText.text = $"Coins: {_coins}";
}