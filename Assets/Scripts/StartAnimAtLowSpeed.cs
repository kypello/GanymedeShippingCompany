using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimAtLowSpeed : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animation>()["SpaceBackgroundSpin"].speed = 0.001f;
        GetComponent<Animation>().Play();
    }
}
