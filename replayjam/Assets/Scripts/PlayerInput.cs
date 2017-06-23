using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System;

public class PlayerInput : MonoBehaviour {
    private GameOptions go;

    private float currentSpeed;
    public int PlayerNum = 1;
    public bool UseForces = false;
    public Sprite PlayerSprite;

    //Dashing vars
    public Sprite DashingSprite;
    public bool IsDashing = false;
    public GameObject dashTrail;
    private float DashRechargeTimeLeft = 0.0f;
    private float DashTimeLeft = 0.0f;
    private float LastHorizontal = 0.0f;
    private float LastVertical = 0.0f;
    
    public GameObject hand;
    public GameObject sprite;
    public GameObject baseSprite;
    public GameObject shield;

    public SoundEffectHandler dashSound;
    
    public bool mouseInput = false;
    private bool dashPressed = false;
    private bool dashReleased = false;

    XboxController xboxController;

    Rigidbody2D rb2d;

    bool playerPaused = false;

	// Use this for initialization
	void Start () {
        go = Globals.Instance.GameManager.gameOptions;
        
        rb2d = GetComponent<Rigidbody2D>();
        currentSpeed = go.playerMoveSpeed;
        dashTrail.GetComponent<ParticleSystem>().Stop(false);
        xboxController = (XboxController)PlayerNum;
    }
	
	// Update is called once per frame
	void Update () {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleShieldMovement();
            HandleAttack();
            CheckDash();
        }
        HandlePause();
    }

    private void CheckDash()
    {
        if (XCI.GetButtonDown(XboxButton.A, xboxController)
            || XCI.GetButtonDown(XboxButton.RightBumper, xboxController)
            || XCI.GetButtonDown(XboxButton.B, xboxController))
        {
            dashPressed = true;
        }

        if (!XCI.GetButton(XboxButton.A, xboxController)
            && !XCI.GetButton(XboxButton.B, xboxController)
            && !XCI.GetButton(XboxButton.RightBumper, xboxController))
        {
            dashReleased = true;
        }
    }

    void FixedUpdate()
    {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleDash();
            HandleMovement();
        }
    }

    private void HandleAttack()
    {
        if (XCI.GetButtonDown(XboxButton.B))
        {
            Debug.Log("B button pressed");
        }
    }

    private void HandlePause()
    {
        if (XCI.GetButtonDown(XboxButton.Start, xboxController))
        {
            Debug.Log("Start pressed");

            //track which player paused the game - so only that player can unpause/return to menu
            if (Globals.Instance.paused)
            {
                if (playerPaused)
                {
                    Globals.Instance.Pause(false);
                    playerPaused = false;
                }
            } else
            {
                Globals.Instance.Pause(true);
                playerPaused = true;
            }
        }

        if (XCI.GetButtonDown(XboxButton.Back, xboxController) && playerPaused)
        {
            //return to player select screen
            Globals.Instance.Pause(false);
            Globals.Instance.GameManager.SetupGame();
        }
    }

    void HandleDash()
    {
        DashRechargeTimeLeft -= Time.fixedDeltaTime;

//        Debug.Log("A? " + XCI.GetButtonDown(XboxButton.A, xboxController));
  //      Debug.Log("B? " + XCI.GetButtonDown(XboxButton.B, xboxController));
    //    Debug.Log("RB? " + XCI.GetButtonDown(XboxButton.RightBumper, xboxController));

        if (dashPressed)
        {
            if (DashRechargeTimeLeft <= 0 && !IsDashing)
            {
                
                BeginDash();
            }
            else
            {
                // TODO Alert the player that they can't dash yet
                //Debug.Log("Can't dash, let player know here");
            }
        }
        else if (IsDashing && dashReleased)
        {
            dashReleased = false;
            EndDash();
        }

        dashPressed = false;
        dashReleased = false;
    }

    private Vector3 beginDashPos;
    void BeginDash()
    {
        beginDashPos = gameObject.transform.position;
        IsDashing = true;
        DashTimeLeft = go.playerDashTime;
        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();
        sr.sprite = DashingSprite;
        baseSprite.GetComponent<SpriteRenderer>().enabled = false;
        currentSpeed = go.playerDashSpeed;
        gameObject.GetComponent<BoundsChecker>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Dashing Player");
        dashSound.PlayEffect();
        dashTrail.GetComponent<ParticleSystem>().Play();
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg + 90;
        Vector3 rotation = new Vector3(0, 0, angle);
        dashTrail.transform.eulerAngles = rotation;
    }

    void EndDash()
    {
        IsDashing = false;
        DashTimeLeft = 0.0f;
        DashRechargeTimeLeft = go.playerDashRechargeTime;
        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();
        sr.sprite = PlayerSprite;
        baseSprite.GetComponent<SpriteRenderer>().enabled = true;
        currentSpeed = go.playerMoveSpeed;
        gameObject.GetComponent<BoundsChecker>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
        dashTrail.GetComponent<ParticleSystem>().Stop(false);
    }

    void HandleMovement()
    {
        if (IsDashing)
        {
            DashTimeLeft -= Time.fixedDeltaTime;
            if (DashTimeLeft <= 0)
            {
                EndDash();
            }
        }

        float horizontal = 0.0f;
        float vertical = 0.0f;

        if (go.playerLockDashDirection && IsDashing)
        {
            horizontal = LastHorizontal;
            vertical = LastVertical;
        }
        else
        {
            horizontal = XCI.GetAxisRaw(XboxAxis.LeftStickX, xboxController);
            vertical = XCI.GetAxisRaw(XboxAxis.LeftStickY, xboxController);
            LastHorizontal = horizontal;
            LastVertical = vertical;
        }

        Vector2 moveDirection = new Vector2(horizontal, vertical);

        if (IsDashing)
        {
            moveDirection.Normalize(); //always dash with full speed
        }

        Vector2 moveForce = moveDirection * currentSpeed;

        if (UseForces)
        {
            rb2d.AddForce(moveForce, ForceMode2D.Force);
        }
        else
        {
            rb2d.velocity = moveForce;
        }

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        if (sprite != null) sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void HandleShieldMovement()
    {
        if (!IsDashing)
        {
            Vector2 rinput;
            if (mouseInput)
            {
                rinput = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rinput.Normalize();
            }
            else
            {
                // We are going to read the input every frame
                //rinput = new Vector2(Input.GetAxisRaw("RHorizontal" + PlayerNum), Input.GetAxisRaw("RVertical" + PlayerNum));
                rinput = new Vector2(XCI.GetAxisRaw(XboxAxis.RightStickX, xboxController), XCI.GetAxisRaw(XboxAxis.RightStickY, xboxController));
            }

            if (rinput.magnitude > 0)
            {
                // Apply the transform to the object  
                var angle = Mathf.Atan2(rinput.y, rinput.x) * Mathf.Rad2Deg;
                hand.transform.eulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
}
