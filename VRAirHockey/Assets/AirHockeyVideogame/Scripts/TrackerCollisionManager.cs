using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://forum.unity.com/threads/using-vive-controllers-to-set-velocities.410824/ 
//el plugin ES ESTA VERSION:  https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class TrackerCollisionManager : MonoBehaviour
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
