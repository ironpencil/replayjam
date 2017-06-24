using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System;

public class PlayerInput : MonoBehaviour {
    private float currentSpeed;
    public int playerNum = 1;
    public Sprite playerSprite;
    
    public GameObject sprite;
    public GameObject baseSprite;

    private HingeJoint2D hinge;

    public float maxThrust;
    
    XboxController xboxController;

    Rigidbody2D rb2d;

    bool playerPaused = false;

    public ThrustStyle thrustStyle;

    public enum ThrustStyle
    {
        trigger, joystick
    }

	// Use this for initialization
	void Start () {
        hinge = GetComponent<HingeJoint2D>();
        rb2d = GetComponent<Rigidbody2D>();
        xboxController = (XboxController)playerNum;
    }
	
	// Update is called once per frame
	void Update () {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleAttack();
        }
    }

    void FixedUpdate()
    {
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleThrust();
        }
    }

    private void HandleAttack()
    {
        if (XCI.GetButton(XboxButton.A))
        {
            Debug.Log("A button pressed");
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
        float y = -XCI.GetAxisRaw(XboxAxis.LeftStickY, xboxController);

        Debug.Log("x: " + x);
        Debug.Log("y: " + y);

        if (Math.Abs(x) > .5 || Math.Abs(y) > .5)
        {
            Vector2 toDir = new Vector2(x, y);
            Vector2 fromDir = (Vector2)(Quaternion.Euler(0, 0, hinge.jointAngle) * Vector2.right);
            float ang = Vector2.Angle(fromDir, toDir);
            Vector3 cross = Vector3.Cross(fromDir, toDir);

            if (cross.z > 0) ang = 360 - ang;
            
            Vector3 thrust = Vector3.zero;

            if (ang < 180)
            {
                thrust = transform.up * maxThrust;
            }
            else
            {
                thrust = -transform.up * maxThrust;
            }

            if (ang > 20)
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
}
