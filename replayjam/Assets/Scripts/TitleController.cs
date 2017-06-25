using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XboxCtrlrInput;

public class TitleController : MonoBehaviour {

    public CanvasRenderer directions;

    public float directionsFadeInTime;
    public float directionsFadeDuration;

    public SoundEffectHandler welcomeTo;
    public SoundEffectHandler blackHoleBullet;

    bool introFinished = false;

    bool directionsDisplayed = false;
    
    // Use this for initialization
    void Start () {
        DisplayTitle();
	}
	
	// Update is called once per frame
	void Update () {

        if (!directionsDisplayed && introFinished)
        {
            directionsDisplayed = true;
            StartCoroutine(DoFadeInCanvas(directions, directionsFadeInTime, directionsFadeDuration));
        }

        //if (Input.anyKeyDown)
        //{
        //    //button was pressed
        //    if (!introFinished)
        //    {
        //        //intro hasn't finished playing - finish it
        //        StopAllCoroutines();
                

        //        directionsDisplayed = false;
        //        introFinished = true;

        //    } else
        //    {
        //        //IntroPanel panel = gameObject.GetComponent<IntroPanel>();
        //        //panel.DoneDisplaying();
        //    }
        //}
		
	}

    public void DisplayTitle()
    {
        

        //directions.SetAlpha(0.0f);
    }

    IEnumerator DoMoveTransform(RectTransform t, Vector2 from, Vector2 to, float start, float duration, AnimationCurve easing)
    {
        t.anchoredPosition = from;

        float startTime = Time.time + start;

        while (Time.time < startTime)
        {
            yield return 0; //wait one frame
        }

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            yield return 0;
            elapsedTime = Time.time - startTime;
            Vector2 newPos = Vector2.Lerp(from, to, easing.Evaluate(elapsedTime / duration));
            t.anchoredPosition = newPos;
        }

        t.anchoredPosition = to;

    }

    IEnumerator DoFadeInCanvas(CanvasRenderer cr, float start, float duration)
    {

        cr.SetAlpha(0.0f);

        float startTime = Time.time + start;

        while (Time.time < startTime)
        {
            yield return 0; //wait one frame
        }

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            yield return 0;
            elapsedTime = Time.time - startTime;
            float newAlpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime);
            cr.SetAlpha(newAlpha);
        }

        cr.SetAlpha(1.0f);
    }

    IEnumerator WaitThenPlay(SoundEffectHandler sound, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        sound.PlayEffect();
    }
}
