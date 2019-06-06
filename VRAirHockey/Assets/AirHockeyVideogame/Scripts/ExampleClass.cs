using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public GameObject particle;
    void Update()
    {
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                Instantiate(particle, hit.transform.position, hit.transform.rotation);
        }
    }
}