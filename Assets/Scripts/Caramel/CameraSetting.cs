using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour
{
    //in the Camera

    float baseWidth = 1067;
    float baseHeight = 1600;

    void Awake()
    {
        GetComponent<Camera>().aspect = baseWidth / baseHeight;
    }
}
