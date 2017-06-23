using UnityEngine;
using System.Collections;

public class MoveTowardsTarget : BaseMovement {

    public Transform target;    

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
    }

    public override void Move()
    {
        if (disabled || target == null) {
            return;
        }

        movementDirection = (target.position - transform.position).normalized;        
    }
}
