using UnityEngine;
using TMPro; // or UnityEngine.UI if using Text

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    private Player player;

    void Start()
    {
        player = Player.Instance;
        UpdateText();
    }

    public void UpdateText()
    {
        if (player != null)
            healthText.text = $"HP:{player.CurrentHealth}/{player.MaxHealth}";
    }
}
