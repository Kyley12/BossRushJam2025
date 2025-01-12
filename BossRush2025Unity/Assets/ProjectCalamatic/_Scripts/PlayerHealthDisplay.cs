using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerHPText;
    public PlayerStatSO playerStat;

    private void Update()
    {
        playerHPText.text = $"{playerStat.cursorHealth}/{playerStat.cursorMaxHealth}";
    }
}
