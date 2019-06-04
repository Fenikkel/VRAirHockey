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
    private float m_RetreatCounter = 0;
    private float m_CulDeSacCounter = 0; 
    public float m_CulDeSacTimeTrigger = 1.5f; //tiempo que tarda en activarse la retirada
    public float m_CulDeSacDistance = 6.0f;

    public float m_HitOffset = 0.2f; //Molt cutre aço


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
    private void Update()
    {
        Closer();
    }
    private void FixedUpdate()
    {
        //Debug.Log(m_RetreatCounter);
        float movementSpeed;



        if (m_RetreatCounter > 0)
        {
            m_RetreatCounter -= Time.fixedDeltaTime;

            movementSpeed = m_MaxMovementSpeed * Random.Range(0.1f, 0.3f);
            m_TargetPosition = new Vector3(m_StartingPosition.x,
                                           m_StartingPosition.y,
                                           m_StartingPosition.z); //vuelve a proteger la porteria
        }

        else if (m_PuckRB.position.z < m_EnemyPuckBoundary.Down) //si el puck esta en la zona del otro jugador o esta en modo retirada
        {

           

            if (m_isFirstTimeInOpponentsHalf) //debuff para el enemigo
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

            //No ha de ir a la posicion, sino a esa posicion -+ la mitad del tamaño del puck

            float xOffset = m_PuckRB.position.x - this.transform.position.x; //luego lo reutilizamos poniendo el valor adecuado
            float zOffset = m_PuckRB.position.z - this.transform.position.z;


            if (xOffset<0)
            {
                xOffset = m_PuckRB.position.x + m_HitOffset;
            }
            else
            {
                xOffset = m_PuckRB.position.x - m_HitOffset;
            }

            if (zOffset < 0)
            {
                zOffset = m_PuckRB.position.z + m_HitOffset;
            }
            else
            {
                zOffset = m_PuckRB.position.z - m_HitOffset;
            }


            m_TargetPosition = new Vector3(Mathf.Clamp(xOffset, m_EnemyBoundary.Left,
                                            m_EnemyBoundary.Right),
                                            m_StartingPosition.y,
                                            Mathf.Clamp(zOffset, m_EnemyBoundary.Down,
                                            m_EnemyBoundary.Up));


            /*m_TargetPosition = new Vector3(Mathf.Clamp(m_PuckRB.position.x, m_EnemyBoundary.Left,
                                            m_EnemyBoundary.Right),
                                            m_StartingPosition.y,
                                            Mathf.Clamp(m_PuckRB.position.z, m_EnemyBoundary.Down,
                                            m_EnemyBoundary.Up));*/

        }

        m_EnemyRB.MovePosition(Vector3.MoveTowards(m_EnemyRB.position, m_TargetPosition, movementSpeed * Time.fixedDeltaTime));

        

    }


    public void Retreat()
    {
        m_RetreatCounter = m_RetreatTime;
    }

    private void Closer()
    {
        float distance = Vector3.Distance(this.transform.position, m_PuckRB.position);

        if (distance <= m_CulDeSacDistance)
        {
            m_CulDeSacCounter += Time.deltaTime;

            if (m_CulDeSacTimeTrigger < m_CulDeSacCounter)
            {
                Retreat(); //se retira el striker
                m_CulDeSacCounter = 0; //para que no entre mientras este en retreat
            }
        }
        else
        {
            m_CulDeSacCounter = 0;
        }
        //print("DistanceBetween "+ distance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Puck")
        {
            Retreat();

        }
    }



}
