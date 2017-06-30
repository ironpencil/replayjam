using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMatchPrompt : MonoBehaviour {

    public Text prompt;
    public Text start;

    public float promptDisplayTime = 2.0f;
    public float startDisplayTime = .5f;

    public float fadeTime = 0.25f;

	// Use this for initialization
	void Start () {
        prompt.canvasRenderer.SetAlpha(0.0f);
        start.canvasRenderer.SetAlpha(0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Display()
    {
        StartCoroutine(DisplayPrompts());
    }

    IEnumerator DisplayPrompts()
    {
        float elapsedTime = 0.0f;
        prompt.canvasRenderer.SetAlpha(0.0f);
        start.canvasRenderer.SetAlpha(0.0f);

        while (elapsedTime < fadeTime)
        {
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;

            prompt.canvasRenderer.SetAlpha(Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeTime));    
        }

        yield return new WaitForSeconds(promptDisplayTime);

        elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;

            prompt.canvasRenderer.SetAlpha(Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeTime));
        }

        yield return new WaitForSeconds(startDisplayTime);

        start.canvasRenderer.SetAlpha(1.0f);
        Globals.Instance.GameManager.EnableShooting();

        yield return new WaitForSeconds(startDisplayTime);

        elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;

            start.canvasRenderer.SetAlpha(Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeTime));
        }

        

    }
}
