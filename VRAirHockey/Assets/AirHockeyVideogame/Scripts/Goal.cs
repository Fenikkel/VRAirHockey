using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager m_GameManager;

    public bool m_PlayerGoal; // Si es la porteria del jugador 
    public ParticleSystem m_Explosion;
    private ParticleSystem m_Rubish;


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
            m_Rubish = Instantiate(m_Explosion, other.transform.position, Quaternion.identity);
            StartCoroutine(WaitForDestroy());
            other.gameObject.SetActive(false);

            if (m_PlayerGoal)
            {
                m_GameManager.EnemyScores();

            }
            else
            {
                m_GameManager.PlayerScores();

            }



        }
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(m_Explosion.main.duration / m_Explosion.main.simulationSpeed);
        Destroy(m_Rubish);

    }
}
