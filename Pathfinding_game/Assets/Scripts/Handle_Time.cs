using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Handle_Time : MonoBehaviour
{
    public TMP_Text tiempoRestanteText;
    private float tiempoRestante = 120;

    void Start()
    {
        ActualizarTiempoRestante();
        InvokeRepeating("ActualizarContador", 1f, 1f);
    }

    void ActualizarContador()
    {
        tiempoRestante -= 1;
        ActualizarTiempoRestante();
        
        if (tiempoRestante <= 0)
        {
            CancelInvoke("ActualizarContador");
            tiempoRestanteText.text = "Sobreviviste!";
            Debug.Log("Sobreviviste!");
            SceneManager.LoadScene("Menu");
        }
    }

    void ActualizarTiempoRestante()
    {
        tiempoRestanteText.text = "Tiempo restante: " + tiempoRestante.ToString();
    }
}
