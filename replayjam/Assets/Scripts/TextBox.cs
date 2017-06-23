using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextBox : MonoBehaviour {

    public List<string> screenStrings;

    public int nextScreenIndex = 0;

    public MessageBox messageBox;
    public Text textControl;
    public Scrollbar scrollbar;

	// Use this for initialization
	void Start () {

        NextScreen();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextScreen()
    {
        if (nextScreenIndex >= screenStrings.Count)
        {
            messageBox.StartClose();
            return;
        }

        string nextText = screenStrings[nextScreenIndex];

        textControl.text = nextText;
        scrollbar.value = 1.0f;

        nextScreenIndex++;
    }
}
