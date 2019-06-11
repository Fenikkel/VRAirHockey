using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule //hereda del BaseInputModule (es un implements?)
{
    //Funcionna para SteamVR 2.2

    public Camera m_Camera;
    //public SteamVR_Input_Sources m_TargetSource; //solo funciona en 2.2
    //public SteamVR_Action_Boolean m_ClickAction; //solo funciona en 2.2

    private GameObject m_CurrentObject = null;
    private PointerEventData m_Data = null;

    private GameObject m_LastObject = null;

    //Para hacerlo desde la camara y no del mando
    public float m_PressTime = 1.5f; //eL INPUT SYSTEM ES asincrono o va a ridmo del engine, osea que esta entre FixedTime y DeltaTime
    private float m_PressCounter = 0.0f;

    public float m_ClickTime = 0.25f;
    private float m_ClickCounter = 0.0f;

    public bool m_ClickDown = false;
    public bool m_ClickUp = false;

    

    protected override void Awake()
    {
        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }
    protected override void Start()
    {
        m_PressCounter = m_PressTime;
        m_ClickCounter = m_ClickTime;

        //m_LastObject = new GameObject();
    }

    public override void Process() //lo que hace todo el tiempo mientras apunta?
    {
        //Reset data, set camera
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        // Raycast
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache); //pasamos al event sistem la data and resultcache?
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        //clear
        m_RaycastResultCache.Clear();

        //Hover
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        if (m_CurrentObject!=null && m_LastObject !=null && m_LastObject.Equals(m_CurrentObject))
        {
            
            m_PressCounter -= Time.deltaTime;
            if (m_PressCounter < 0.0f)
            {
                m_ClickDown = true;
            }
        }
        else
        {
            m_PressCounter = m_PressTime;
        }

        //print("Lst: " + m_LastObject);
        //print("Curr: " + m_CurrentObject);
        print(m_PressCounter);

        m_LastObject = m_CurrentObject;

        if (m_ClickDown)
        {
            ProcessPress(m_Data);
            m_ClickCounter -= Time.deltaTime;

            if (m_ClickCounter < 0.0f)
            {
                m_ClickUp = true;
            }
        }

        if (m_ClickUp)
        {
            m_ClickDown = false;
            m_ClickUp = false;
            m_PressCounter = m_PressTime;
            m_ClickCounter = m_ClickTime;

            ProcessRelease(m_Data);
            
        }

        /*
        //Press
        if (m_ClickAction.GetStateDown(m_TargetSource))
        {
            ProcessPress(m_Data);
        }

        //Press
        if (m_ClickAction.GetStateUp(m_TargetSource))
        {
            ProcessRelease(m_Data);
        }
        */


    }

    public PointerEventData GetData()
    {
        return m_Data;
    }

    private void ProcessPress(PointerEventData data)
    {
        // Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        //Check for object hit, get the down handler, call
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject,data,ExecuteEvents.pointerDownHandler);

        // If no down handler, try and get click handler
        if (newPointerPress = null)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);
        }

        //set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = m_CurrentObject;

    }

    private void ProcessRelease(PointerEventData data)
    {
        //Execute pointer up

        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler); //Hace que el boton este presionado

        //Check for click Handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);


        //Check if actual

        ExecuteEvents.Execute(pointerUpHandler, data, ExecuteEvents.pointerClickHandler);


        /*I think I may have the solution of the OnClick() problem:

In the ProcessRelease function, seems that data.pointerPress == pointerUpHandler is never true (I think they are diferent things: data.pointerPress is the object that recived the OnPointerDown, and   pointerUpHandler is the event handler of the pressed object)

If you delete the if statemend and put instead this: ExecuteEvents.Execute(pointerUpHandler, data, ExecuteEvents.pointerClickHandler);

It will work but we don't check if button pressed previously is the same as the one we are releasing as Andrew says in the tutorial.

Someone know how to fix this if statement? ->  data.pointerPress == pointerUpHandler

PD: Andrew you are still the best <3*/


        /*if (data.pointerPress != null &&  pointerUpHandler!= null && data.pointerPress.Equals(pointerUpHandler)) //data.pointerPress == pointerUpHandler
        {
            print("Este parece ser que no funciona porque no son iguales...");

            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
            
        }*/

        //clear selected gameobject

        eventSystem.SetSelectedGameObject(null);

        //reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;

        
        /*pointerUpHandler.pressPosition = Vector2.zero;
        pointerUpHandler.pointerPress = null;
        pointerUpHandler.rawPointerPress = null;*/
    }

}
