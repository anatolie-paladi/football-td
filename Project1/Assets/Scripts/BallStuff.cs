using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallStuff : MonoBehaviour {

	public GameObject ball;
	public double speed = 2.0;
	private Vector2 position;
	private bool init;
	private Rigidbody2D rb2d;
	private double angle;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		ComputeAngle();
		init = false;
	}
	
	void Update()
	{
		if (!init)
		{
            if ((angle >= 0) && (angle <= Math.PI / 2))
			    position = new Vector2 (transform.position.x, transform.position.y);
            else if ((angle >= Math.PI / 2) && (angle <= Math.PI))
                position = new Vector2(transform.position.x, transform.position.y);
            else  if ((angle >= Math.PI) && (angle <= 3 * Math.PI / 2))
                position = new Vector2(transform.position.x, transform.position.y);
            else
                position = new Vector2(transform.position.x, transform.position.y);
            init = true;
		}
		rb2d.AddForce(position);
		position.x += (float) (speed * Math.Cos(angle));
		position.y += (float) (speed * Math.Sin(angle));
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }

    public void doIt(float x, float y)
	{
		Instantiate(ball, new Vector3 (x, y, 0), Quaternion.identity);
	}
	
	void ComputeAngle()
	{
		Vector3 mouseCoord;
			
		mouseCoord = GetMouseCoordinates();
        if ((mouseCoord.x >= transform.position.x) && (mouseCoord.y >= transform.position.y))
        {
            if (mouseCoord.x == transform.position.x)
                angle = Math.PI / 2;
            else
                angle = Math.Atan((mouseCoord.y - transform.position.y) / (mouseCoord.x - transform.position.x));
            //Debug.Log("Cadranul I");
        }
        else if ((mouseCoord.x <= transform.position.x) && (mouseCoord.y >= transform.position.y))
        {
            if (mouseCoord.x == transform.position.x)
                angle = Math.PI / 2;
            else
                angle = Math.PI - Math.Atan((transform.position.y - mouseCoord.y) / (mouseCoord.x - transform.position.x));
            //Debug.Log("Cadranul II");
        }
        else if ((mouseCoord.x <= transform.position.x) && (mouseCoord.y <= transform.position.y))
        {
            if (mouseCoord.x == transform.position.x)
                angle = -Math.PI / 2;
            else
                angle = Math.PI + Math.Atan((transform.position.y - mouseCoord.y) / (transform.position.x - mouseCoord.x));
            //Debug.Log("Cadranul III");
        }
        else
        {
            if (mouseCoord.x == transform.position.x)
                angle = -Math.PI / 2;
            else
                angle = Math.Atan((mouseCoord.y - transform.position.y) / (mouseCoord.x - transform.position.x));
            //Debug.Log("Cadranul IV");
        }
	}
	
	Vector3 GetMouseCoordinates()
	{
		Vector3 p = new Vector3();
        
		p = Input.mousePosition;
		p.z = 10.0f;
        p = Camera.main.ScreenToWorldPoint(p);
		
		return p;
	}
}
