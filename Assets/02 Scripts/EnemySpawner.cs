using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;
    public float damageMax = 40f, damageMin = 20f;
    public float healthMax = 200f, healthMin = 100f;
    public float speedMax = 3f, speedMin = 1f;
    public Color strongEnemycolor = Color.red;
    List<Enemy> enemies = new List<Enemy>();
    int wave;

    void Update()
    {
        if(GameManager.instance != null && GameManager.instance.isGameover) {
            return;
        }
        if (enemies.Count <= 0)
        {
            SpawnWave();
        }
        UpdateUI();
    }
    void SpawnWave() {
        wave++;
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        for(int i = 0; i < spawnCount; i++) {
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity);
        }
    }
    void UpdateUI()  {
        UIManager.instance.UpdateWaveText(wave, enemies.Count);
    }
    void CreateEnemy(float intensity) {
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);
        Color skinColor = Color.Lerp(Color.white, strongEnemycolor, intensity);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(health, damage, speed, skinColor);
        enemies.Add(enemy);
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }
}
