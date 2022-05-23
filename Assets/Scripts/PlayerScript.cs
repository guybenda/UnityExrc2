using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 200;
    public int enemiesKilled = 0;
    public int enemyKillGoal = 5;
    public GameObject flash;
    public GameObject pistol;
    public int damage = 34;

    AudioSource pistolAudioSource;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        pistolAudioSource = pistol.GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();
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
        health = Mathf.Clamp(health - damage, 0, health);

        if (health <= 0)
        {
            SceneManager.LoadScene("DeadScene", LoadSceneMode.Single);

        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled >= enemyKillGoal)
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }

    public void Shoot()
    {
        StartCoroutine(FlashRoutine());

        const int mask = ~(1 << 8);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, Mathf.Infinity, mask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 3);
                Debug.Log("HIT " + hit.collider.gameObject.name);

                if (hit.collider.GetComponent<EnemyScript>().Damage(damage))
                {
                    EnemyKilled();
                }
            }
        }
    }


    IEnumerator FlashRoutine()
    {
        flash.SetActive(true);
        pistolAudioSource.Play();
        yield return new WaitForSeconds(0.05f);
        flash.SetActive(false);
        flash.transform.Rotate(Vector3.right * Random.Range(-180f, 180f));
    }
}
