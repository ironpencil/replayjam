using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System;

public class PlayerInput : MonoBehaviour {
    public int playerPosition = 1;
    public int playerHealth = 3;
    public float maxReticalX;
    public float maxReticalY;
    public Transform retical;
    public float reticalLowMax;
    public float reticalLowMin;
    public float reticalHighMax;
    public float reticalHighMin;

    public float shieldAlpha = 0.35f;

    public SpriteRenderer aura;
    public GameObject playerRing;

    public ShootProjectiles gun;

    public HingeJoint2D hinge;
    private Vector2 rinput;

    public float maxThrust;
    
    XboxController xboxController;

    Rigidbody2D rb2d;

    bool playerPaused = false;
    public bool kill = false;

    public ThrustStyle thrustStyle;
    public PlayerInfo playerInfo;

    public enum ThrustStyle
    {
        trigger, joystick
    }

	// Use this for initialization
	void Start () {
        hinge = GetComponent<HingeJoint2D>();
        rb2d = GetComponent<Rigidbody2D>();
        xboxController = (XboxController)1;// playerNum;

        //JointAngleLimits2D limits = new JointAngleLimits2D();
        //limits.min = 180 * (playerNum - 1);
        //limits.max = 180 * playerNum;
        //hinge.limits = limits;
        //hinge.useLimits = true;
        int numPlayers = Globals.Instance.GameManager.numPlayers;

        float degreesPerPlayer = Mathf.Round(360.0f / numPlayers);
        float startAngle = degreesPerPlayer * -1 * playerPosition;

        float offset = 0.0f;
        if (numPlayers == 3)
        {
            offset = 90.0f;
        }
        else if (numPlayers == 4)
        {
            offset = 135.0f;
        }

        startAngle -= offset;

        Vector3 startRotation = new Vector3(0.0f, 0.0f, startAngle);

        transform.Rotate(startRotation);

        float angleLimit = Mathf.Round(degreesPerPlayer * 0.5f);

        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = angleLimit * -1;
        limits.max = angleLimit;
        hinge.limits = limits;

        hinge.enabled = true;

        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(playerInfo.playerNum);
        playerColor.a = shieldAlpha;

        aura.color = playerColor;


        //set up player ring
        ParticleSystem ps = playerRing.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = playerColor;
        var shape = ps.shape;
        shape.arc = degreesPerPlayer;

        startRotation.z -= angleLimit;
        playerRing.transform.Rotate(startRotation);
    }
	
	// Update is called once per frame
	void Update () {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleAttack();
        }

        if (kill)
        {
            Kill();
        }
    }

    void FixedUpdate()
    {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleAiming();
            HandleThrust();
        }

        CheckHingeAlign();
    }

    private void CheckHingeAlign()
    {
        if (hinge.jointAngle < hinge.limits.min)
        {
            rb2d.AddForce(-transform.right * maxThrust * 0.5f, ForceMode2D.Force);
        }
        else if (hinge.jointAngle > hinge.limits.max)
        {
            rb2d.AddForce(transform.right * maxThrust * 0.5f, ForceMode2D.Force);
        }
    }

    private void HandleAttack()
    {
        if (XCI.GetButton(XboxButton.RightBumper))
        {
            gun.Shoot();
        }
    }

    void HandleAiming()
    {
        float x = XCI.GetAxisRaw(XboxAxis.RightStickX, xboxController);
        float y = XCI.GetAxisRaw(XboxAxis.RightStickY, xboxController);
        
        rinput = new Vector2(x, y);

        if (rinput.magnitude > 0)
        {
            // Original Rotation Setup
            var angle = Mathf.Atan2(rinput.y, rinput.x) * Mathf.Rad2Deg -90;

            retical.eulerAngles = new Vector3(0, 0, angle);
            float max = 0.0f;
            float min = 0.0f;
            if (retical.localEulerAngles.z > 180)
            {
                max = reticalHighMax;
                min = reticalHighMin;
            } else
            {
                max = reticalLowMax;
                min = reticalLowMin;
            }
            
            Debug.Log("General Angle: " + angle);
            Debug.Log("Local Angle: " + retical.localEulerAngles.z);

            if (retical.localEulerAngles.z > max || retical.localEulerAngles.z < min)
            {
                retical.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(retical.localEulerAngles.z, min, max));
            }

            /*
            if (x > 0)
            {
                retical.localEulerAngles = new Vector3(0, 0, (reticalHighMax - (reticalHighMax - reticalHighMin) * Math.Abs(x)));
            }
            else
            {
                retical.localEulerAngles = new Vector3(0, 0, (reticalLowMin + (reticalLowMax - reticalLowMin) * Math.Abs(x)));
            }
            */

        }
    }

    void HandleThrust()
    {
        switch(thrustStyle)
        {
            case ThrustStyle.trigger:
                ApplyTriggerThrust();
                break;
            case ThrustStyle.joystick:
                ApplyJoystickThrust();
                break;
        }
        

        //float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        //if (sprite != null) sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void ApplyTriggerThrust()
    {
        float rightThrust = XCI.GetAxisRaw(XboxAxis.RightTrigger, xboxController);
        float leftThrust = XCI.GetAxisRaw(XboxAxis.LeftTrigger, xboxController);

        Vector3 thrust = (transform.up * rightThrust * maxThrust) + (-transform.up * leftThrust * maxThrust);

        Debug.Log("Thrust: " + thrust);

        rb2d.AddForce(thrust, ForceMode2D.Force);
    }

    void ApplyJoystickThrust()
    {
        float x = XCI.GetAxisRaw(XboxAxis.LeftStickX, xboxController);
        float y = XCI.GetAxisRaw(XboxAxis.LeftStickY, xboxController);


        if (Math.Abs(x) > .5 || Math.Abs(y) > .5)
        {
            Vector2 toDir = new Vector2(x, y).normalized;
            //Vector2 fromDir = (Vector2)(Quaternion.Euler(0, 0, hinge.jointAngle) * Vector2.up);
            Vector2 fromDir = transform.localPosition.normalized;
            
            float ang = Vector2.Angle(fromDir, toDir);

            

            Vector3 cross = Vector3.Cross(fromDir, toDir);

            if (cross.z < 0) ang = ang * -1;

            //Debug.Log(fromDir + " " + toDir + " " + ang);

            Vector3 thrust = Vector3.zero;

            if (ang > 0)
            {
                thrust = transform.right * maxThrust;
            }
            else
            {
                thrust = -transform.right * maxThrust;
            }

            if (Mathf.Abs(ang) > 10)
            {
                rb2d.AddForce(thrust, ForceMode2D.Force);
            }
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

    public bool Hit()
    {
        playerHealth--;

        bool killed = false;

        if (playerHealth < 1)
        {
            Kill();
            killed = true;
        }
        
        return killed;
    }

    public void Kill()
    {
        Destroy(gameObject);
        ParticleSystem ps = playerRing.GetComponent<ParticleSystem>();
        ps.Stop();
        Destroy(playerRing, 5.0f);

        Globals.Instance.GameManager.KillPlayer(playerInfo.playerNum);
    }

    public void AdjustHingeLimits(float min, float max)
    {
        JointAngleLimits2D limits = hinge.limits;
        limits.min += min;
        limits.max += max;

        hinge.limits = limits;

        playerRing.transform.Rotate(new Vector3(0, 0, max * -1));
        ParticleSystem ps = playerRing.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.arc = limits.max - limits.min;
    }
}
