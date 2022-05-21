using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public Text healthText;
    public Text enemiesText;
    //public Image crosshairImage;
    // TODO victory screen
    public PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        //this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthText == null || enemiesText == null)// || crosshairImage == null)
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
