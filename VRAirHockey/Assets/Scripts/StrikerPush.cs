using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerPush : MonoBehaviour
{
    private PuckController m_PuckController;
    public float m_PushForce = 5;
    private Vector3 m_HitDirection; 


    void Start()
    {
        m_PuckController = FindObjectOfType<PuckController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {

            //print("pUSHED");

            m_HitDirection = other.transform.position - transform.position; //asi cojemos la direccion contraria a la que el jugador estaba iendo

            m_HitDirection =  m_HitDirection * m_PushForce;

            //hitDirection = hitDirection.normalized; //normalizamos para que sea unitario y el impuso que pille no depenga del tamaño del vector

            m_PuckController.KnockBack(m_HitDirection);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {
            m_PuckController.KnockBack(m_HitDirection);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //SONIDO DE GOLPE
    }
}
