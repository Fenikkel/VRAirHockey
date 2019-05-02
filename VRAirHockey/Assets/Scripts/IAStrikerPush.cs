using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAStrikerPush : MonoBehaviour
{
    private PuckController m_PuckController;
    public float m_PushForce = 5;
    private Vector3 m_HitDirection;

    //private float m_RandomBounce;
    //public float m_RandomBounceLimit = 0.05f;


    void Start()
    {
        //m_RandomBounce = Random.Range(-m_RandomBounceLimit, m_RandomBounceLimit);
        m_PuckController = FindObjectOfType<PuckController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {

            print("pUSHED");

            m_HitDirection = other.transform.position - transform.position; //asi cojemos la direccion contraria a la que el jugador estaba iendo

            m_HitDirection =  m_HitDirection * m_PushForce;

            //hitDirection = hitDirection.normalized; //normalizamos para que sea unitario y el impuso que pille no depenga del tamaño del vector

            m_PuckController.KnockBack(m_HitDirection);

        }
    }

    private void OnTriggerStay(Collider other)
    {

        //SI ESTA ALGO DE RATO, HACEMOS EL RETREAT

        m_HitDirection = new Vector3(m_HitDirection.x, m_HitDirection.y, m_HitDirection.z);

        //m_HitDirection = new Vector3(m_HitDirection.x + m_RandomBounce, m_HitDirection.y, m_HitDirection.z - m_RandomBounce);
        m_PuckController.KnockBack(m_HitDirection);
    }

    private void OnTriggerExit(Collider other)
    {
        //SONIDO DE GOLPE
    }
}
