using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boss_room : MonoBehaviour
{
    [SerializeField] Animator doorAnim1;
    [SerializeField] Animator doorAnim2;
    [SerializeField] Rigidbody2D bossRB;

    private BoxCollider2D bc;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject))
        {
            doorAnim1.SetTrigger("close");
            doorAnim2.SetTrigger("close");
            bc.enabled = false;
            bossRB.simulated = true;
        }

    }
}
