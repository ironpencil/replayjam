using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour {
    private float MoveSpeed = 8.0f;
    public float SpinSpeed = 8.0f;
    public float PushForce = 10.0f;
    public float SpinDirection = 1;
    public float minPushPercent = 0.5f;
    public float timeForKillCredit = 1.5f;
    private List<PlayerCollision> colliders;
    private Collider2D lastCollider;
    private Dictionary<Rigidbody2D, Vector2> pushes = new Dictionary<Rigidbody2D, Vector2>();

    private class PlayerCollision {
        public float time;
        public int playerNum;

        public PlayerCollision(float time, int playerNum)
        {
            this.playerNum = playerNum;
            this.time = time;
        }
    }

    Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        MoveSpeed = Globals.Instance.GameManager.gameOptions.spinnerSpeed;
        colliders = new List<PlayerCollision>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(Random.Range(-1.0f, 1.0f) * MoveSpeed, Random.Range(-1.0f, 1.0f) * MoveSpeed);
    }
	
	void FixedUpdate () {
        HandleMovement();

        foreach (Rigidbody2D rb in pushes.Keys)
        {
            if (rb != null)
            {
                rb.AddForce(pushes[rb], ForceMode2D.Impulse);
            }
        }

        pushes.Clear();
    }

    void HandleMovement()
    {
        Vector2 direction = rb2d.velocity.normalized;
        rb2d.velocity = direction * MoveSpeed;
        if (Mathf.Abs(SpinSpeed) > 0.0f)
        {
            rb2d.angularVelocity = SpinSpeed * SpinDirection;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            int victimPlayerNum = pc.GetComponent<PlayerInput>().PlayerNum;
            PlayerCollision lastCollider = GetLastCollider(victimPlayerNum);
            Debug.Log("Kill Credit Time: " + (Time.time - timeForKillCredit));
            if (lastCollider != null) Debug.Log("Last Collider Time: " + lastCollider.time);
            if (lastCollider != null && lastCollider.time > Time.time - timeForKillCredit)
            {
                Globals.Instance.GameManager.AddKill(lastCollider.playerNum, victimPlayerNum, Kill.Weapon.Spinner);
            }
            pc.Kill(PlayerController.KillType.Dissolve);
        } else if (collision.collider.transform.name.Equals("Shield"))
        {
            // Cache push direction so it happens in the next frame, that way
            // forces are applied to the spinner naturally
            // TODO Add li'l ass smacker cheevo for hitting him from behind
            Rigidbody2D playerRigidBody = collision.collider.transform.parent.parent.gameObject.GetComponent<Rigidbody2D>();
            Vector2 pushDirection = playerRigidBody.transform.position - transform.position;
            float pushModifier = Mathf.Max(Vector2.Dot(pushDirection.normalized, rb2d.velocity.normalized), 0);
            pushModifier = minPushPercent + (pushModifier * (1 - minPushPercent));
            pushes[playerRigidBody] = pushDirection.normalized * PushForce * pushModifier;
            colliders.Add(new PlayerCollision(Time.time, playerRigidBody.gameObject.GetComponent<PlayerInput>().PlayerNum));
        }
    }

    private PlayerCollision GetLastCollider(int playerNum)
    {
        PlayerCollision lastCollider = null;

        for (int i = colliders.Count - 1; i >= 0; i--)
        {
            lastCollider = colliders[i];

            if (colliders[i].playerNum != playerNum) break;
        }

        return lastCollider;
    }
}
