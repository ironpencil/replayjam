using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {

    public RectTransform rect;
    public CanvasGroup canvasGroup;

    public Vector2 closedSize = new Vector2(76.0f, 76.0f);
    public Vector2 openSize = new Vector2(400.0f, 300.0f);

    public float openTime = 1.0f;
    public float closeTime = 1.0f;

    public bool isOpen = false;
    public bool isResizing = false;

    public bool visibleWhileClosed = false;
    public float visibleAlpha = 1.0f;
    public float invisibleAlpha = 0.0f;

    public SoundEffectHandler openSound;
    public SoundEffectHandler closeSound;
    public SoundEffectHandler openedSound;

    public bool playOpenedSound = false;
    public bool duckMusicOnOpen = false;
    public bool unduckMusicOnClose = false;

    private bool playSound = false;

    private IEnumerator currentResize = null;
    private IEnumerator currentHorizontalResize = null;
    private IEnumerator currentVerticalResize = null;

    public List<Transform> disableWhileClosed = new List<Transform>();

    public enum ResizeStyle
    {
        Simultaneous,
        WidthFirst,
        HeightFirst
    }

    public ResizeStyle openStyle = ResizeStyle.WidthFirst;
    public ResizeStyle closeStyle = ResizeStyle.HeightFirst;
    
	// Use this for initialization
	void Start () {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        playSound = false;
        if (isOpen)
        {
            SetOpen();
        }
        else
        {
            SetClosed();
        }
        playSound = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ToggleAndWait()
    {
        DebugLogger.Log("Toggling - currently " + isOpen);

        if (isOpen)
        {
            yield return StartCoroutine(CloseAndWait(closeTime));
        }
        else
        {
            yield return StartCoroutine(OpenAndWait(openTime));
        }
    }

    public void ToggleWithoutWait()
    {
        StartCoroutine(ToggleAndWait());
    }
    
    public IEnumerator OpenAndWait(float duration)
    {
        SetVisible(true);
        //DebugLogger.Log("Opening");
        if (isResizing)
        {
            DebugLogger.Log("OpenAndWait::StopCoroutine:" + gameObject.name);
            StopCoroutine(currentResize);
            StopCoroutine(currentHorizontalResize);
            StopCoroutine(currentVerticalResize);
        }

        if (duckMusicOnOpen)
        {
            AudioManager.Instance.DuckMusic();
        }

        if (openSound != null && playSound)
        {
            openSound.PlayEffect();
        }

        currentResize = Resize(closedSize, openSize, openTime, openStyle);
        yield return StartCoroutine(currentResize);
        isOpen = true;
        EnableChildren(true);

        if (openedSound != null && playOpenedSound && playSound)
        {
            openedSound.PlayEffect();
        }
    }
    
    public IEnumerator CloseAndWait(float duration)
    {
        //DebugLogger.Log("Closing");
        isOpen = false; 
        EnableChildren(false);

        if (isResizing)
        {
            DebugLogger.Log("CloseAndWait::StopCoroutine:" + gameObject.name);
            StopCoroutine(currentResize);
            StopCoroutine(currentHorizontalResize);
            StopCoroutine(currentVerticalResize);
        }

        if (unduckMusicOnClose)
        {
            AudioManager.Instance.UnduckMusic();
        }

        if (closeSound != null && playSound)
        {
            closeSound.PlayEffect();
        }

        currentResize = Resize(openSize, closedSize, duration, closeStyle);
        yield return StartCoroutine(currentResize);
        
        if (!visibleWhileClosed)
        {
            SetVisible(false);
        }
    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            canvasGroup.alpha = visibleAlpha;
        }
        else
        {
            canvasGroup.alpha = invisibleAlpha;
        }
    }

    public void EnableChildren(bool enable)
    {
        foreach (Transform child in disableWhileClosed)
        {
            child.gameObject.SetActive(enable);
        }
    }

    public void SetOpen()
    {
        StartCoroutine(OpenAndWait(0.0f));
    }

    public void SetClosed()
    {
        StartCoroutine(CloseAndWait(0.0f));
    }

    [ContextMenu("Open")]
    public void StartOpen()
    {
        StartCoroutine(OpenAndWait(openTime));
    }

    [ContextMenu("Close")]
    public void StartClose()
    {
        StartCoroutine(CloseAndWait(closeTime));        
    }          

    private IEnumerator Resize(Vector2 startSize, Vector2 targetSize, float duration, ResizeStyle resizeStyle)
    {
        isResizing = true;

        //DebugLogger.Log("Resize " + gameObject.name + " start: t=" + Time.realtimeSinceStartup);
        switch (resizeStyle)
        {
            case ResizeStyle.Simultaneous:
                currentHorizontalResize = ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration);
                currentVerticalResize = ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration);
                StartCoroutine(currentHorizontalResize);
                yield return StartCoroutine(currentVerticalResize);
                break;
            case ResizeStyle.WidthFirst:
                currentHorizontalResize = ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration * 0.5f);
                currentVerticalResize = ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration * 0.5f);
                yield return StartCoroutine(currentHorizontalResize);
                yield return StartCoroutine(currentVerticalResize);
                break;
            case ResizeStyle.HeightFirst:
                currentHorizontalResize = ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration * 0.5f);
                currentVerticalResize = ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration * 0.5f);
                yield return StartCoroutine(currentVerticalResize);
                yield return StartCoroutine(currentHorizontalResize);                                
                break;
            default:
                break;
        }
        //DebugLogger.Log("Resize " + gameObject.name + " complete: t=" + Time.realtimeSinceStartup);

        isResizing = false;
        //float elapsedTime = 0.0f;

        //while (elapsedTime < duration)
        //{
        //    float percentComplete = elapsedTime / duration;
        //    float newWidth = Mathf.Lerp(startSize.x, targetSize.x, percentComplete);
        //    float newHeight = Mathf.Lerp(startSize.y, targetSize.y, percentComplete);
            
        //    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        //    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        //    yield return null;

        //    elapsedTime += Time.deltaTime;
        //}

        //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize.x);
        //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize.y);
    }

    private IEnumerator ResizeAxis(RectTransform.Axis axis, float startSize, float targetSize, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {            
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float realDeltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += realDeltaTime;

            float percentComplete = elapsedTime / duration;
            float newSize = Mathf.Lerp(startSize, targetSize, percentComplete);

            rect.SetSizeWithCurrentAnchors(axis, newSize);
        }

        rect.SetSizeWithCurrentAnchors(axis, targetSize);
    }
}
