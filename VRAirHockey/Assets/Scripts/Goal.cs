using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager m_GameManager;

    public bool m_PlayerGoal; // Si es la porteria del jugador 


    private void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {
            //print("Destroy");
            //Destroy(other.gameObject);
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);

            if (m_PlayerGoal)
            {
                m_GameManager.EnemyScores();

            }
            else
            {
                m_GameManager.PlayerScores();

            }
            Time.timeScale = 0.25f;



        }
    }
}
