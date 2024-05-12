using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Handle_Time : MonoBehaviour
{
    public TMP_Text tiempoRestanteText;
    private float tiempoRestante = 120;
    private GameDB_Manager GamedbManager;

    void Start()
    {
        GuardarHoraInicio();
        ActualizarTiempoRestante();
        InvokeRepeating("ActualizarContador", 1f, 1f);
    }

    private void GuardarHoraInicio()
    {
        DateTime HoraInicio = DateTime.Now;
        PlayerPrefs.SetString("HoraInicio", HoraInicio.ToString("HH:mm:ss"));
    }

    void ActualizarContador()
    {
        tiempoRestante -= 1;
        ActualizarTiempoRestante();
        if (tiempoRestante <= 0)
        {
            CancelInvoke("ActualizarContador");
            tiempoRestanteText.text = "Sobreviviste!";
            GameDB_Manager GamedbManager = FindObjectOfType<GameDB_Manager>();
            GamedbManager.InsertarVictoria();
            Debug.Log("Sobreviviste!");
            SceneManager.LoadScene("Menu");
        }
    }

    void ActualizarTiempoRestante()
    {
        tiempoRestanteText.text = "Tiempo restante: " + tiempoRestante.ToString();
        PlayerPrefs.SetString("TiempoRestante",tiempoRestante.ToString());
    }
}
