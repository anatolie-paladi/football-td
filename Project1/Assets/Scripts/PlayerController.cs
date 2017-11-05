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
    private int textTime = 1; //seconds
    private DateTime currentTimeEnemies;
    private DateTime currentTimeText;
    public int score;
    public Text countText;
    public Text achievementText;
    public Text currentText;
    public Text gameOverText;

    void Start()
    {
        animator = this.GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D> ();
        currentTimeEnemies = DateTime.Now;
        currentTimeText = DateTime.MinValue;
        score = 0;
        SetCountText();
        SetAchievementText();
        ShowPoints('N', 0);
        SetGameOverText();
    }
 
    void Update()
    {
        //player movement section
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
		
        //player shooting section
		if (Input.GetMouseButtonDown(0))
		{
			bs.doIt(transform.position.x, transform.position.y);
		}

        //enemy generation section
        DateTime now = DateTime.Now;
        var diff = (now - currentTimeEnemies).TotalSeconds;
        if (diff >= timeBetweenWaves)
        {
            es.InitEnemy();
            currentTimeEnemies = now;
            //timeBetweenWaves *= 20;
        }
        ShowPoints('N', 0);
    }

    public void IncreasePlayerScore(int value)
    {
        score += value;
        ShowPoints('W', value);
        SetCountText();
        SetAchievementText();
    }

    public void DecreasePlayerScore(int value)
    {
        score -= value;
        if (score < 0)
        {
            Time.timeScale = 0;
            SetGameOverText();
        }
        ShowPoints('L', value);
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Score: " + score;
    }

    void ShowPoints(char type, int value)
    {
        if (type == 'W')
        {
            currentText.text = "+" + value + "Points";
            currentTimeText = DateTime.Now;
        }
        else if (type == 'L')
        {
            currentText.text = "-" + value + "Points";
            currentTimeText = DateTime.Now;
        }
        else
        {
            DateTime now = DateTime.Now;
            var diff = (now - currentTimeText).TotalSeconds;
            if (diff > textTime)
            {
                currentText.text = "";
            }
        }
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

    void SetGameOverText()
    {
        if (score < 0)
        {
            gameOverText.text = "Game Over!";
        }
        else
        {
            gameOverText.text = "";
        }
    }
}
