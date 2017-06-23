using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour {

    public RectTransform transitionPanel;

    public float rectTopPos = 164;
    public float rectMidPos = 0;
    public float rectBottomPos = -164;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool transitionComplete = false;
    public bool doingTransition = false;

    public IEnumerator TransitionCoverScreen(float duration)
    {
        if (!doingTransition)
        {
            doingTransition = true;
            transitionComplete = false;

            float elapsedTime = 0.0f;
            float currentTime = Time.realtimeSinceStartup;

            Vector2 rectStartPos = new Vector2(0, rectTopPos);

            transitionPanel.anchoredPosition = rectStartPos;    

            while (elapsedTime < duration)
            {
                yield return null;

                float deltaTime = Time.realtimeSinceStartup - currentTime;
                currentTime = Time.realtimeSinceStartup;

                elapsedTime += deltaTime;

                float percentComplete = elapsedTime / duration;

                float rectY = Mathf.Lerp(rectTopPos, rectMidPos, percentComplete);

                Vector2 rectPos = new Vector2(0, rectY);

                transitionPanel.anchoredPosition = rectPos;

            }

            Vector2 rectFinalPos = new Vector2(0, rectMidPos);

            transitionPanel.anchoredPosition = rectFinalPos;

            doingTransition = false;
            transitionComplete = true;
        }
        
    }

    public IEnumerator TransitionUncoverScreen(float duration)
    {
        if (!doingTransition)
        {
            doingTransition = true;
            transitionComplete = false;

            float elapsedTime = 0.0f;
            float currentTime = Time.realtimeSinceStartup;

            Vector2 rectStartPos = new Vector2(0, rectMidPos);

            transitionPanel.anchoredPosition = rectStartPos;            

            while (elapsedTime < duration)
            {
                yield return null;

                float deltaTime = Time.realtimeSinceStartup - currentTime;
                currentTime = Time.realtimeSinceStartup;

                elapsedTime += deltaTime;

                float percentComplete = elapsedTime / duration;

                float rectY = Mathf.Lerp(rectMidPos, rectBottomPos, percentComplete);

                Vector2 rectPos = new Vector2(0, rectY);

                transitionPanel.anchoredPosition = rectPos;

            }

            Vector2 rectFinalPos = new Vector2(0, rectBottomPos);

            transitionPanel.anchoredPosition = rectFinalPos;

            doingTransition = false;
            transitionComplete = true;
        }
    }
}
