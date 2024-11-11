using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private Animator anim;
    private BoxCollider2D bc;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject) && (!Hero.Instance.getHit) && (!Hero.Instance.isInvulnerability))
        {
            Hero.Instance.GetDamage(damage, bc.transform);
            Debug.Log("touch");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject) && (!Hero.Instance.isInvulnerability))
            anim.SetTrigger("active");
    }
}
