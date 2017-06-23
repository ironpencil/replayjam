using UnityEngine;
using System.Collections;

public class MoveRandomly : BaseMovement {

    public float minMoveTime = 1.0f;
    public float maxMoveTime = 2.0f;

    public float minMoveDelay = 1.0f;
    public float maxMoveDelay = 2.0f;

    private float moveStartTime = 0.0f; 
    private float moveEndTime = 0.0f;

    public float moveXChance = 0.5f;
    public float moveYChance = 0.5f;

    public float moveXMin = 0.5f;
    public float moveYMin = 0.5f;

    public bool debugMovement = false;
    private bool doDebugVelocity = false;

    private bool isMoving = false;    

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        if (disabled) { return; }

        if (moveDuringUpdate)
        {
            Move();
        }
        
	
	}    

    void FixedUpdate()
    {
        if (disabled) { return; }
        Vector2 velocity = movementDirection * speed;
        if (rb.velocity != velocity)
        {
            rb.velocity = velocity;
        }

        if (doDebugVelocity)
        {
            Debug.Log("Velocity = " + velocity);
            doDebugVelocity = false;
        }
        //rb.AddRelativeForce(velocity);

        //if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        //{
        //    rb.velocity = rb.velocity.normalized * maxSpeed;
        //}

    }

    public override void Move()
    {
        if (disabled) { return; }

        if (isMoving && Time.time > moveEndTime)
        {
            //we are moving, and we should stop
            StopMoving();
        }
        else if (!isMoving && Time.time > moveStartTime)
        {
            //we are not moving, and we should start
            StartMoving();
        }
    }

    private void StartMoving()
    {
        movementDirection = new Vector2(0.0f, 0.0f);

        if (debugMovement)
        {
            movementDirection.x = 1.0f;
            if (Random.Range(0, 2) > 0)
            {
                movementDirection.x = -1.0f;
            }
        }
        else
        {

            bool moveX = false;
            bool moveY = false;

            moveX = Random.Range(0.0f, 1.0f) < moveXChance;
            moveY = Random.Range(0.0f, 1.0f) < moveYChance;

            if (moveX)
            {
                float movementRange = Random.Range(moveXMin, 1.0f);

                //flip sign 50% of the time
                if (Random.Range(0, 2) > 0)
                {
                    movementRange *= -1;
                }
                movementDirection.x = movementRange;
            }

            if (moveY)
            {
                float movementRange = Random.Range(moveYMin, 1.0f);

                //flip sign 50% of the time
                if (Random.Range(0, 2) > 0)
                {
                    movementRange *= -1;
                }
                movementDirection.y = movementRange;
            }
        }

        if (debugMovement)
        {
            Debug.Log("Started moving: " + movementDirection.ToString());
            doDebugVelocity = true;
        }
        //movementDirection.x = Random.Range(-1, 2);
        //movementDirection.y = Random.Range(-1, 2);
        //movementDirection.x = Mathf.Clamp(Random.Range(-2.0f, 2.0f), -1.0f, 1.0f);
        //movementDirection.y = Mathf.Clamp(Random.Range(-2.0f, 2.0f), -1.0f, 1.0f);
        isMoving = true;
        moveEndTime = Time.time + Random.Range(minMoveTime, maxMoveTime);
    }

    private void StopMoving()
    {
        movementDirection = Vector2.zero;
        isMoving = false;
        moveStartTime = Time.time + Random.Range(minMoveDelay, maxMoveDelay);
    }


}
