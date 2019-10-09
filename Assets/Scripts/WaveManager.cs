using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints = default;
    private GameObject player;
    private TankPlayer playerTank;
    private Rigidbody playerRb;

    [SerializeField]
    private GameObject enemyPrefab = default;
    public List<GameObject> aliveEnemies;

    [SerializeField]
    private TextMeshProUGUI countdownText = default;
    [SerializeField]
    private string countdownTextString = "Game starts in:";
    [SerializeField]
    private string waveCountdownTextString = "Next wave starts in:";
    [SerializeField]
    private int countdownTime = 3;
    [SerializeField]
    private int waveCountdownTime = 3;
    private int waveCountdownTimeMax;

    [SerializeField]
    private TextMeshProUGUI waveText = default;

    private bool waveOnGoing;
    private bool startedWaveCountdown;

    private void Awake()
    {
        aliveEnemies = new List<GameObject>();
        player = GameObject.Find("PRE_Tank_Player");
        playerTank = player.GetComponent<TankPlayer>();
        playerRb = player.GetComponent<Rigidbody>();
        waveCountdownTimeMax = waveCountdownTime;
    }

    private void Start()
    {
        currentWave = Waves.None;
        countdownText.enabled = true;
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(StartCountdownTimer(countdownTime));
    }

    private IEnumerator StartCountdownTimer(float countdownTime)
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
                break;
        }
    }

    private void Wave1()
    {
        if (!startedWaveCountdown)
        {
            if (!waveOnGoing)
            {
                waveOnGoing = true;
                SpawnEnemies(3);
            }

            waveText.text = $"Wave 1: enemies alive {aliveEnemies.Count}";

            if (aliveEnemies.Count <= 0)
            {
                waveOnGoing = false;
                HealPlayer();
                StartCoroutine(WaveCountdown(Waves.Wave2));
            }
        }
    }

    private void Wave2()
    {
        if (!startedWaveCountdown)
        {
            if (!waveOnGoing)
            {
                waveOnGoing = true;
                SpawnEnemies(5);
            }

            waveText.text = $"Wave 2: enemies alive {aliveEnemies.Count}";

            if (aliveEnemies.Count <= 0)
            {
                waveOnGoing = false;
                startedWaveCountdown = true;
                HealPlayer();
                StartCoroutine(WaveCountdown(Waves.Wave3));
            }
        }
    }

    private void Wave3()
    {
        if (!startedWaveCountdown)
        {
            if (!waveOnGoing)
            {
                waveOnGoing = true;
                SpawnEnemies(7);
            }

            waveText.text = $"Wave 3: enemies alive {aliveEnemies.Count}";

            if (aliveEnemies.Count <= 0)
            {
                waveOnGoing = false;
                startedWaveCountdown = true;
                HealPlayer();
                StartCoroutine(WaveCountdown(Waves.Boss));
            }
        }
    }

    private void Boss()
    {
        waveText.text = "Boss";
    }

    private IEnumerator WaveCountdown(Waves changeWave)
    {
        startedWaveCountdown = true;
        countdownText.enabled = true;

        while (waveCountdownTime > 0)
        {
            countdownText.text = $"{waveCountdownTextString} {waveCountdownTime}";
            yield return new WaitForSeconds(1);
            waveCountdownTime--;
        }

        countdownText.enabled = false;
        waveCountdownTime = waveCountdownTimeMax;
        startedWaveCountdown = false;
        currentWave = changeWave;
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject spawn = Instantiate(enemyPrefab);
            aliveEnemies.Add(spawn);
            spawn.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position + new Vector3(0, 0.5f, 0);
        }
    }

    private void HealPlayer()
    {
        if (playerTank.HitPoints <= playerTank.MaxHitpoints / 2)
        {
            playerTank.HitPoints += 100;
        }
    }
}