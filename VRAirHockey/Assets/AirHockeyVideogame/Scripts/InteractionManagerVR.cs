using UnityEngine;
using System.Collections;
using System;
using Valve.VR;
using Valve.VR.InteractionSystem;



// https://forum.unity.com/threads/using-vive-controllers-to-set-velocities.410824/ 

//el plugin ES ESTA VERSION:  https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class InteractionManagerVR : MonoBehaviour

{

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public int m_CollisionVelocity = 100; 

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void OnTriggerStay(Collider col)
    {
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            col.attachedRigidbody.isKinematic = true;
            col.gameObject.transform.SetParent(this.gameObject.transform);
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;

            tossObject(col.attachedRigidbody);
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            hitObject(col.attachedRigidbody);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitObject(collision.collider.attachedRigidbody);
    }

    void tossObject(Rigidbody rigidbody)
    {
        rigidbody.velocity = device.velocity;
        rigidbody.angularVelocity = device.angularVelocity;
    }


    void hitObject(Rigidbody rigidbody)
    {
        rigidbody.velocity = device.velocity * m_CollisionVelocity;
        rigidbody.angularVelocity = device.angularVelocity * m_CollisionVelocity;
    }
}