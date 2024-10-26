using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boss_room : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    private Animator anim1;
    private Animator anim2;

    void Start()
    {
        anim1 = door1.GetComponentInChildren<Animator>();
        anim2 = door2.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject))
        {
            anim1.SetTrigger("close");
            anim2.SetTrigger("close");
        }

    }
}
