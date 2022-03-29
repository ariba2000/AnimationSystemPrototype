using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    [Range(0.5f, 10)]
    public float scale;

    void Start()
    {

    }

    void Update()
    {
        transform.position = player.transform.position + scale * offset;
    }
}
