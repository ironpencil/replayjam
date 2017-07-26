using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitSpeechManager : MonoBehaviour {

    public List<RectTransform> speechContainers;
    public List<Image> emotionImages;

    public Sprite dizzySprite;
    public Sprite laughSprite;
    public Sprite smirkSprite;

    public enum Emotion
    {
        Dizzy,
        Laugh,
        Smirk
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowEmotion(Emotion emotion, int playerNum, float duration)
    {
        StartCoroutine(DoShowEmotion(emotion, playerNum, duration));
    }

    IEnumerator DoShowEmotion(Emotion emotion, int playerNum, float duration)
    {
        int index = playerNum - 1;

        Image theImage = emotionImages[index];
        RectTransform rect = speechContainers[index];

        switch (emotion)
        {
            case Emotion.Dizzy:
                theImage.sprite = dizzySprite;
                break;
            case Emotion.Laugh:
                theImage.sprite = laughSprite;
                break;
            case Emotion.Smirk:
                theImage.sprite = smirkSprite;
                break;
            default:
                break;
        }

        if (rect.gameObject.activeSelf)
        {
            yield break; // for now, don't worry about trying to transition between emotes...
        }

        theImage.canvasRenderer.SetAlpha(0.0f);

        Vector2 openSize = rect.sizeDelta;
        Vector2 closedSize = openSize * 0.1f;

        Vector2 currentSize = closedSize;
        rect.sizeDelta = currentSize;

        rect.gameObject.SetActive(true);

        float openTime = duration * 0.05f;
        float longWait = duration * 0.8f;

        float elapsedTime = 0.0f;

        while (elapsedTime < openTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentSize.x = Mathf.Lerp(closedSize.x, openSize.x, elapsedTime / openTime);
            rect.sizeDelta = currentSize;
        }

        currentSize.x = openSize.x;
        rect.sizeDelta = currentSize;

        elapsedTime = 0.0f;

        while (elapsedTime < openTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentSize.y = Mathf.Lerp(closedSize.y, openSize.y, elapsedTime / openTime);
            rect.sizeDelta = currentSize;
        }

        currentSize = openSize;
        rect.sizeDelta = currentSize;

        theImage.canvasRenderer.SetAlpha(1.0f);

        yield return new WaitForSeconds(longWait);

        theImage.canvasRenderer.SetAlpha(0.0f);

        elapsedTime = 0.0f;

        while (elapsedTime < openTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentSize.y = Mathf.Lerp(openSize.y, closedSize.y, elapsedTime / openTime);
            rect.sizeDelta = currentSize;
        }

        currentSize.y = closedSize.y;
        rect.sizeDelta = currentSize;

        elapsedTime = 0.0f;

        while (elapsedTime < openTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentSize.x = Mathf.Lerp(openSize.x, closedSize.x, elapsedTime / openTime);
            rect.sizeDelta = currentSize;
        }

        currentSize = closedSize;
        rect.sizeDelta = currentSize;

        rect.gameObject.SetActive(false);

        rect.sizeDelta = openSize; //reset the speech window so we're always working with an "open" window initially
    }
}
