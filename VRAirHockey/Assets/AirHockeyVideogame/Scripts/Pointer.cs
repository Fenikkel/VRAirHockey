﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Pointer : MonoBehaviour
{

    public float m_DefaultLength = 5.0f; //Ray length
    public GameObject m_Dot;
    public VRInputModule m_InputModule;

    private LineRenderer m_LineRender = null;


    private void Awake()
    {
        m_LineRender = GetComponent<LineRenderer>();
    }


    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // Use defauld or distance

        PointerEventData data = m_InputModule.GetData();

        float targetLength = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : data.pointerCurrentRaycast.distance; //Si choca contra  el boton, pilla la distancia del boton, sino la defauld

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //Default

        Vector3 endPosition = transform.position + (transform.forward * targetLength); // cojes posicion inicial y le sumas la longitud del rayo (en la direccion que toca) y obtienes la posicion final

        //Or based on hit
        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        //Set position of the dot
        m_Dot.transform.position = endPosition;

        //Set linerenderer
        m_LineRender.SetPosition(0, transform.position); //Beginning of the line render
        m_LineRender.SetPosition(1, endPosition); //End of the line render


    }


    private RaycastHit CreateRaycast(float length)
    {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);



        return hit;

    }
}
