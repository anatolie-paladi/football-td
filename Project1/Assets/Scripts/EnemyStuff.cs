using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStuff : MonoBehaviour {

    public GameObject enemy;
    public PlayerController pc;
    public float speed = 0.01f;
    private Rigidbody2D rb2d;
    private bool init;
    private Vector2 position;
    private int value;
    System.Random rnd;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        init = false;
        GameObject player = GameObject.Find("professor");
        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
        rnd = new System.Random();
        value = 2;
    }
	
	void Update () {
        if (!init)
        {
            position = new Vector2(transform.position.x, transform.position.y);
            init = true;
        }
        rb2d.AddForce(position);
        int r = rnd.Next(1, 5);
        switch(r)
        {
            case 1:
                position.x += (float)(speed);
                position.y += (float)(speed);
                break;
            case 2:
                position.x -= (float)(speed);
                position.y += (float)(speed);
                break;
            case 3:
                position.x -= (float)(speed);
                position.y -= (float)(speed);
                break;
            default:
                position.x += (float)(speed);
                position.y -= (float)(speed);
                break;
        }
    }

    public void InitEnemy(float x, float y)
    {
        Instantiate(enemy, new Vector3(x, y, 0), Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        pc.IncreasePlayerScore(value);
        Destroy(gameObject);
    }
}
