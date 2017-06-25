using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitBehavior : MonoBehaviour {
    
    public List<Sprite> glassStages;

    private int damage = 0;

    private RectTransform rt;

    public Image glass;

    public RectTransform container;

    // Slide Variables
    private float startTime;
    public Vector2 startPos;
    public Vector2 endPos;
    public float minSlideInTime;
    public float maxSlideInTime;
    public float minSlideOutTime;
    public float maxSlideOutTime;
    private float slideInTime;
    private float slideOutTime;
    public AnimationCurve slideInCurve;
    public AnimationCurve slideOutCurve;
    public bool slidingIn;
    
    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();
	}
	
    public void TakeDamage()
    {
        if (glassStages.Count > damage)
        {
            glass.sprite = glassStages[damage++];
        }
        glass.color = Color.white;
    }

    public void ResetDamage()
    {
        damage = 0;
        glass.sprite = null;
        glass.color = Color.clear;
    }

    public void SlideIn()
    {
        ResetDamage();
        startTime = Time.time;
        slideInTime = UnityEngine.Random.Range(minSlideInTime, maxSlideInTime);
        slidingIn = true;
        gameObject.GetComponent<GameObjectShake>().enabled = false;
    }

    public void SlideOut()
    {
        startTime = Time.time;
        slideOutTime = UnityEngine.Random.Range(minSlideOutTime, maxSlideOutTime);
        slidingIn = false;
        gameObject.GetComponent<GameObjectShake>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (slidingIn)
        {
            float percent = slideInCurve.Evaluate((Time.time - startTime) / slideInTime);
            Vector2 newPos = startPos + ((endPos - startPos) * percent); //lerp is capped at 1
            container.anchoredPosition = newPos; //Vector2.Lerp(startPos, endPos, percent);

            if (percent == 1) gameObject.GetComponent<GameObjectShake>().enabled = true;
        } else
        {
            float percent = slideOutCurve.Evaluate((Time.time - startTime) / slideOutTime);
            container.anchoredPosition = Vector2.Lerp(endPos, startPos, percent);
        }
        
	}
}
