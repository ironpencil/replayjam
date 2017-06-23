using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XboxCtrlrInput;

public class OptionsScreenBehavior : MonoBehaviour {
    private GameOptions selectedGameOptions;
    public GameOptions easyGameOptions;
    public GameOptions mediumGameOptions;
    public GameOptions hardGameOptions;
    public GameOptions wtfGameOptions;
    public GameOptions weirdGameOptions;
    public EventSystem eventSystem;

    public Dropdown difficulty;

    // Spinner Menu
    public Toggle spinner;

    public Text spinnerCountText;
    public Slider spinnerCount;

    public Text spinnerSpeedText;
    public Slider spinnerSpeed;

    // Wall Laser (Laser Turret) Menu
    public Toggle wallLaser;

    public Text wallLaserCountText;
    public Slider wallLaserCount;

    public Text wallLaserBounceCountText;
    public Slider wallLaserBounceCount;

    public Text wallLaserShotFrequencyText;
    public Slider wallLaserShotFrequency;

    //public Text wallLaserChargeTimeText;
    //public Slider wallLaserChargeTime;

    // Bomb Menu
    public Toggle bomb;

    public Text bombFrequencyText;
    public Slider bombFrequency;

    public Text bombFallTimeText;
    public Slider bombFallTime;

    // Mine Menu
    public Toggle mine;
    public Toggle mineStartsArmed;

    public Text mineTimeToDetonateText;
    public Slider mineTimeToDetonate;

    public Text maxMinesText;
    public Slider maxMines;

    public Text mineRespawnFrequencyText;
    public Slider mineRespawnFrequency;

    public Text mineMaxLifetimeText;
    public Slider mineMaxLifetime;

    // Floor Menu
    public Toggle floor;

    public Text floorCollapseIntervalText;
    public Slider floorCollapseInterval;
    
