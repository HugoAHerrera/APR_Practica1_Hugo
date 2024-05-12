using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Load_Game : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    public TMP_InputField nombreInputField;
    private DB_Manager dbManager;

    public void CargarEscenaDeJuego()
    {
        string modo = Dropdown.options[Dropdown.value].text;
        string nombre = nombreInputField.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.Log("Escriba el nombre con el que se registrará la partida");
            return;
        }
        PlayerPrefs.SetString("ModoSeleccionado", modo);
        PlayerPrefs.SetString("NombreJugador", nombre);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Juego");

        DB_Manager dbManager = FindObjectOfType<DB_Manager>();
        dbManager.InsertarJugador();
    }
}

