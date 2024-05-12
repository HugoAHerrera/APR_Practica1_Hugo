using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    //Cada 10 segundos el gamemanager spawnea un enemigo más al juego
    public GameObject enemyPrefab;
    public Transform player;
    public Transform[] spawnPoints;
    public float intervaloSpawn;

    void Start()
    {
        string modo = PlayerPrefs.GetString("ModoSeleccionado", "");
        if(modo == "Normal")
        {
            intervaloSpawn = 10f;
        }else
        {
            intervaloSpawn = 5f;
        }
        InvokeRepeating("SpawnEnemy", intervaloSpawn, intervaloSpawn);
        Instantiate(enemyPrefab, spawnPoints[0].position, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        //Quiero que aparezca en la posición más alejada del jugador
        Vector3 puntoMasLejano = Vector3.zero;
        float maxDistance = 0f;
        foreach (Transform spawnPoint in spawnPoints)
        {
            float distanceToPlayer = Vector3.Distance(spawnPoint.position, player.position);
            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                puntoMasLejano = spawnPoint.position;
            }
        }

        Instantiate(enemyPrefab, puntoMasLejano, Quaternion.identity);
    }
}
