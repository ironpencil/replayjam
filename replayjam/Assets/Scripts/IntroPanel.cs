using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroPanel : MonoBehaviour
{

    public float fadeInTime = 0.25f;
    public float displayTime = 0.5f;
    public float fadeOutTime = 0.25f;

    public float minimumDisplayTime = 0.0f;

    public bool automaticallyDismiss = true;

    public bool minimumDisplayTimeMet = false;

    public CanvasGroup fader;

    public IntroPanel nextPanel;
    public GameObject introParent;

    public bool lastPanel = false;
    public bool deactivateSelfOnDismiss = true;
    public bool deactivateParentOnDismiss = false;

    public List<string> dismissButtons;

    private bool doneDisplaying = false;

    void Update()
    {
        foreach (string buttonName in dismissButtons)
        {
            if (Input.GetButtonDown(buttonName))
            {
                DoneDisplaying();
            }
        }
    }

    public void DisplayPanel()
    {
        if (Globals.Instance.playIntro)
        {
            gameObject.SetActive(true);
            StartCoroutine(DoDisplayPanel());
        }
        else
        {
            lastPanel = true;
            DoneDisplaying();
        }
    }

    public void DoneDisplaying()
    {
        if (!doneDisplaying && minimumDisplayTimeMet)
        {
            doneDisplaying = true;

            if (deactivateSelfOnDismiss)
            {
                this.gameObject.SetActive(false);
            }

            if (lastPanel)
            {
                //let globals know to start
                Globals.Instance.IntroFinished();
                if (deactivateParentOnDismiss)
                {
                    introParent.SetActive(false);
                }
            }
            else
            {
                nextPanel.DisplayPanel();
            }
        }
    }

    public IEnumerator DoDisplayPanel()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInTime)
        {
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float deltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += deltaTime;

            float percentageComplete = elapsedTime / fadeInTime;

            fader.alpha = percentageComplete;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < displayTime)
        {
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float deltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += deltaTime;

            if (elapsedTime > minimumDisplayTime)
            {
                minimumDisplayTimeMet = true;
            }
        }

        yield return null;

        minimumDisplayTimeMet = true;

        if (automaticallyDismiss)
        {
            elapsedTime = 0.0f;

            while (elapsedTime < fadeOutTime)
            {
                float currentTime = Time.realtimeSinceStartup;

                yield return null;

                float deltaTime = Time.realtimeSinceStartup - currentTime;

                elapsedTime += deltaTime;

                float percentageComplete = elapsedTime / fadeOutTime;

                fader.alpha = 1 - percentageComplete;
            }

            DoneDisplaying();
        }
    }


}
