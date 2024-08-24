using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray forwardRay = new Ray(transform.position, transform.forward + Vector3.down);
        Ray backRay = new Ray(transform.position, -transform.forward + Vector3.down);
        Ray rightRay = new Ray(transform.position, transform.right + Vector3.down);
        Ray leftRay = new Ray(transform.position, -transform.right +  Vector3.down);
    }
}
