using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


    public float jump = 500;
    public float move = 30;

    public float grounded = 0;

    public Enemy[] enemies;

    Rigidbody2D body;

    private Vector2 vel;

    public Text timeText;
    public Text timeShadowText;
    private float timer;

    public GameObject paw;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {

		if (Input.GetButtonDown("Jump"))
        {
            body.AddForce(Vector2.up * jump * grounded);
        }


        grounded -= Time.deltaTime * 3;
        if (grounded < 0)
            grounded = 0;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            timer = 0;

            foreach (var tramp in FindObjectsOfType<Trampolin>())
            {
                tramp.damping = 1;
            }
        }

        if (Time.timeScale > 0.5f)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("00.0");
            timeShadowText.text = timer.ToString("00.0");
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (i < (timer) / 2)
            {
                if (!enemies[i].gameObject.activeSelf)
                {
                    if (Vector2.Distance(transform.position, enemies[i].transform.position) > 4 + (i - timer / 2))
                    {
                        enemies[i].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (enemies[i].gameObject.activeSelf)
                {
                    enemies[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        vel = body.velocity;
        float right = Input.GetAxis("Horizontal") * move * (0.5f + grounded * 0.5f);
        if (vel.x > 0 == right < 0)
        {
            right *= 3;
        }
        body.AddForce(Vector2.right * right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tram = collision.gameObject.GetComponent<Trampolin>();
        if (tram != null)
        {
            tram.Hit(body, vel.y);
            //float x = transform.position.x;

            //int i = Mathf.RoundToInt(x + 10);
            //if (i < 1)
            //    i = 1;
            //if (i > 19)
            //    i = 19;

            //if (velY < 0)
            //{
            //    tram.rope[i].velocity += velY * 100;
            //}

            //if (tram.rope[i].velocity > 0)
            //{
            //    body.velocity += new Vector2(0, tram.rope[i].velocity * 10);
            //}
        }

        if (collision.contacts[0].point.y < transform.position.y)
        {
            grounded = 1;
        }

        if (collision.gameObject.GetComponent<Enemy>() && Time.timeScale > 0.5f)
        {
            Time.timeScale = 0.2f;

            foreach (var tramp in FindObjectsOfType<Trampolin>())
            {
                tramp.damping = 0.9f;
            }

            Instantiate(paw, collision.contacts[0].point, paw.transform.rotation);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            grounded = 1;
        }
    }
}
