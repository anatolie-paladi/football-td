using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyThreeStuff : MonoBehaviour
{

    public GameObject enemy;
    public PlayerController pc;
    public EnemyBallStuff ebs;
    private float speed = 0.03f;
    private Rigidbody2D rb2d;
    private bool init;
    private Vector2 position;
    private int value;
    private int lifes;
    private double angle;
    private int timeBetweenBalls = 2; //seconds
    private DateTime currentTimeBall;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        init = false;
        GameObject player = GameObject.Find("professor");
        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
        value = 3;
        lifes = 1;
        currentTimeBall = DateTime.Now;
    }

    void Update()
    {
        if (!init)
        {
            position = new Vector2(transform.position.x, transform.position.y);
            position *= speed;
            float distanceOx = 11.0f - position.x;
            float distanceOy = position.y;
            angle = Math.Atan(distanceOy / distanceOx);
            init = true;
        }
        rb2d.AddForce(position);
        position.x += (float)Math.Cos(angle) * speed;
        position.y -= (float)Math.Sin(angle) / 3;

        DateTime now = DateTime.Now;
        var diff = (now - currentTimeBall).TotalSeconds;
        if (diff >= timeBetweenBalls)
        {
            ebs.shoot(transform.position.x, transform.position.y, value);
            currentTimeBall = now;
        }
    }

    public void InitEnemies(int numEnemies, float speedBoost)
    {
        float x = -10.0f;
        float y = -4.0f;
        float step = -2.0f * y / (numEnemies - 1);

        while (y <= 4.05f)
        {
            GameObject e = Instantiate(enemy, new Vector3(x, y, 0), Quaternion.identity);
            EnemyThreeStuff es = e.GetComponent<EnemyThreeStuff>();
            es.speed *= speedBoost;
            y += step;
        }
    }

    public void DestroyEnemies()
    {
        EnemyThreeStuff[] enemies = FindObjectsOfType(typeof(EnemyThreeStuff)) as EnemyThreeStuff[];
        foreach (EnemyThreeStuff enemy in enemies)
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
        //collision with a ball
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
