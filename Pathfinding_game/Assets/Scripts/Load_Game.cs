using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Load_Game : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    public TMP_InputField nombreInputField;

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
    }

    public void descargarMarcado()
    {
        Debug.Log("Descarga de datos");
    }

    public void borrarHistorialJugador()
    {
        string nombre = nombreInputField.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.Log("Escribe el nombre del jugador que quieres borrar los datos");
            return; 
        }
        Debug.Log("Registros borrados del jugador " + nombre);
    }
}

