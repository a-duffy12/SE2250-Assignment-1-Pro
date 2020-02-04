using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineRotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3 (5, 60, 5) * Time.deltaTime); // deltaTime ensures that the animation is framerate independent
    }
}
