using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Text timer;

    //временная мера
    [SerializeField] public float lifeTime = 10f;

    private float gameTime;
    private int tempTime;

    public static TimeManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime == 0)
        {
            timer.text = "Время вышло!";
            Hero.Instance.isDead = true;
            Hero.Instance.healthPoints = 0;
            return;
        }

        timer.text = lifeTime.ToString();
        gameTime += 1 * Time.deltaTime;

        if (gameTime >= 1)
        {
            lifeTime -= 1;
            gameTime = 0;
        }

    }

    public void AddTime(int count)
    {
        lifeTime += count;
    }

}
