using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collider_Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Personaje"))//Collisiona y es el jugador
        {
            //LLeva al menú
            SceneManager.LoadScene("Menu");
        }
    }
}
