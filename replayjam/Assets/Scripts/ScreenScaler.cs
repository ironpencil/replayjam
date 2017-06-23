using UnityEngine;
using System.Collections;

public class ScreenScaler : MonoBehaviour {

    public int targetPixelsPerUnit = 8;
    public int targetHalfScreenHeightInUnits = 9;    

    public int targetScreenPixelWidth = 160;
    public int targetScreenPixelHeight = 144;

    public int cameraScale = 1;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("UpdatePixelScale")]
    public void UpdatePixelScale()
    {

        int actualScreenWidthInUnits = targetScreenPixelWidth / targetPixelsPerUnit;
        int actualScreenHeightInUnits = targetScreenPixelHeight / targetPixelsPerUnit;

        int actualScreenWidthInPixels = actualScreenWidthInUnits * targetPixelsPerUnit;
        int actualScreenHeightInPixels = actualScreenHeightInUnits * targetPixelsPerUnit;

        Screen.SetResolution(actualScreenWidthInPixels, actualScreenHeightInPixels, false);

        float cameraSize = actualScreenHeightInUnits * 0.5f;

        cameraSize = cameraSize / cameraScale;

        Camera.main.orthographicSize = cameraSize;
        
    }
}
