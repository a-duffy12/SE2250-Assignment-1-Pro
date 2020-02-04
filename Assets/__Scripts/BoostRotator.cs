using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3 (0, -30, 0) * Time.deltaTime); // deltaTime ensures that the animation is framerate independent
    }
}
