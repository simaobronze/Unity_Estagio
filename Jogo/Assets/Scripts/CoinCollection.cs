using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int Coin = 0;

    public TextMeshProUGUI coinText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            Coin++;
            coinText.text = "Coins Collected: " + Coin.ToString();
            Debug.Log("Coin collected: " + Coin);
            Destroy(other.gameObject);
        }
    }
}
