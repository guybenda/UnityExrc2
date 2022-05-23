using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public Text healthText;
    public Text enemiesText;
    public PlayerScript player;

    void Update()
    {
        if (healthText == null || enemiesText == null)
        {
            Debug.Log("HUD fields are missing!");
            return;
        }

        if (player == null)
        {
            Debug.Log("Player is missing!");
            return;
        }

        healthText.text = $"HP: {player.health} / {player.maxHealth}";
        enemiesText.text = $"ENEMIES KILLED: {player.enemiesKilled} / {player.enemyKillGoal}";
    }
}
