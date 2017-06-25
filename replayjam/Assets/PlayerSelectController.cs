using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectController : MonoBehaviour {

    public Text startGame;
    public List<PlayerSelector> playerSelectors;

	// Use this for initialization
	void Start () {
        startGame.canvasRenderer.SetAlpha(0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetScreen()
    {
        foreach (PlayerSelector ps in playerSelectors)
        {
            ps.Leave();
        }

        startGame.canvasRenderer.SetAlpha(0.0f);
    }

    public void DisplayStartGame(bool doDisplay)
    {
        StopAllCoroutines();

        float newAlpha = 0.0f;
        if (doDisplay)
        {
            newAlpha = 1.0f;
        }

        StartCoroutine(DoFadeStartGame(newAlpha));
    }

    IEnumerator DoFadeStartGame(float alpha)
    {
        float fadeTime = 0.5f;

        float elapsedTime = 0.0f;

        float startAlpha = startGame.canvasRenderer.GetAlpha();

        if (alpha == startAlpha)
        {
            elapsedTime = fadeTime; //we're done
        }

        while (elapsedTime < fadeTime)
        {
            float percentDone = elapsedTime / fadeTime;

            startGame.canvasRenderer.SetAlpha(Mathf.Lerp(startAlpha, alpha, percentDone));

            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;
        }

        startGame.canvasRenderer.SetAlpha(alpha);
    }
}
