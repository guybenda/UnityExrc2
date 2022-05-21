using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 5, Color.green);

        if (Physics.Raycast(transform.position, -Vector3.up, 0.8f))
        {
            Debug.Log("HEET");
            gameObject.GetComponent<Rigidbody>().velocity += Vector3.up*5f;
        }
    }
}
