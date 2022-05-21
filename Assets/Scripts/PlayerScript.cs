using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int enemiesKilled = 0;
    public int enemyKillGoal = 5;
    public GameObject flash;
    public GameObject pistol;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health < 0) health = 0;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    public void Shoot()
    {
        pistol.GetComponent<AudioSource>().Play();
        StartCoroutine(FlashRoutine());
    }


    IEnumerator FlashRoutine()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        flash.SetActive(false);
        flash.transform.Rotate(Vector3.right * Random.Range(-180f, 180f));
    }
}
