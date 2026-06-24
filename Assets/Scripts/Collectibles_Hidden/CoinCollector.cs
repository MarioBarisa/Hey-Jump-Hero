using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int _coins;

    private void Start() => UpdateUI(); //OVO TREBA COMMENT AKO IKADA BUDEMO ŽLILI MAKNUTI UI ELEMENT ZA PRIKAZ BROJA NOVCICA

    //TROŠENJE COINSA SECTION
    public bool HasEnoughCoins(int amount) => _coins >= amount; //provjer ako ima dovoljno coins 
    public bool SpendCoins(int amount)
    {
        if (HasEnoughCoins(amount))
        {
            _coins -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddCoin()
    {
        _coins++;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        UpdateUI();
     }

    private void UpdateUI()
    {
        coinText.text = $"Coins: {_coins}";
        HealthSystem hs = GetComponent<HealthSystem>();
        if(hs != null) hs.refreshUpgradeLabel();
    } 
}