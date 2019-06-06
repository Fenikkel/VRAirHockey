using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    public AudioSource m_HittedObjectAudio;
    private float m_OriginalPitch;
    public float m_PitchRange = 0.2f;

    private void Start()
    {
        m_OriginalPitch = m_HittedObjectAudio.pitch;

    }



    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Puck")
        {

            m_HittedObjectAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); //valor entre 0.8 i 1.2 //se fa eixina per a no tindre magic numbers en el cas de que es restructurara el codic
            m_HittedObjectAudio.Play();

            //other.GetComponent<AudioSource>().Play(); //mejor si hago que el object puck tenga el sonido hit
            //print("Sounded");
        }
    }
}
