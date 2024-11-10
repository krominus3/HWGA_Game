using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Collectable_coin : MonoBehaviour
{
    private Game_manager game_Manager;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private float destroyTime = 0.3f;

    private void Start()
    {
        game_Manager = Game_manager.Instance; // Получаем ссылку на Game_manager
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hero")
        {
            game_Manager.coinsCount++;
            StartCoroutine(CollectEffect());

        }
    }

    private IEnumerator CollectEffect()
    {
        Color _color = sr.material.color;

        rb.gravityScale = 1;
        rb.velocity = new Vector2 (1, 2);
        float tempTime;
        tempTime = destroyTime / 100;
        for (int i = 100; i > 0; i--)
        {
            _color.a = i * 0.01f;
            sr.material.color = _color;
            yield return new WaitForSeconds(tempTime);
        }
        Destroy(this.gameObject);
    }

    //private IEnumerator Invulnerability()
    //{
    //    isInvulnerability = true;

    //    Color _color = sr.material.color;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        _color.a = 0.5f;
    //        sr.material.color = _color;
    //        yield return new WaitForSeconds(invulnerabilityTime / 4);
    //        _color.a = 1f;
    //        sr.material.color = _color;
    //        yield return new WaitForSeconds(invulnerabilityTime / 4);
    //    };

    //    isInvulnerability = false;
    //}

}