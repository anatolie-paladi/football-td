using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
 
	public float speed;
	public BallStuff bs;
    public EnemyStuff es;
	private Animator animator;
	private Rigidbody2D rb2d;
    private int timeBetweenWaves = 5; //seconds
    private DateTime currentTime;
    public int score;
    public Text countText;
    public Text achievementText;

    void Start()
    {
        animator = this.GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D> ();
        currentTime = DateTime.Now;
        score = 0;
        SetCountText();
        SetAchievementText();
    }
 
    void Update()
    {
 
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
 
        if (vertical > 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (vertical < 0)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (horizontal > 0)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (horizontal < 0)
        {
            animator.SetInteger("Direction", 1);
        }
		
		Vector2 movement = new Vector2(horizontal, vertical);
		rb2d.AddForce(movement * speed);
		
		if (Input.GetMouseButtonDown(0))
		{
			bs.doIt(transform.position.x, transform.position.y);
		}

        DateTime now = DateTime.Now;
        var diff = (now - currentTime).TotalSeconds;
        if (diff >= timeBetweenWaves)
        {
            es.InitEnemy(0, 0);
            currentTime = now;
        }
    }

    public void IncreasePlayerScore(int value)
    {
        score += value;
        SetCountText();
        SetAchievementText();
    }

    void SetCountText()
    {
        countText.text = "Score: " + score;
    }

    void SetAchievementText()
    {
        if ((score % 20 == 0) && (score > 0))
        {
            achievementText.text = score + " points. On fire!";
        }
        else
        {
            achievementText.text = "";
        }
    }
}
