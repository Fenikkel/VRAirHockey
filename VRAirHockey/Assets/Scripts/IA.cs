using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    //MEJOR SI EL PUCK EMPIEZA EN EL AREA DEL JUGADOR Y NO LA ENEMIGA

    public float m_MaxMovementSpeed;
    private Rigidbody m_EnemyRB;
    private Vector3 m_StartingPosition;

    public Rigidbody m_PuckRB;

    public Transform m_EnemyBoundaryHolder; //el boundary de la zona que puede menearse el jugador
    private Boundary m_EnemyBoundary;
    //Estos dos boudaries son diferentes por los tamaños diferentes del puck y del striker
    public Transform m_EnemyPuckBoundaryHolder; //el boundary de la zona enemiga que el puck puede menearse
    private Boundary m_EnemyPuckBoundary;

    private Vector3 m_TargetPosition;

    private bool m_isFirstTimeInOpponentsHalf = true; //primera vez que esta en el campo enemigo
    private float m_OffsetXFromTarget;

    public float m_RetreatTime = 1;
    private float m_RetreatCounter;

    private void Start()
    {
        m_EnemyRB = GetComponent<Rigidbody>();
        m_StartingPosition = m_EnemyRB.position;

        m_EnemyBoundary = new Boundary(m_EnemyBoundaryHolder.GetChild(0).position.z, //up
                                        m_EnemyBoundaryHolder.GetChild(1).position.z, //down
                                        m_EnemyBoundaryHolder.GetChild(2).position.x, //left
                                        m_EnemyBoundaryHolder.GetChild(3).position.x //right
                                        );

        m_EnemyPuckBoundary = new Boundary(m_EnemyPuckBoundaryHolder.GetChild(0).position.z, //up
                                m_EnemyPuckBoundaryHolder.GetChild(1).position.z, //down
                                m_EnemyPuckBoundaryHolder.GetChild(2).position.x, //left
                                m_EnemyPuckBoundaryHolder.GetChild(3).position.x //right
                                );
    }

    private void FixedUpdate()
    {
        Debug.Log(m_RetreatCounter);
        float movementSpeed;



        if (m_PuckRB.position.z < m_EnemyPuckBoundary.Down || m_RetreatCounter > 0) //si el puck esta en la zona del otro jugador...
        {

            m_RetreatCounter -= Time.fixedDeltaTime;

            if (m_isFirstTimeInOpponentsHalf)
            {
                m_isFirstTimeInOpponentsHalf = false;
                m_OffsetXFromTarget = Random.Range(-1f, 1f);
            }

            movementSpeed = m_MaxMovementSpeed * Random.Range(0.1f, 0.3f);
            m_TargetPosition = new Vector3(Mathf.Clamp(m_PuckRB.position.x + m_OffsetXFromTarget, m_EnemyBoundary.Left,
                                                        m_EnemyBoundary.Right),
                                           m_StartingPosition.y,
                                           m_StartingPosition.z); //si esta fuera vuelve a proteger la porteria
        }
        else //si esta dentro del area del IA
        {
            m_isFirstTimeInOpponentsHalf = true;


            movementSpeed = Random.Range(m_MaxMovementSpeed * 0.4f, m_MaxMovementSpeed);
            m_TargetPosition = new Vector3(Mathf.Clamp(m_PuckRB.position.x, m_EnemyBoundary.Left,
                                            m_EnemyBoundary.Right),
                                            m_StartingPosition.y,
                                            Mathf.Clamp(m_PuckRB.position.z, m_EnemyBoundary.Down,
                                            m_EnemyBoundary.Up));

        }

        m_EnemyRB.MovePosition(Vector3.MoveTowards(m_EnemyRB.position, m_TargetPosition, movementSpeed * Time.fixedDeltaTime));

        

    }


    public void Retreat()
    {
        m_RetreatCounter = m_RetreatTime;
    }



}
