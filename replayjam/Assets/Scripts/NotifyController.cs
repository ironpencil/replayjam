using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyController : MonoBehaviour {
    public float notifyTime;
    private float startTime;
    private float endTime;
    public Vector2 startPos;
    public Vector2 endPos;
    public AnimationCurve floatRate;
    public AnimationCurve fadeRate;
    SpriteRenderer sprite;
    Transform parent;

	// Use this for initialization
	void Start () {
        sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float percentComplete = (Time.time - startTime) / notifyTime;
        // Once we get to the shadow, blow up
        if (Time.time < startTime + notifyTime)
        {
            float floatPercent = floatRate.Evaluate(percentComplete);
            transform.localPosition = Vector2.Lerp(startPos, endPos, floatPercent);

            Color newColor = sprite.color;
            float fadePercent = fadeRate.Evaluate(percentComplete);
            newColor.a = Mathf.Lerp(1f, 0f, fadePercent);
            sprite.color = newColor;
        }
    }

    public void Notify()
    {
        transform.localPosition = new Vector2(0.0f, 0.0f);
        Color color = sprite.color;
        color.a = 255;
        sprite.color = color;
        startTime = Time.time;
        endTime = startTime + notifyTime;
    }
}
