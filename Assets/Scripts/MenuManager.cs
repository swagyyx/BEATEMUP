using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
