using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    //player movement speed
	public float speed;
    //keeps track of the current score
    public int score;
    //references to other scene objects classes
    public BallStuff bs;
    public EnemyStuff es;
    //references to text box elements (for displaying various information)
    public Text countText;
    public Text achievementText;
    public Text currentText;
    public Text gameOverText;
    //properties elements of the player
    private Animator animator;
	private Rigidbody2D rb2d;
    //time between enemy waves
    private int timeBetweenWaves = 5; //seconds
    //number of enemies in a wave (differs between levels)
    private int numberEnemies = 5;
    //display time for additional text
    private int textTime = 1; //seconds
    //the time when the last enemy wave was generated
    private DateTime currentTimeEnemies;
    //the time when the last special text was displayed
    private DateTime currentTimeText;
    //level upgrade score details
    private int upgradeLevelScore = 30;
    private int deltaUpgradeScore = 10;
    //keeps track of the current level
    private int level;

    void Start()
    {
        animator = this.GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D> ();
        currentTimeEnemies = DateTime.Now;
        currentTimeText = DateTime.MinValue;
        score = 0;
        level = 1;
        SetCountText();
        SetAchievementText();
        SetGameOverText();
        ShowPoints('N', 0);
    }
 
    void Update()
    {
        MovePlayer();
		
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
            es.InitEnemies(numberEnemies);
            currentTimeEnemies = now;
        }
        ShowPoints('N', 0);
    }

    void MovePlayer()
    {
        //get input data
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
    }

    public void UpgradeLevel()
    {
        //stop the game
        Time.timeScale = 0;

        //make current background invisible
        GameObject stadium = GameObject.Find("Stadium_Level" + level);
        if (stadium != null)
        {
            SpriteRenderer sr = stadium.GetComponent<SpriteRenderer>();

            if (sr)
            {
                sr.sortingLayerName = "Default";
            }
        }

        level++;

        //make next background visible
        stadium = GameObject.Find("Stadium_Level" + level);
        if (stadium != null)
        {
            SpriteRenderer sr = stadium.GetComponent<SpriteRenderer>();

            if (sr)
            {
                sr.sortingLayerName = "Background";
            }
        }

        //make the net larger (more difficult to defend)
        GameObject net = GameObject.Find("GoalNet");
        if (net != null)
        {
            Transform t = net.GetComponent<Transform>();
            
            if (t)
            {
                t.localScale += new Vector3(0.3f, 0.05f, 0);
            }
        }

        es.DestroyEnemies();
        score = 0;
        currentTimeEnemies = DateTime.Now;
        numberEnemies += level;
        upgradeLevelScore += deltaUpgradeScore;
        SetCountText();

        Time.timeScale = 1;
    }

    public void IncreasePlayerScore(int value)
    {
        score += value;
        ShowPoints('W', value);
        SetCountText();
        SetAchievementText();

        //level upgrade
        if (score >= upgradeLevelScore)
        {
            UpgradeLevel();
        }
    }

    public void DecreasePlayerScore(int value)
    {
        score -= value;

        //game over
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
