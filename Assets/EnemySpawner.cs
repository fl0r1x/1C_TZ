using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float minSpawnInterval = 1f; // Минимальное время ожидания до следующего спавна
    public float maxSpawnInterval = 3f; // Максимальное время ожидания до следующего спавна
    public int minEnemies = 5; // Минимальное количество врагов
    public int maxEnemies = 10; // Максимальное количество врагов
    public GameObject victoryPanel;
    public GameObject gameOverPanel; // Панель проигрыша
    public Transform finishLine; // Финишная линия
    public GameObject destructionEffect; // Эффект уничтожения врага
    public Image[] hearts; // Сердечки в UI
    private int remainingHearts = 3; // Количество оставшихся сердечек
    public float effectDuration = 2f; // Время отображения эффекта взрыва

    public float minEnemySpeed = 1f; // Минимальная скорость врага
    public float maxEnemySpeed = 3f; // Максимальная скорость врага

    private float spawnTimer;
    private float spawnInterval; // Интервал между спавнами
    private int spawnedEnemyCount = 0;
    private int totalEnemiesToSpawn; // Общее количество врагов для спавна
    private int destroyedEnemiesCount = 0; // Количество уничтоженных врагов
    private bool gameWon = false;
    private bool gameOver = false;

    public EnemyFactory enemyFactory; // Фабрика врагов

    void Start()
    {
        InitializeGame();

        if (enemyFactory == null)
        {
            Debug.LogError("Enemy factory is not assigned in the EnemySpawner script.");
        }
    }

    void Update()
    {
        if (gameWon || gameOver) return;

        if (spawnedEnemyCount >= totalEnemiesToSpawn)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
            // Устанавливаем новый случайный интервал между спавнами
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void InitializeGame()
    {
        // Сброс состояния игры
        gameWon = false;
        gameOver = false;
        remainingHearts = 3;
        destroyedEnemiesCount = 0; // Сброс количества уничтоженных врагов

        // Выбираем случайное количество врагов для спавна
        totalEnemiesToSpawn = Random.Range(minEnemies, maxEnemies + 1);

        // Устанавливаем начальный интервал между спавнами
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

        UpdateHeartUI();
        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f; // Сброс времени игры
        spawnedEnemyCount = 0;
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in the EnemySpawner script.");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = enemyFactory.CreateEnemy(spawnPoints[spawnIndex].position);

        // Передаем ссылку на спавнер врагу
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetSpawner(this);

            // Устанавливаем случайную скорость для врага
            float randomSpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
            enemyScript.SetSpeed(randomSpeed);
        }

        spawnedEnemyCount++;
        Debug.Log("Enemy spawned with speed " + enemyScript.GetSpeed() + ". Total spawned enemies: " + spawnedEnemyCount);
    }

    public void OnEnemyDestroyed(Vector3 position)
    {
        // Уменьшаем количество оставшихся врагов
        spawnedEnemyCount--;
        destroyedEnemiesCount++; // Увеличиваем количество уничтоженных врагов

        // Создаем эффект уничтожения
        GameObject effect = Instantiate(destructionEffect, position, Quaternion.identity);
        Destroy(effect, effectDuration); // Удаляем эффект через заданное время

        // Проверка завершения игры, если все заспавненные враги уничтожены
        if (destroyedEnemiesCount >= totalEnemiesToSpawn)
        {
            GameWon();
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !gameWon)
        {
            gameWon = true;
            Debug.Log("All enemies destroyed! Showing victory panel...");
            Invoke("ShowVictoryPanel", 1f);
        }
    }

    public void OnEnemyCrossedFinishLine(Vector3 position)
    {
        remainingHearts--;

        // Удаление сердечка
        if (remainingHearts >= 0)
        {
            hearts[remainingHearts].enabled = false;
        }

        // Создаем эффект уничтожения
        GameObject effect = Instantiate(destructionEffect, position, Quaternion.identity);
        Destroy(effect, effectDuration); // Удаляем эффект через заданное время

        // Проверка окончания игры
        if (remainingHearts <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over! Showing game over panel...");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Остановка игры
    }

    void GameWon()
    {
        gameWon = true;
        Debug.Log("Game Won! Showing victory panel...");
        victoryPanel.SetActive(true);
        Time.timeScale = 0f; // Остановка игры
    }

    void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            Debug.Log("Displaying victory panel.");
            victoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Victory panel is not assigned in the inspector!");
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Убираем паузу
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateHeartUI()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = true;
        }
    }
}
