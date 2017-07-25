using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using System;
using System.Collections.Generic;
using TMPro;

public class PlayerInput : MonoBehaviour {
    public int playerPosition = 1;
    public int playerHealth = 3;
    public float invincibleTime = 3;
    private float invincibleTimeLeft = 0;
    public Transform retical;
    public List<SpriteRenderer> aimDots;
    public float reticalLowMax;
    public float reticalLowMin;
    public float reticalHighMax;
    public float reticalHighMin;
    public ParticleSystem shield;
    public GameObject destShieldPrefab;
    public GameObject playerExplosion;
    public PortraitBehavior portrait;

    //public float shieldAlpha = 0.35f;

    public SpriteRenderer playerShip;
    public Color blink;

    public bool invulnerable = false;
    public bool canShoot = true;
    //public SpriteRenderer aura;
    public GameObject playerRing;
    public GameObject playerNameTag;
    public TextMeshProUGUI playerNameText;

    public ShootProjectiles gun;

    public HingeJoint2D hinge;
    private Vector2 rInput;

    public float maxThrust;
    
    XboxController xboxController;

    Rigidbody2D rb2d;

    bool playerPaused = false;
    public bool kill = false;

    public ThrustStyle thrustStyle;
    public PlayerInfo playerInfo;

    public float tauntInterval;
    private float nextTauntTime;

    public GameObjectShaker portraitShaker;
    public SoundEffectHandler shieldHitSound;
    public SoundEffectHandler shipHitSound;
    public SoundEffectHandler shieldFadeSound;
    public SoundEffectHandler shipExplodeSound;

    public enum ThrustStyle
    {
        trigger, joystick
    }

	// Use this for initialization
	void Start () {
        nextTauntTime = Time.time;
        hinge = GetComponent<HingeJoint2D>();
        rb2d = GetComponent<Rigidbody2D>();
        portraitShaker = gameObject.GetComponent<GameObjectShaker>();
        portraitShaker.shakeObject = portrait.gameObject.GetComponent<GameObjectShake>();

        xboxController = (XboxController) playerInfo.playerNum;

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

        degreesPerPlayer -= 15;

        float angleLimit = degreesPerPlayer * 0.5f;// Mathf.Round(degreesPerPlayer * 0.5f);

        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = angleLimit * -1;
        limits.max = angleLimit;
        hinge.limits = limits;

        hinge.enabled = true;

        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(playerInfo.playerNum);

        foreach (SpriteRenderer aimDot in aimDots)
        {
            aimDot.color = playerColor;
        }

        playerColor.a = 0.25f;
        blink = playerColor;
        

        //aura.color = playerColor;
        invincibleTimeLeft = invincibleTime;
        StartCoroutine(HandleInvincibility());

        //set up player ring
        ParticleSystem ps = playerRing.GetComponent<ParticleSystem>();

        if (!ps.isPlaying)
        {
            var main = ps.main;
            main.startColor = playerColor;
            var shape = ps.shape;
            shape.arc = degreesPerPlayer;

            
            //this is bullshit but it works don't touch it
            
            Vector3 ringRotation = new Vector3(startRotation.x, startRotation.y, startRotation.z - angleLimit);
            
            playerRing.transform.Rotate(ringRotation);
            ps.Play();

            playerNameTag = playerRing.transform.GetChild(0).gameObject;

            float angle = startAngle - 7.5f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 dir = rot * Vector3.up;
            Vector3 worldDir = transform.TransformDirection(dir);

            playerNameTag.transform.position = worldDir * -7.0f;

            float nameTagAngle = startAngle;

            playerNameTag.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angleLimit - 90.0f);
            playerNameText = playerNameTag.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            playerNameText.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.25f);
            playerNameText.text = playerInfo.name;

