using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public List<GameObject> AliveEnemies { get; set; }

    [SerializeField]
    private Transform[] spawnPoints = default;
    [SerializeField]
    private Transform bossSpawnPoint = default;
    [SerializeField]
    private GameObject enemyPrefab = default;
    [SerializeField]
    private GameObject bossEnemyPrefab = default;
    [SerializeField]
    private TextMeshProUGUI countdownText = default;
    [SerializeField]
    private TextMeshProUGUI waveText = default;
    private GameObject player;
    private Rigidbody playerRb;
    private TankPlayer playerTank;
    private GameObject boss;

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
    private int healAmount = 150;
    [SerializeField]
    private int initialWaveSize = 4;

    private bool waveOnGoing;
    private bool startedWaveCountdown;
    private bool bossSpawned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        AliveEnemies = new List<GameObject>();
        player = GameObject.Find("PRE_Tank_Player");
        playerTank = player.GetComponent<TankPlayer>();
        playerRb = player.GetComponent<Rigidbody>();
        waveCountdownTimeMax = waveCountdownTime;
    }

    private void Start()
    {
        currentWave = Waves.None;
        CountdownPrepare(enableText: true, RigidbodyConstraints.FreezeAll);
        StartCoroutine(StartCountdownTimer(countdownTime));
    }

    private IEnumerator StartCountdownTimer(float countdownTime)
    {
        // Start the countdown timer.
        while (countdownTime > 0)
        {
            countdownText.text = $"{countdownTextString} {countdownTime}";
            yield return new WaitForSeconds(1);
            countdownTime--;
        }

        CountdownPrepare(enableText: false, RigidbodyConstraints.None);
        currentWave = Waves.Wave1;
    }

    private void CountdownPrepare(bool enableText, RigidbodyConstraints constraints)
    {
        // Enable or disable the countdown text and constraints.
        countdownText.enabled = enableText;
        playerRb.constraints = constraints;
    }

    private enum Waves
    {
        None,
        Wave1,
        Wave2,
        Wave3,
        Wave4,
        Boss
    }

    private Waves currentWave;

    private void Update()
    {
        // All the wave states.
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
            case Waves.Wave4:
                Wave4();
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
            WaveStageEnemies(0);

            WaveText(1, false);

            WaveStateNext(Waves.Wave2);
        }
    }

    private void Wave2()
    {
        if (!startedWaveCountdown)
        {
            WaveStageEnemies(2);

            WaveText(2, false);

            WaveStateNext(Waves.Wave3);
        }
    }

    private void Wave3()
    {
        if (!startedWaveCountdown)
        {
            WaveStageEnemies(3);

            WaveText(3, false);

            WaveStateNext(Waves.Wave4);
        }
    }

    private void Wave4()
    {
        if (!startedWaveCountdown)
        {
            WaveStageEnemies(4);

            WaveText(4, false);

            WaveStateNext(Waves.Boss);
        }
    }

    private void Boss()
    {
        WaveText(0, isBoss: true);

        if (!bossSpawned)
        {
            boss = Instantiate(bossEnemyPrefab);
            boss.transform.position = bossSpawnPoint.transform.position + new Vector3(0, 1.5f, 0);
            bossSpawned = true;
        }
        else if (boss == null)
        {
            countdownText.enabled = true;
            countdownText.text = "Victory";
        }
    }

    private void WaveStageEnemies(int waveSize)
    {
        if (!waveOnGoing)
        {
            waveOnGoing = true;
            SpawnEnemies(initialWaveSize + waveSize);
        }
    }

    private void WaveStateNext(Waves nextWave)
    {
        if (AliveEnemies.Count <= 0)
        {
            waveOnGoing = false;
            HealPlayer();
            StartCoroutine(WaveCountdown(nextWave));
        }
    }

    private IEnumerator WaveCountdown(Waves changeWave)
    {
        // Countdown timer for use in between waves.
        startedWaveCountdown = true;
        if (countdownText != null)
        {
            countdownText.enabled = true;
        }
        // Pause coroutine until timer has completed.
        while (waveCountdownTime > 0)
        {
            countdownText.text = $"{waveCountdownTextString} {waveCountdownTime}";
            yield return new WaitForSeconds(1);
            waveCountdownTime--;
        }

        if (countdownText != null)
        {
            countdownText.enabled = false;
        }
        waveCountdownTime = waveCountdownTimeMax;
        startedWaveCountdown = false;
        // Change the wave.
        currentWave = changeWave;
    }

    private void SpawnEnemies(int amount)
    {
        // Spawn enemies at random spawnpoints.
        for (int i = 0; i < amount; i++)
        {
            GameObject spawn = Instantiate(enemyPrefab);
            AliveEnemies.Add(spawn);
            int random = Random.Range(0, spawnPoints.Length);
            spawn.transform.position = spawnPoints[random].position + Vector3.up;
        }
    }

    private void HealPlayer()
    {
        // Heal the player if max hitpoints are below half.
        if (playerTank.HitPoints <= playerTank.MaxHitpoints / 2)
        {
            playerTank.HitPoints += healAmount;
        }
    }

    private void WaveText(int waveNumber, bool isBoss)
    {
        waveText.text = isBoss ? "Boss" : $"Wave {waveNumber}: enemies alive {AliveEnemies.Count}";
    }
}