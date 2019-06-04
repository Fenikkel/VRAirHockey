using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int m_GoalsForWin = 6;
    public int m_EnemyGoals = 0;
    public int m_PlayerGoals = 0;

    public int m_GoalValue = 1;
    public int m_Round = 1;


    public TextMeshProUGUI m_PlayerScoreText;
    public TextMeshProUGUI m_EnemyScoreText;
    public TextMeshProUGUI m_RoundText;


    private PuckController m_PuckScript;
    public float m_StartPushForce = 5;

    public GameObject m_Puck;
    private Vector3 m_PuckSpawn;


    private void Start()
    {
        m_PuckSpawn = m_Puck.transform.position;
        m_PuckScript = m_Puck.GetComponent<PuckController>();
     

        //m_PuckSpawn = GameObject.FindGameObjectWithTag("Puck").transform.position;
        StartCoroutine(RoundCountdown());
    }
    private void Update()
    {
        

    }


    public void PlayerScores()
    {
        m_PlayerGoals += m_GoalValue;
        m_Round++;
        m_PlayerScoreText.text = "Player: " + m_PlayerGoals;
        StartCoroutine(RoundCountdown());
    }

    public void EnemyScores()
    {
        m_EnemyGoals += m_GoalValue;
        m_Round++;
        m_EnemyScoreText.text = "Enemy: " + m_EnemyGoals;
        StartCoroutine(RoundCountdown());

    }

    public void StartRound()
    {

        float z = Mathf.Pow(-1, m_Round);

        Vector3 direction = new Vector3(0, 0, z);

        m_PuckScript.KnockBack(direction * m_StartPushForce);
    }

    IEnumerator RoundCountdown()
    {
        //HAY QUE VACIARLO DE FUERZAS Y VOLVER A MODO NORMAL
        m_Puck.SetActive(true);
        m_Puck.transform.position = m_PuckSpawn;

        Color col = m_RoundText.color;
        col.a = 255;
        m_RoundText.color = col;


        m_RoundText.text = "Round " + m_Round;

        yield return new WaitForSeconds(1);

        col.a = 0;
        m_RoundText.color = col;
        StartRound();
    }
}
