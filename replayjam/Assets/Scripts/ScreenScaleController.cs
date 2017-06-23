using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenScaleController : MonoBehaviour {

    public int screenWidthInPixels = 160;
    public int screenHeightInPixels = 144;

    public int scale = 1;
    
    public List<int> availableScales = new List<int> { 1, 2, 4 };
    private int currentScaleIndex = 0;

    public ScreenScaler scaler;

	// Use this for initialization
	void Start () {
        Screen.fullScreen = false;
        UpdateScale();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Change Scale"))
        {
            CycleScale();
        }
	}

    public void CycleScale()
    {
        currentScaleIndex++;

        if (currentScaleIndex >= availableScales.Count)
        {
            currentScaleIndex = 0;
        }

        scale = availableScales[currentScaleIndex];

        UpdateScale();
    }

    public void UpdateScale()
    {
        scaler.targetScreenPixelWidth = screenWidthInPixels * scale;
        scaler.targetScreenPixelHeight = screenHeightInPixels * scale;

        scaler.cameraScale = scale;

        scaler.UpdatePixelScale();
    }
}
