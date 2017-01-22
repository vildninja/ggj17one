using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public float jump = 500;
    public float move = 30;

    public float grounded = 0;

    Rigidbody2D body;

    private float velY;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        velY = body.velocity.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tram = collision.gameObject.GetComponent<Trampolin>();
        if (tram != null)
        {
            tram.Hit(body, velY);
            //float x = transform.position.x;

            //int i = Mathf.RoundToInt(x + 10);
            //if (i < 1)
            //    i = 1;
            //if (i > 19)
            //    i = 19;

            //if (velY < 0)
            //{
            //    tram.rope[i].velocity += velY;
            //}

            //if (tram.rope[i].velocity > 0)
            //{
            //    body.velocity += new Vector2(0, tram.rope[i].velocity * 10);
            //}
        }

        grounded = 1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = 1;
    }
}
