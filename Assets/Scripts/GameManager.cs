using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject[] enemiePrefabs;
    [SerializeField] private Transform spawn;
    [SerializeField] private int enemiesCount;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform healthPanel;
    [SerializeField] private Text countdown;
    [SerializeField] private AudioSource[] horns;
    [SerializeField] private Text points;
    [SerializeField] private GameObject endGamePanel;
    private Transform[] spawnPoints;
    private Transform[] enemies;
    private int pointsCount;

    private void Start()
    {
        enemiesCount += DataHolder.difficulty;
        enemies = new Transform[enemiesCount];
        spawnPoints = spawn.GetComponentsInChildren<Transform>();
        SpawnEnemies();
    }

    public void Defeat(Transform target)
    {
        if (target == player.transform)
        {
            foreach (Transform enemy in enemies)
            {
                if (enemy != null) enemy.GetComponent<TransportController>().target = null;
            }
            StartCoroutine(EndGame("DEFEAT!", Color.red));
        }
        else
        {
            player.DeleteDefetedTarget(target);
            pointsCount++;
            points.text = pointsCount.ToString();
            if (pointsCount == enemiesCount) StartCoroutine(EndGame("VICTORY!", Color.green));
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene("LevelScene");
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            int point = Random.Range(0, spawnPoints.Length);
            if (spawnPoints[point].position.z > 0)
            {
                i--;
                continue;
            }
            GameObject enemy = Instantiate(enemiePrefabs[Random.Range(0, enemiePrefabs.Length)], spawnPoints[point].position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            GameObject healthBar = Instantiate(healthBarPrefab, healthPanel);
            enemy.GetComponent<TransportController>().SetHealthBar(healthBar.GetComponent<HealthController>());
            enemies[i] = enemy.transform;
            spawnPoints[point].position = Vector3.forward;
        }
        StartCoroutine(Countdown());
    }

    private IEnumerator EndGame(string result, Color color)
    {
        yield return new WaitForSeconds(0.5f);

        Text[] info = endGamePanel.GetComponentsInChildren<Text>();
        info[0].text = result;
        info[0].color = color;
        info[1].text = "RESULT: " + pointsCount;
        endGamePanel.SetActive(true);

    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);
        Count(3);
        yield return new WaitForSeconds(1f);
        Count(2);
        yield return new WaitForSeconds(1f);
        Count(1);
        yield return new WaitForSeconds(1f);
        Count(0);
        foreach (var enemy in enemies) enemy.GetComponent<EnemyController>().Attack(new Transform[] { player.transform }, GetComponent<GameManager>());
        player.Attack(enemies, GetComponent<GameManager>());
        yield return new WaitForSeconds(2.5f);
        Count(-1);
    }

    private void Count(int count)
    {
        switch (count)
        {
            case -1:
                countdown.gameObject.SetActive(false);
                break;
            case 0:
                countdown.text = "GO!";
                if (DataHolder.soundsOn) horns[1].Play();
                break;
            case 3:
                countdown.gameObject.SetActive(true);
                if (DataHolder.soundsOn) horns[0].Play();
                break;
            case 1:
            case 2:
                countdown.text = count.ToString();
                if (DataHolder.soundsOn) horns[0].Play();
                break;
        }
    }
}
