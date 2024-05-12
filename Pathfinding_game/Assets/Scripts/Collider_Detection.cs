using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collider_Detection : MonoBehaviour
{
    private GameDB_Manager GamedbManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Personaje"))
        {
            GameDB_Manager GamedbManager = FindObjectOfType<GameDB_Manager>();
            GamedbManager.InsertarDerrota();
            SceneManager.LoadScene("Menu");
        }
    }
}
