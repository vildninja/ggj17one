using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour {

    [System.Serializable]
    public struct Rope
    {
        public float y;
        public float velocity;
    }

    public bool upsideDown = false;

    public float resonate = 10;
    public float damping = 0.99f;
    public float jump = 10;
    public float limit = 0.5f;

    public Rope[] rope;
    private Vector3[] pos;
    private Vector2[] pos2;

    private LineRenderer line;
    private PolygonCollider2D col;

	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
        pos = new Vector3[rope.Length];
        pos2 = new Vector2[rope.Length + 2];

        Vector2 first = line.GetPosition(0);
        Vector2 last = line.GetPosition(1);

        pos2[rope.Length] = last + new Vector2(0, -10);
        pos2[rope.Length + 1] = first + new Vector2(0, -10);

        for (int i = 0; i < rope.Length; i++)
        {
            pos[i] = Vector3.Lerp(first, last, i / (pos.Length - 1f));
            pos2[i] = pos[i];
        }

        line.numPositions = pos.Length;
        line.SetPositions(pos);

        col = GetComponent<PolygonCollider2D>();
        col.points = pos2;
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 1; i < rope.Length - 1; i++)
        {
            rope[i].velocity += (rope[i - 1].y - rope[i].y) * Time.deltaTime * resonate;
            rope[i].velocity += (rope[i + 1].y - rope[i].y) * Time.deltaTime * resonate;
            rope[i].velocity = Mathf.Clamp(rope[i].velocity * damping, -limit, limit);
        }

        for (int i = 1; i < rope.Length - 1; i++)
        {
            rope[i].y += rope[i].velocity * Time.deltaTime * 10;
            pos[i].y = rope[i].y;
            pos2[i].y = rope[i].y;
        }

        line.SetPositions(pos);
        col.points = pos2;
    }

    public void Hit(Rigidbody2D body, float velY)
    {
        int scale = upsideDown ? -1 : 1;

        velY *= scale;

        float x = body.position.x * scale;
        

        int i = Mathf.RoundToInt(x + 10);
        if (i < 1)
            i = 1;
        if (i > 19)
            i = 19;

        if (velY < 0)
        {
            if (rope[i].velocity < 0)
            {
                rope[i].velocity += velY * 1000;
            }
            else
            {
                rope[i].velocity = velY * 1000;
            }
        }

        if (rope[i].velocity > 0)
        {
            body.velocity += new Vector2(0, rope[i].velocity * 10 * scale);
        }
    }
}
