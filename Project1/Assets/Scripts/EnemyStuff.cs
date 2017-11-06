using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStuff : MonoBehaviour {

    public GameObject enemy;
    public PlayerController pc;
    private float speed = 0.05f;
    private Rigidbody2D rb2d;
    private bool init;
    private Vector2 position;
    private int value;
    private double angle;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        init = false;
        GameObject player = GameObject.Find("professor");
        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
        value = 2;
    }
	
	void Update () {
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

    public void InitEnemies(int numEnemies)
    {
        float x = -10.0f;
        float y = -4.0f;
        float step = -2.0f * y / (numEnemies - 1);

        while (y <= 4.05f)
        {
            Instantiate(enemy, new Vector3(x, y, 0), Quaternion.identity);
            y += step;
        }
    }

    public void DestroyEnemies()
    {
        EnemyStuff[] enemies = FindObjectsOfType(typeof(EnemyStuff)) as EnemyStuff[];
        foreach (EnemyStuff enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            pc.DecreasePlayerScore(value);
        }
        else if (other is CircleCollider2D)
        {
            pc.IncreasePlayerScore(value);
        }
        Destroy(gameObject);
    }
}
