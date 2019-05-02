using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckController : MonoBehaviour
{

    private Rigidbody m_PuckRB;

    void Start()
    {
        m_PuckRB = GetComponent<Rigidbody>();
    }


    public void KnockBack(Vector3 direction)
    {
        //print("KNOCK KNOCK ");

        m_PuckRB.AddForce(direction, ForceMode.Impulse); //Add an instant force impulse to the rigidbody, using its mass.


        //Debug.Log(direction);
        
    }
}
