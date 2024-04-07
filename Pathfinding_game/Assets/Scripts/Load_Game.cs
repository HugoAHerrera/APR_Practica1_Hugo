using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Game : MonoBehaviour
{

    public void CargarEscenaDeJuego()
    {
        SceneManager.LoadScene("Juego");
    }
}
