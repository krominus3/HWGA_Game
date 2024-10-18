using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    void Update()
    {
        pos = player.position;
        pos.z = -10f;
        pos.y = player.position.y + 2;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
