using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTwoStuff : MonoBehaviour
{

    public GameObject enemy;
    public PlayerController pc;
    private float speed = 0.05f;
    private Rigidbody2D rb2d;
    private bool init;
    private Vector2 position;
    private int value;
    private int lifes;
    private double angle;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        init = false;
        GameObject player = GameObject.Find("professor");
        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
        value = 2;
        lifes = 2;
    }

    void Update()
    {
        if (!init)
        {
            position = new Vector2(transform.position.x, transform.position.y);
            position *= speed;
            float distanceOx = 11 - position.x;
            float distanceOy = position.y;
            angle = Math.Atan(distanceOy / distanceOx);
            init = true;
        }
        rb2d.AddForce(position);
        position.x += (float)Math.Cos(angle) * speed;
        position.y -= (float)Math.Sin(angle) / 3;
    }

    public void InitEnemies(int numEnemies, float speedBoost)
    {
        float x = -10.0f;
        float y = -4.0f;
        float step = -2.0f * y / (numEnemies - 1);

        while (y <= 4.05f)
        {
            GameObject e = Instantiate(enemy, new Vector3(x, y, 0), Quaternion.identity);
            EnemyTwoStuff es = e.GetComponent<EnemyTwoStuff>();
            es.speed *= speedBoost;
            y += step;
        }
    }

    public void DestroyEnemies()
    {
        EnemyTwoStuff[] enemies = FindObjectsOfType(typeof(EnemyTwoStuff)) as EnemyTwoStuff[];
        foreach (EnemyTwoStuff enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //collision with the goal net
        if (other is PolygonCollider2D)
        {
            pc.DecreasePlayerScore(value);
            Destroy(gameObject);
        }
        else if (other is CircleCollider2D)
        {
            GameObject obj = other.gameObject;
            BallStuff bs = obj.GetComponent<BallStuff>();
            if (bs != null)
            {
                lifes -= 1;
                if (lifes == 0)
                {
                    pc.IncreasePlayerScore(value);
                    Destroy(gameObject);
                }
            }
        }
    }
}