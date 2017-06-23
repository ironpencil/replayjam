using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PlayerColorSelector : MonoBehaviour {

    public int playerNum;
    public Image sampleImage;

    public Color currentColor = Color.white;

    public float sensitivity = 0.5f;

    XboxController controller;

	// Use this for initialization
	void Start () {
        controller = (XboxController)playerNum;
        currentColor = Globals.Instance.GameManager.GetPlayerColor(playerNum);
        sampleImage.color = currentColor;
	}
	
	// Update is called once per frame
	void Update () {

        //if (XCI.GetButtonDown(XboxButton.Y, controller))
        //{
        //    float h;
        //    float s;
        //    float v;

        //    Color.RGBToHSV(currentColor, out h, out s, out v);

        //    Debug.Log("RGB: " + currentColor.ToString());
        //    Debug.Log("HSV: " + h + " " + s + " " + v);
        //}

        float lX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float rX = XCI.GetAxis(XboxAxis.RightStickX, controller);
        float rY = XCI.GetAxis(XboxAxis.RightStickY, controller);

        if (lX != 0.0f || rX != 0.0f || rY != 0.0f)
        {
            Debug.Log("Changing player color: " + lX + " " + rX + " " + rY);
            float h;
            float s;
            float v;

            Color.RGBToHSV(currentColor, out h, out s, out v);

            float newH;
            float newS;
            float newV;

            newH = h + (lX * Time.deltaTime * sensitivity);
            newS = s + (rX * Time.deltaTime * sensitivity);
            newV = v + (rY * Time.deltaTime * sensitivity);

            h = Mathf.Repeat(newH, 1.0f);
            s = Mathf.Clamp(newS, 0.0f, 1.0f);
            v = Mathf.Clamp(newV, 0.0f, 1.0f);

            currentColor = Color.HSVToRGB(h, s, v);

            sampleImage.color = currentColor;
            Globals.Instance.GameManager.SetPlayerColor(currentColor, playerNum);
        }


    }
}
