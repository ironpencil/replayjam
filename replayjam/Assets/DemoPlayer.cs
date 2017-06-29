using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour {


    public Transform retical;

    public float reticalLowMax;
    public float reticalLowMin;
    public float reticalHighMax;
    public float reticalHighMin;
    public ParticleSystem shield;

    public Vector2 moveInput;
    public Vector2 aimInput;

    //public float shieldAlpha = 0.35f;

    public SpriteRenderer playerShip;

    public ShootProjectiles gun;

    public HingeJoint2D hinge;

    public float maxThrust;

    Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        rb2d = GetComponent<Rigidbody2D>();

        hinge.enabled = true;
        //JointAngleLimits2D limits = new JointAngleLimits2D();
        //limits.min = 180 * (playerNum - 1);
        //limits.max = 180 * playerNum;
        //hinge.limits = limits;
        //hinge.useLimits = true;
        
        if (!Globals.Instance.GameManager.enableShields)
        {
            shield.gameObject.SetActive(false);
        }

    }

    void OnEnable()
    {
        //reset position etc.
        transform.localEulerAngles = Vector3.zero;


    }

    // Update is called once per frame
    void Update()
    {
        HandleAiming();
    }

    void FixedUpdate()
    {
        HandleThrust();

        //CheckHingeAlign();
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


    void HandleAiming()
    {
        if (aimInput.magnitude > 0)
        {
            // Original Rotation Setup
            var angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg - 90;

            //angle *= (2 / 3);

            retical.eulerAngles = new Vector3(0, 0, angle);

            float adjustedAngle;

            float max = 0.0f;
            float min = 0.0f;
            if (retical.localEulerAngles.z > 180)
            {
                //smoothing
                float angleOffset = 360 - retical.localEulerAngles.z;
                adjustedAngle = 360 - (angleOffset * 0.85f);

                max = reticalHighMax;
                min = reticalHighMin;
            }
            else
            {
                //smoothing
                adjustedAngle = retical.localEulerAngles.z; // * (2 / 3);
                adjustedAngle *= 0.85f;

                max = reticalLowMax;
                min = reticalLowMin;
            }

            Vector3 angles = new Vector3(0, 0, adjustedAngle);

            //if (playerInfo.playerNum == 1) { Debug.Log("Input: " + rInput + " Aim angle: " + adjustedAngle + " Local: " + retical.localEulerAngles.z); }

            retical.localEulerAngles = angles;

            if (retical.localEulerAngles.z > max || retical.localEulerAngles.z < min)
            {
                retical.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(retical.localEulerAngles.z, min, max));
            }
        }
    }

    void HandleThrust()
    {

        ApplyJoystickThrust();

    }

    void ApplyJoystickThrust()
    {
        float x = moveInput.x;
        float y = moveInput.y;

        if (Mathf.Abs(x) > .5 || Mathf.Abs(y) > .5)
        {
            Vector2 toDir = new Vector2(x, y).normalized;
            //Vector2 fromDir = (Vector2)(Quaternion.Euler(0, 0, hinge.jointAngle) * Vector2.up);
            Vector2 fromDir = transform.localPosition.normalized;

            float ang = Vector2.Angle(fromDir, toDir);

            Vector3 cross = Vector3.Cross(fromDir, toDir);

            if (cross.z < 0) ang = ang * -1;

            Debug.Log(fromDir + " " + toDir + " " + ang);

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
}
