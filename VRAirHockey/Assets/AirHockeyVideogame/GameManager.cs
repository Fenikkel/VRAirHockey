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
    public TextMeshProUGUI m_GoalText;
    public TextMeshProUGUI m_GoText;
    public TextMeshProUGUI m_CounterText;


    public Color m_RedColor;
    public Color m_BlueColor;
    public Color m_WhiteColor;




    private PuckController m_PuckScript;
    public float m_StartPushForce = 5;

    public GameObject m_Puck;
    private Rigidbody m_PuckRB;
    private Vector3 m_PuckSpawn;
    public AirHockeyPlayerController m_PlayerController;
    private IA m_EnemyIA;
    public GameObject m_PlayerStriker;
    private Vector3 m_PlayerStrikerSpawn;
    public GameObject m_EnemyStriker;
    private Vector3 m_EnemyStrikerSpawn;

    private TimeManager m_TimeManager;

    public bool m_VRMode = false;


    private void Start()
    {
        m_PuckRB = m_Puck.GetComponent<Rigidbody>();
        m_PuckSpawn = m_Puck.transform.position;
        m_PuckScript = m_Puck.GetComponent<PuckController>();
        m_EnemyIA = m_EnemyStriker.GetComponent<IA>();
        m_TimeManager = GetComponent<TimeManager>();

        m_PlayerStrikerSpawn = m_PlayerStriker.transform.position;
        m_EnemyStrikerSpawn = m_EnemyStriker.transform.position;

        m_EnemyScoreText.text = m_EnemyGoals.ToString();
        m_PlayerScoreText.text = m_PlayerGoals.ToString();



        Color col = m_GoalText.color;
        col.a = 0;
        m_GoalText.color = col;

        col = m_GoText.color;
        col.a = 0;
        m_GoText.color = col;

        col = m_RoundText.color;
        col.a = 0;
        m_RoundText.color = col;

        col = m_CounterText.color;
        col.a = 0;
        m_CounterText.color = col;


        //m_PuckSpawn = GameObject.FindGameObjectWithTag("Puck").transform.position;
        StartCoroutine(StartGame());
    }
    
    public void PlayerScores()
    {
        m_PlayerGoals += m_GoalValue;
        m_Round++;
        m_GoalText.color = m_RedColor;

        m_PlayerScoreText.text = m_PlayerGoals.ToString();
        m_GoalText.text = "RED SCORES";
        StartCoroutine(RoundCountdown());
    }

    public void EnemyScores()
    {
        m_EnemyGoals += m_GoalValue;
        m_Round++;
        m_GoalText.color = m_BlueColor;
        m_GoalText.text = "BLUE SCORES";
        m_EnemyScoreText.text = m_EnemyGoals.ToString();
        StartCoroutine(RoundCountdown());

    }

    private void ResetPlayground()
    {
        if (!m_VRMode)
        {
            m_PlayerController.ControlsEnabled(false);
            m_PlayerController.ControlsEnabled(false);
            m_PlayerStriker.transform.position = m_PlayerStrikerSpawn;


        }
        m_EnemyIA.EnableIA(false);

        m_Puck.SetActive(true);
        m_Puck.transform.position = m_PuckSpawn;
        m_PuckRB.velocity = Vector3.zero;
        m_EnemyStriker.transform.position = m_EnemyStrikerSpawn;

        m_PuckScript.RestartMaterial();

    }

    public void StartRound()
    {

        float z = Mathf.Pow(-1, m_Round);

        Vector3 direction = new Vector3(0, 0, z);

        m_PuckScript.KnockBack(direction * m_StartPushForce);
        m_EnemyIA.EnableIA(true);

        if (!m_VRMode)
        {
            m_PlayerController.ControlsEnabled(true);

        }

        //Time.timeScale = 1.0f;
    }

    IEnumerator RoundCountdown()
    {
        Color col = m_GoalText.color;

        //Time.timeScale = 0.25f;
        m_TimeManager.DoSlowmotion();

        //Goal text
        /*col = m_GoalText.color;
        col.a = 255;
        m_GoalText.color = col;*/
        yield return new WaitForSeconds(1.0f);

        //goal text fade
        col.a = 0;
        m_GoalText.color = col;

        col = m_WhiteColor;//m_RoundText.color;
        //col.a = 255;
        m_RoundText.color = col;
        m_RoundText.text = "Round " + m_Round;

        ResetPlayground();

        yield return new WaitForSeconds(1.0f);

        col.a = 0;
        m_RoundText.color = col;

        col = m_WhiteColor;//m_GoText.color;
        //col.a = 255;
        m_GoText.color = col;
        

        StartRound();


        yield return new WaitForSeconds(0.5f);

        col.a = 0;
        m_GoText.color = col;

    }

    IEnumerator StartGame()
    {

        Color col = m_WhiteColor;

        ResetPlayground();

        //tree

        col = m_WhiteColor;
        m_CounterText.text = "3";
        m_CounterText.color = col;
        yield return new WaitForSeconds(0.5f);
        m_CounterText.text = "2";

        yield return new WaitForSeconds(0.5f);
        m_CounterText.text = "1";

        yield return new WaitForSeconds(0.5f);

        col.a = 0;
        m_CounterText.color = col;
        //GO in
        col = m_WhiteColor;//m_GoText.color;
        //col.a = 255;
        m_GoText.color = col;

        StartRound();

        yield return new WaitForSeconds(0.5f);

        //go out

        col.a = 0;
        m_GoText.color = col;
    }
}
