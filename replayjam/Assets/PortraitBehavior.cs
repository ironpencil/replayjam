using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitBehavior : MonoBehaviour {
    
    public List<Image> portraits;
    public List<Image> glassStages;

    private RectTransform rt;

    // Slide Variables
    private float startTime;
    public Vector2 startPos;
    public Vector2 endPos;
    public float minSlideTime;
    public float maxSlideTime;
    private float slideTime;
    public AnimationCurve slideCurve;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        rt = GetComponent<RectTransform>();
        slideTime = UnityEngine.Random.Range(minSlideTime, maxSlideTime);
	}
	
	// Update is called once per frame
	void Update () {
        float percent = slideCurve.Evaluate((Time.time - startTime) / slideTime);
        rt.anchoredPosition = Vector2.Lerp(startPos, endPos, percent);
	}
}
