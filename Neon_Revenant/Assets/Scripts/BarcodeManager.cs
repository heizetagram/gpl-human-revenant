using UnityEngine;
using TMPro;

public class BarcodeManager : MonoBehaviour
{
    public static BarcodeManager Instance;

    public int totalBarcodes = 0;
    public TextMeshProUGUI amountText;
    public PlayerController playerController;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddBarcode(int amount)
    {
        totalBarcodes += amount;
        UpdateText();

        if (playerController != null)
        {
            playerController.AssignGuns(totalBarcodes);
        }
    }

    void UpdateText()
    {
        amountText.text = "" + totalBarcodes;
    }
}