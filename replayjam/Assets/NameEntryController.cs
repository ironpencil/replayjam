using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameEntryController : MonoBehaviour {

    public List<Text> characterDisplay = new List<Text>();
    public Text nameLabel;
    public Text nameDisplay;
    public GameObject textEntryParent;
    public RectTransform cursor;

    public char[] characters;

    private Color textColor = Color.white;
    public Color TextColor
    {
        get { return textColor; }
        set
        {
            textColor = value;
            nameLabel.color = textColor;
            nameDisplay.color = textColor;
            foreach (Text charText in characterDisplay)
            {
                charText.color = textColor;
            }
        }
    }
    public int cursorPos = 0;
    //public int characterIndex = 0;
    public Vector2 cursorOffset = new Vector2(0, -30);
    bool cursorInitialized = false;

    public string validCharacters = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-!?";

    public List<string> randomNames = new List<string>() { "BIFF", "ROCK", "JERRY", "WLBH4", "NOSKP", "JIIIM", "TK421", "SNSDK", "NESS", "RANDO", "CHUNK", "SPIKE" };

	// Use this for initialization
	void Start () {
        //characters = new char[characterDisplay.Count];
        //for (int i = 0; i < characterDisplay.Count; i++)
        //{
        //    characters[i] = characterDisplay[i].text[0];
        //}
        SetName(GetRandomName(false));
	}

    void OnEnable()
    {
        ResetCursor();
    }

    void ResetCursor()
    {
        if (cursorInitialized)
        {
            cursorPos = 0;
            cursor.anchoredPosition = characterDisplay[cursorPos].rectTransform.anchoredPosition + cursorOffset;
        } else
        {
            cursor.gameObject.SetActive(false);
            StartCoroutine(DoInitializeCursor());
        }
    }

    IEnumerator DoInitializeCursor()
    {
        //wait for 2 frames for Text controls to layout and initialize properly
        yield return null;
        yield return null;

        cursor.gameObject.SetActive(true);
        cursorInitialized = true;
        ResetCursor();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCursor(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCursor(false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeLetter(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeLetter(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string name = GetName();
            Debug.Log("Name = '" + name + "'");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetRandomName();
        }
    }

    public void SetRandomName()
    {
        string newName = GetRandomName(true);
        SetName(newName);
    }

    public string GetRandomName(bool checkRepeat)
    {
        string currentName = "";

        if (checkRepeat)
        {
            currentName = GetName();
        }

        string newName = currentName;

        while (currentName.Equals(newName))
        {
            int randomNameIndex = Random.Range(0, randomNames.Count);
            newName = randomNames[randomNameIndex];
        }

        return newName;
    }

    public void SetName(string newName)
    {
        int maxLength = characterDisplay.Count;

        newName = newName.PadRight(maxLength, ' ');
        if (newName.Length > maxLength)
        {
            newName = newName.Substring(0, maxLength);
        }

        characters = newName.ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            char theChar = characters[i];
            characterDisplay[i].text = theChar.ToString();
        }
    }

    public void ChangeLetter(bool next)
    {
        int direction = (next ? 1 : -1);

        char currentLetter = characters[cursorPos];
        int currentIndex = validCharacters.IndexOf(currentLetter);
        int newIndex = currentIndex + direction;

        newIndex = (int) Mathf.Repeat(newIndex, validCharacters.Length);
        char newLetter = validCharacters[newIndex];

        characters[cursorPos] = newLetter;
        characterDisplay[cursorPos].text = newLetter.ToString();
    }

    public void MoveCursor(bool next)
    {
        int direction = (next ? 1 : -1);

        int newCursorPos = cursorPos + direction;
        newCursorPos = (int)Mathf.Repeat(newCursorPos, characterDisplay.Count);

        cursorPos = newCursorPos;

        Vector2 characterPos = characterDisplay[cursorPos].rectTransform.anchoredPosition;
        cursor.anchoredPosition = characterPos + cursorOffset;
    }

    public string GetName()
    {
        string name = new string(characters).Trim();

        if (string.IsNullOrEmpty(name))
        {
            name = GetRandomName(false);
            SetName(name);
        }

        return name;
    }

    public void DisplayName()
    {
        nameDisplay.gameObject.SetActive(true);
        nameLabel.gameObject.SetActive(false);
        textEntryParent.SetActive(false);

        string name = GetName();
        nameDisplay.GetComponent<Text>().text = name;
    }

    public void DisplayTextEntry()
    {
        ResetCursor();
        nameDisplay.gameObject.SetActive(false);
        nameLabel.gameObject.SetActive(true);
        textEntryParent.SetActive(true);
    }
}
