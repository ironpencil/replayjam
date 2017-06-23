using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBladeBehavior : MonoBehaviour {
    public float WallSpeed = .001f;
    public float MoveSpeed = 1f;
    public float SpinSpeed = 8.0f;
    public float WallOffset = 2.0f;
    public float TriggerRange = 0.5f;
    public SliderJoint2D northSliderPrefab;
    public SliderJoint2D southSliderPrefab;
    public Vector2 Direction = Vector2.zero;
    private WallSide currentWall = WallSide.None;
    private enum WallSide { North, South, East, West, None };

    Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        /*
        // Move along the wall
        // Check for a player
		if (currentWall != WallSide.None)
        {
            gameObject.transform.position += (Vector3)Direction * WallSpeed;
            foreach (PlayerController pc in Globals.Instance.gameObject.GetComponent<GameManager>().players.Values)
            {
                float pcx = pc.transform.position.x;
                float wbx = gameObject.transform.position.x;
                float pcy = pc.transform.position.y;
                float wby = gameObject.transform.position.y;

                if ((wbx + TriggerRange >= pcx && wbx - TriggerRange <= pcx)
                    || (wby + TriggerRange >= pcy && wby - TriggerRange <= pcy))
                {
                    EjectFromWall();
                }
            }
            // Move that direction
        }
        */
	}

    private void FixedUpdate()
    {
        /*
        if (currentWall != WallSide.None)
        {
            rb2d.velocity = rb2d.velocity.normalized * MoveSpeed;
        }
        */
    }

    private void OnCollisionEnter2DBackup(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<WallBehavior>() != null)
        {
            LockOntoWall(collision.collider.gameObject.transform);
        }
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Kill(PlayerController.KillType.None);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EdgeCollider2D ec = collider.GetComponent<EdgeCollider2D>();
        if (ec != null)
        {
            // Get the closest point
            Vector2 closestPoint = Vector2.zero;
            float closestDistance = 0.0f;

            for (int i = 0; i < ec.points.Length; i++)
            {
                Vector2 point = ec.points[i];
                float distance = Vector2.Distance(gameObject.transform.position, point);
                if (closestPoint == Vector2.zero ||
                    distance < closestDistance) {
                    closestPoint = point;
                    closestDistance = distance;
                }
            }
        }
    }

    private void OnTriggerEnter2DBackup(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<WallBehavior>() != null && currentWall != GetWallSide(collider.transform))
        {
            LockOntoWall(collider.gameObject.transform);
        }
    }

    private void EjectFromWall()
    {
        // Begin ignoring physics
        rb2d.isKinematic = false;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;

        // On a wide wall
        switch (currentWall)
        {
            case WallSide.North:
                gameObject.transform.position += new Vector3(0, -1, 0);
                Direction = new Vector2(0, -1);
                break;
            case WallSide.South:
                gameObject.transform.position += new Vector3(0, 1, 0);
                Direction = new Vector2(0, 1);
                break;
            case WallSide.East:
                gameObject.transform.position += new Vector3(-1, 0, 0);
                Direction = new Vector2(-1, 0);
                break;
            case WallSide.West:
                gameObject.transform.position += new Vector3(1, 0, 0);
                Direction = new Vector2(1, 0);
                break;
        }

        currentWall = WallSide.None;
        rb2d.velocity = Direction * MoveSpeed;
    }

    private WallSide GetWallSide(Transform wall)
    {
        WallSide wallSide = WallSide.None;

        if (wall.localScale.x > wall.localScale.y)
            if (wall.position.y > 0)
                wallSide = WallSide.North;
            else
                wallSide = WallSide.South;
        else
            if (wall.position.x > 0)
                    wallSide = WallSide.East;
                else
                    wallSide = WallSide.West;

        return wallSide;
    }

    private void LockOntoWall(Transform targetWall)
    {
        WallSide targetWallSide = GetWallSide(targetWall);
        
        // Begin ignoring physics
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;

        float dir = -1.0f;
        if (currentWall == WallSide.North || currentWall == WallSide.West)
        {
            dir = 1.0f;
        }
        if (targetWallSide == WallSide.North || targetWallSide == WallSide.South)
        {
            Direction = new Vector2(dir, 0);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, targetWall.position.y);
        } else
        {
            Direction = new Vector2(0, dir);
            gameObject.transform.position = new Vector2(targetWall.position.x, gameObject.transform.position.y);
        }

        currentWall = targetWallSide;
    }
}
