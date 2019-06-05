using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour {

    public KeyCode botonCerrar = KeyCode.C, 
        botonReiniciar = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(botonReiniciar))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        else if (Input.GetKeyDown(botonCerrar))
        {
            Application.Quit();
        }
    }
}
