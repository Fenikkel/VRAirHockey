using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckController : MonoBehaviour
{

    private Rigidbody m_PuckRB;
    private AudioSource m_HitAudioSource;

    private float m_OriginalPitch;
    public float m_PitchRange = 0.1f;

    

    void Start()
    {
        m_PuckRB = GetComponent<Rigidbody>();
        m_HitAudioSource = GetComponent<AudioSource>();

        m_OriginalPitch = m_HitAudioSource.pitch;

    }


    public void KnockBack(Vector3 direction)
    {
        //print("KNOCK KNOCK ");


        m_PuckRB.AddForce(direction, ForceMode.Impulse); //Add an instant force impulse to the rigidbody, using its mass.

        /*if (m_PuckRB.velocity.Equals(Vector3.zero))
        {
            print("yeah");
            m_PuckRB.AddForce(direction, ForceMode.Force); //Add an instant force impulse to the rigidbody, using its mass.

        }
        else
        {
            m_PuckRB.AddForce(direction, ForceMode.Impulse); //Add an instant force impulse to the rigidbody, using its mass.
        }*/

        //Debug.Log(direction);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //da igual con que
        m_HitAudioSource.Play();
        m_HitAudioSource.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); //valor entre 0.8 i 1.2 //se fa eixina per a no tindre magic numbers en el cas de que es restructurara el codic
        m_HitAudioSource.Play();

    }

}
