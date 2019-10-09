using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints = default;
    private GameObject player;
    private Rigidbody playerRb;

    [SerializeField]
    private GameObject enemyPrefab = default;
    public List<GameObject> aliveEnemies;

    [SerializeField]
    private TextMeshProUGUI countdownText = default;
    [SerializeField]
    private string countdownTextString = "Game starts in:";
    [SerializeField]
    private int countdownTime = 3;

    [SerializeField]
    private TextMeshProUGUI waveText = default;

    private bool waveOnGoing;

    private void Awake()
    {
        aliveEnemies = new List<GameObject>();
        player = GameObject.Find("PRE_Tank_Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentWave = Waves.None;
        countdownText.enabled = true;
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(CountdownTimer(countdownTime));
    }

    private IEnumerator CountdownTimer(float countdownTime)
    {
        while (countdownTime > 0)
        {
            countdownText.text = $"{countdownTextString} {countdownTime}";
            yield return new WaitForSeconds(1);
            countdownTime--;
        }

        countdownText.enabled = false;
        playerRb.constraints = RigidbodyConstraints.None;
        currentWave = Waves.Wave1;
    }

    private enum Waves
    {
        None,
        Wave1,
        Wave2,
        Wave3,
        Boss
    }

    private Waves currentWave;

    private void Update()
    {
        switch (currentWave)
        {
            case Waves.Wave1:
                Wave1();
                break;
            case Waves.Wave2:
                Wave2();
                break;
            case Waves.Wave3:
                Wave3();
                break;
            case Waves.Boss:
                Boss();
                break;
            default:
                #if UNITY_EDITOR
                Debug.Log("currentWave shouldn't be at default");
                #endif
                break;
        }
    }

    private void Wave1()
    {
        SpawnEnemies(4);

        waveText.text = $"Wave 1: alive enemies {aliveEnemies.Count}";

        if (aliveEnemies.Count <= 0)
        {
            waveOnGoing = false;
            currentWave = Waves.Wave2;
        }
    }

    private void Wave2()
    {
        SpawnEnemies(6);

        waveText.text = $"Wave 2: alive enemies {aliveEnemies.Count}";

        if (aliveEnemies.Count <= 0)
        {
            waveOnGoing = false;
            currentWave = Waves.Wave3;
        }
    }

    private void Wave3()
    {
        SpawnEnemies(8);

        waveText.text = $"Wave 3: alive enemies {aliveEnemies.Count}";

        if (aliveEnemies.Count <= 0)
        {
            waveOnGoing = false;
            currentWave = Waves.Boss;
        }
    }

    private void Boss()
    {
        waveText.text = "Boss";
    }

    private void SpawnEnemies(int amount)
    {
        if (!waveOnGoing)
        {
            waveOnGoing = true;
            for (int i = 0; i < amount; i++)
            {
                GameObject spawn = Instantiate(enemyPrefab);
                aliveEnemies.Add(spawn);
                spawn.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position + new Vector3(0, 0.5f, 0);
            }
        }
    }
}
