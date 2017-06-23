using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XboxCtrlrInput;

public class TitleController : MonoBehaviour {

    public RectTransform bg;
    public RectTransform fg;
    public RectTransform title;
    public CanvasRenderer directions;

    public Vector2 bgStartPos;
    public Vector2 bgEndPos;
    public AnimationCurve bgEasing;

    public float bgBeginTime;
    public float bgDuration;

    public Vector2 fgStartPos;
    public Vector2 fgEndPos;
    public AnimationCurve fgEasing;

    public float fgBeginTime;
    public float fgDuration;

    public Vector2 titleStartPos;
    public Vector2 titleEndPos;
    public AnimationCurve titleEasing;

    public float titleBeginTime;
    public float titleDuration;

    public SoundEffectHandler titleSound;

    public float directionsFadeInTime;
    public float directionsFadeDuration;

    int finishedComponents = 0;
    List<int> controllers = new List<int>() { 1, 2, 3, 4 };

    bool directionsDisplayed = false;
    
    // Use this for initialization
    void Start () {
        DisplayTitle();
	}
	
	// Update is called once per frame
	void Update () {

        if (!directionsDisplayed && finishedComponents == 3)
        {
            directionsDisplayed = true;
            StartCoroutine(DoFadeInCanvas(directions, directionsFadeInTime, directionsFadeDuration));
        }

        if (controllers.Any(c => XCI.GetButtonDown(XboxButton.A, (XboxController)c)))
        {
            //button was pressed
            if (finishedComponents < 3)
            {
                //intro hasn't finished playing - finish it
                StopAllCoroutines();
                bg.anchoredPosition = bgEndPos;
                fg.anchoredPosition = fgEndPos;
                title.anchoredPosition = titleEndPos;
                titleSound.PlayEffect();

                directionsDisplayed = false;
                finishedComponents = 3;

            } else
            {
                IntroPanel panel = gameObject.GetComponent<IntroPanel>();
                panel.DoneDisplaying();
            }
        }
		
	}

    public void DisplayTitle()
    {
        StartCoroutine(DoMoveTransform(bg, bgStartPos, bgEndPos, bgBeginTime, bgDuration, bgEasing));
        StartCoroutine(DoMoveTransform(fg, fgStartPos, fgEndPos, fgBeginTime, fgDuration, fgEasing));
        StartCoroutine(DoMoveTransform(title, titleStartPos, titleEndPos, titleBeginTime, titleDuration, titleEasing));
        StartCoroutine(WaitThenPlay(titleSound, titleBeginTime));

        directions.SetAlpha(0.0f);

        AudioManager.Instance.StartMusic(AudioManager.Instance.musicFadeInTime, false);
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

        finishedComponents++;
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