    IEnumerator DisplayCoroutine()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(difficulty.gameObject);
    }

    public void DisplayOptions()
    {
        gameObject.SetActive(true);
        StartCoroutine("DisplayCoroutine");
    }

    private void HandleDifficultyChange(int arg)
    {
        switch(arg)
        {
            case 0:
                easyGameOptions.Clone(selectedGameOptions);
                break;
            case 1:
                mediumGameOptions.Clone(selectedGameOptions);
                break;
            case 2:
                hardGameOptions.Clone(selectedGameOptions);
                break;
            case 3:
                wtfGameOptions.Clone(selectedGameOptions);
                break;
            case 4:
                weirdGameOptions.Clone(selectedGameOptions);
                break;
        }
        UpdateDisplays();
    }
    
    void SetupHandlers()
    {
        difficulty.onValueChanged.AddListener(HandleDifficultyChange);

        // Spinner
        spinner.onValueChanged.AddListener(val => { selectedGameOptions.isSpinnerEnabled = val; Debug.Log("Spinner Enabled: " + val); });
        spinnerCount.onValueChanged.AddListener(val => { selectedGameOptions.spinnerCount = (int)val; spinnerCountText.text = "Spinner Count: " + (int)val; Debug.Log("Spinner Count: " + (int)val); });
        spinnerSpeed.onValueChanged.AddListener(val => { selectedGameOptions.spinnerSpeed = (int)val; spinnerSpeedText.text = "Spinner Speed: " + (int)val; Debug.Log("Spinner Speed: " + (int)val); });

        // Wall Laser (Laser Turret) Handlers
        wallLaser.onValueChanged.AddListener(val => { selectedGameOptions.isWallLaserEnabled = val; Debug.Log("Wall Turret Enabled: " + val); });
        wallLaserCount.onValueChanged.AddListener(val => { selectedGameOptions.wallLaserCount = (int)val; wallLaserCountText.text = "Wall Turret Count: " + (int)val; Debug.Log("Wall Turret Count: " + (int)val); });
        wallLaserShotFrequency.onValueChanged.AddListener(val => { selectedGameOptions.wallLaserShotFrequency = val; wallLaserShotFrequencyText.text = "Turret Shot Freq: " + val; Debug.Log("Wall Turret Shot Freq: " + val); });
        //wallLaserChargeTime.onValueChanged.AddListener(val => { gameOptions.wallLaserChargeTime = val; wallLaserChargeTimeText.text = "Wall Turret Charge Time: " + val; Debug.Log("Wall Turret Charge Time: " + val); });
        wallLaserBounceCount.onValueChanged.AddListener(val => { selectedGameOptions.wallLaserBounceCount = (int)val; wallLaserBounceCountText.text = "Laser Bounces: " + (int)val; Debug.Log("Laser Bounces: " + (int)val); });

        // Bomb Handlers
        bomb.onValueChanged.AddListener(val => { selectedGameOptions.isBombEnabled = val; Debug.Log("Bomb Enabled: " + val); });
        bombFrequency.onValueChanged.AddListener(val => { selectedGameOptions.bombFrequency = val; bombFrequencyText.text = "Bomb Freq: " + val; Debug.Log("Bomb Freq: " + val); });
        bombFallTime.onValueChanged.AddListener(val => { selectedGameOptions.bombFallTime = val; bombFallTimeText.text = "Fall Time: " + val; Debug.Log("Bomb Fall Time: " + val); });

        // Mine Handlers
        mine.onValueChanged.AddListener(val => { selectedGameOptions.isMineEnabled = val; Debug.Log("Mine Enabled: " + val); });
        mineStartsArmed.onValueChanged.AddListener(val => { selectedGameOptions.mineStartsArmed = val; Debug.Log("Mines Start Armed: " + val); });
        mineTimeToDetonate.onValueChanged.AddListener(val => { selectedGameOptions.mineTimeToDetonate = (int)val; mineTimeToDetonateText.text = "Mine Time to Detonate: " + val; Debug.Log("Mine Time to Detonate: " + val); });
        maxMines.onValueChanged.AddListener(val => { selectedGameOptions.maxMines = (int)val; maxMinesText.text = "Max Mines: " + (int)val; Debug.Log("Max Mines: " + (int)val); });
        mineRespawnFrequency.onValueChanged.AddListener(val => { selectedGameOptions.mineRespawnFrequency = val; mineRespawnFrequencyText.text = "Mine Respawn Freq: " + val; Debug.Log("Mine Respawn Freq: " + val); });
        mineMaxLifetime.onValueChanged.AddListener(val => { selectedGameOptions.mineMaxLifetime = val; mineMaxLifetimeText.text = "Mine Max Lifetime: " + val; Debug.Log("Mine Max Lifetime: " + val); });

        // Floor Handlers
        floor.onValueChanged.AddListener(val => { selectedGameOptions.isFloorEnabled = val; Debug.Log("Floor Enabled: " + val); });
        floorCollapseInterval.onValueChanged.AddListener(val => { selectedGameOptions.floorCollapseInterval = val; floorCollapseIntervalText.text = "Floor Collapse Dur: " + val; Debug.Log("Floor Collapse Duration: " + val); });
    }

    void UpdateDisplays()
    {
        // Spinner
        spinner.isOn = selectedGameOptions.isSpinnerEnabled;
        spinnerCount.value = selectedGameOptions.spinnerCount;
        spinnerCountText.text = "Spinner Count: " + selectedGameOptions.spinnerCount;
        spinnerSpeed.value = selectedGameOptions.spinnerSpeed;
        spinnerSpeedText.text = "Spinner Speed: " + selectedGameOptions.spinnerSpeed;

        
        // Wall Laser
        wallLaser.isOn = selectedGameOptions.isWallLaserEnabled;
        wallLaserCount.value = selectedGameOptions.wallLaserCount;
        wallLaserCountText.text = "Wall Turret Count: " + selectedGameOptions.wallLaserCount;
        wallLaserShotFrequency.value = selectedGameOptions.wallLaserShotFrequency;
        wallLaserShotFrequencyText.text = "Turret Shot Freq: " + selectedGameOptions.wallLaserShotFrequency;
        wallLaserBounceCount.value = selectedGameOptions.wallLaserBounceCount;
        wallLaserBounceCountText.text = "Laser Bounces: " + selectedGameOptions.wallLaserBounceCount;

        
        // Bomb
        bomb.isOn = selectedGameOptions.isBombEnabled;
        bombFrequency.value = selectedGameOptions.bombFrequency;
        bombFrequencyText.text = "Bomb Freq: " + selectedGameOptions.bombFrequency;
        bombFallTime.value = selectedGameOptions.bombFallTime;
        bombFallTimeText.text = "Fall Time: " + selectedGameOptions.bombFallTime;
        

        // Mine
        mine.isOn = selectedGameOptions.isMineEnabled;
        mineStartsArmed.isOn = selectedGameOptions.mineStartsArmed;
        mineTimeToDetonate.value = selectedGameOptions.mineTimeToDetonate;
        mineTimeToDetonateText.text = "Mine Time to Detonate: " + selectedGameOptions.mineTimeToDetonate;
        maxMines.value = selectedGameOptions.maxMines;
        maxMinesText.text = "Max Mines: " + selectedGameOptions.maxMines;
        mineRespawnFrequency.value = selectedGameOptions.mineRespawnFrequency;
        mineRespawnFrequencyText.text = "Mine Respawn Freq: " + selectedGameOptions.mineRespawnFrequency;
        mineMaxLifetime.value = selectedGameOptions.mineMaxLifetime;
        mineMaxLifetimeText.text = "Mine max Lifetime: " + selectedGameOptions.mineMaxLifetime;
        

        // Floor
        floor.isOn = selectedGameOptions.isFloorEnabled;
        floorCollapseInterval.value = selectedGameOptions.floorCollapseInterval;
        floorCollapseIntervalText.text = "Collapse Interval: " + selectedGameOptions.floorCollapseInterval;
    }

    // Use this for initialization
    void Start () {
        selectedGameOptions = Globals.Instance.GameManager.gameOptions;
        easyGameOptions.Clone(selectedGameOptions);
        SetupHandlers();
        UpdateDisplays();
    }

    // Update is called once per frame
    void Update()
    {
        for (int player = 1; player < 5; player++)
        {
            if (XCI.GetButtonDown(XboxButton.Back, (XboxController)player))
            {
                Globals.Instance.GameManager.DisplayPlayerSetup();
            }
        }
    }
}
