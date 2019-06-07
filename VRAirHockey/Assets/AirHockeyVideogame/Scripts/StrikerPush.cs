using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class StrikerPush : MonoBehaviour
{
    private PuckController m_PuckController;
    public float m_PushForce = 5;
    private Vector3 m_HitDirection;
    private Vector3 m_TrackedObjectVelocity;
    public SteamVR_TrackedObject m_TrackedObject;




    void Start()
    {
        m_PuckController = FindObjectOfType<PuckController>();
    }

    private void FixedUpdate()
    {
        //m_TrackedObjectVelocity = m_TrackedObject.GetVelocityVector();
    }

    private void Update()
    {
        //print(m_TrackedObjectVelocity);
    }


    /* private void OnTriggerEnter(Collider other)
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
     }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Puck")
        {

            //print("pUSHED");

            //coudado con los bordes, cambiarlos
            m_HitDirection = collision.collider.transform.position - transform.position;  //asi cojemos la direccion contraria a la que el jugador estaba iendo

            //m_HitDirection = collision.collider.transform.position - new Vector3(transform.position.x, 0, transform.position.y); //asi cojemos la direccion contraria a la que el jugador estaba iendo

            m_HitDirection = m_HitDirection.normalized; //normalizamos para que sea unitario y el impuso que pille no depenga del tamaño del vector

            //collision.collider.GetComponent<Rigidbody>().velocity;

            m_HitDirection = Vector3.Scale(m_HitDirection,  m_TrackedObjectVelocity); 


            m_HitDirection = m_HitDirection* m_PushForce ;


            m_HitDirection.z = 0;

            m_PuckController.KnockBack(m_HitDirection);

            print(m_HitDirection);

        }
    }

    /*public void SetTrackerVelocityVector(Vector3 velocity)
    {
        m_TrackedObjectVelocity = velocity;
        
    }*/
}
