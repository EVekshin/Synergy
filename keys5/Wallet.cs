// Wallet.cs
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coins;

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = $"Coins: {coins}";
    }
}

// Coin.cs
public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Wallet>().AddCoins(value);
            Destroy(gameObject);
        }
    }
}