            //end of (this particular) bullshit
        }

        if (!Globals.Instance.GameManager.enableShields)
        {
            shield.gameObject.SetActive(false);
            playerHealth = 1;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (invincibleTimeLeft > 0)
        {
            invincibleTimeLeft -= Time.deltaTime;
            if (invincibleTimeLeft < 0)
            {
                invincibleTimeLeft = 0;
            }
        }
        
        if (Globals.Instance.acceptPlayerGameInput)
        {
            HandleAttack();
            HandleTaunt();
            HandleAiming();
        }

        if (kill)
        {
            Kill(0);
        }
    }

    void FixedUpdate()
    {
        if (Globals.Instance.acceptPlayerGameInput)
        {
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
        if (canShoot)
        {
            if (XCI.GetButton(XboxButton.RightBumper, xboxController) ||
                XCI.GetButton(XboxButton.LeftBumper, xboxController) ||
                XCI.GetAxis(XboxAxis.RightTrigger, xboxController) > 0.0f ||
                XCI.GetAxis(XboxAxis.LeftTrigger, xboxController) > 0.0f)
            {
                gun.Shoot();
            }
        }
    }

    private void HandleTaunt()
    {
        if (XCI.GetButtonDown(XboxButton.Y, xboxController) &&
            nextTauntTime < Time.time)
        {
            nextTauntTime = Time.time + tauntInterval;
            Globals.Instance.GameManager.characterSounds.PlayVoice(CharacterSoundManager.VoiceType.Taunt, playerInfo.playerNum, true);
        }
    }

    void HandleAiming()
    {
        float x = XCI.GetAxisRaw(XboxAxis.RightStickX, xboxController);
        float y = XCI.GetAxisRaw(XboxAxis.RightStickY, xboxController);
        
        rInput = new Vector2(x, y);

        if (rInput.magnitude > 0)
        {
            // Original Rotation Setup
            var angle = Mathf.Atan2(rInput.y, rInput.x) * Mathf.Rad2Deg -90;

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
            } else
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

    public bool Hit(int hitBy)
    {
        bool killed = false;

        if (invulnerable) { return false; }

        if (invincibleTimeLeft == 0)
        {
            //Debug.Log("Preparing to shake " + playerInfo.playerNum);

            invincibleTimeLeft = invincibleTime;

            StartCoroutine(HandleInvincibility());
            
            shipHitSound.PlayEffect();

            playerHealth--;
            
            if (playerHealth < 1)
            {
                Kill(hitBy);
                killed = true;
            } else if (playerHealth == 2)
            {
                ParticleSystem.MainModule main = shield.main;
                main.simulationSpeed = 3.0f;
                shieldHitSound.PlayEffect();

            } else if (playerHealth == 1)
            {
                StartCoroutine(DestroyShield());
            }

            if (playerHealth > 0)
            {
                //killed player portrait updates are handled in Kill()
                portraitShaker.Shake();
                portrait.TakeDamage();
                Globals.Instance.GameManager.characterSounds.PlayVoice(CharacterSoundManager.VoiceType.Grunt, playerInfo.playerNum, false);
            }
            
        }
        
        return killed;
    }
    
    IEnumerator DestroyShield()
    {
        shieldFadeSound.PlayEffect();
        shield.gameObject.SetActive(false);
        GameObject destShield = GameObject.Instantiate(destShieldPrefab, gameObject.transform);
        SpriteRenderer shieldSprite = destShield.GetComponentInChildren<SpriteRenderer>();
        bool isFading = true;
        while (isFading)
        {
            Color newColor = shieldSprite.color;
            newColor.a -= 0.05f;
            isFading = newColor.a > 0;
            shieldSprite.color = newColor;

            yield return new WaitForSecondsRealtime(0.2f);
        }

        //Wait a bit to allow the particle effect to fade
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(destShield);
    }

    IEnumerator HandleInvincibility()
    {
        bool isBlinking = false;
        while (invincibleTimeLeft > 0)
        {
            isBlinking = !isBlinking;

            if (isBlinking)
            {
                playerShip.color = blink;
            } else
            {
                playerShip.color = Color.white;
            }
            
            yield return new WaitForSecondsRealtime(0.1f);
        }

        playerShip.color = Color.white;
    }

    public void Kill(int killedBy)
    {
        portraitShaker.Shake();
        portrait.TakeDamage();

        shipExplodeSound.PlayEffect();

        if (killedBy > 0)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                Globals.Instance.GameManager.characterSounds.PlayVoice(CharacterSoundManager.VoiceType.Death, playerInfo.playerNum, false);
            } else
            {
                Globals.Instance.GameManager.characterSounds.PlayVoice(CharacterSoundManager.VoiceType.Laugh, killedBy, false);
            }
        }
        
        GameObject explosion = GameObject.Instantiate(playerExplosion, transform.position, Quaternion.identity, Globals.Instance.dynamicsParent);

        ParticleSystem explosionPS = explosion.GetComponent<ParticleSystem>();

        var main = explosionPS.main;
        main.startColor = Globals.Instance.GameManager.GetPlayerColor(playerInfo.playerNum);

        Destroy(gameObject);

        if (Globals.Instance.GameManager.gameMode == GameManager.GameMode.Survival)
        {
            ParticleSystem ps = playerRing.GetComponent<ParticleSystem>();
            ps.Stop();
            Destroy(playerRing, 5.0f);
            
        }

        Destroy(explosion, 5.0f);

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
