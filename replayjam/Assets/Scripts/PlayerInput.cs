using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System;

public class PlayerInput : MonoBehaviour {
    public int playerNum = 1;
    public float maxReticalX;
    public float maxReticalY;
    public Transform retical;
    public float reticalLowMax;
    public float reticalLowMin;
    public float reticalHighMax;
    public float reticalHighMin;

    private HingeJoint2D hinge;
    private Vector2 rinput;

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
            HandleAiming();
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
        float y = -XCI.GetAxisRaw(XboxAxis.LeftStickY, xboxController);

        if (Math.Abs(x) > .5 || Math.Abs(y) > .5)
        {
            Vector2 toDir = new Vector2(x, y);
            Vector2 fromDir = (Vector2)(Quaternion.Euler(0, 0, hinge.jointAngle) * Vector2.up);
            
            float ang = Vector2.Angle(fromDir, toDir);

            Vector3 cross = Vector3.Cross(fromDir, toDir);

            if (cross.z > 0) ang = 360 - ang;
            
            Vector3 thrust = Vector3.zero;

            if (ang < 180)
            {
                thrust = transform.right * maxThrust;
            }
            else
            {
                thrust = -transform.right * maxThrust;
            }

            if (ang > 10)
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
