using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage = 1;

    private CapsuleCollider2D cc;

    void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject) && (!Hero.Instance.isInvulnerability))
        {
            Hero.Instance.GetDamage(damage, cc.transform);
            Debug.Log("touch");
        }
    }

    
}
