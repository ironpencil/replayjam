using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitBehavior : MonoBehaviour {
    
    public List<Sprite> glassStages;
    public Sprite damagedPortrait;

    public List<Sprite> idlePortraits;
    private List<AnimationStage> idleAnimationStages = new List<AnimationStage>();
    private AnimationStage currentAnimationStage;

    private int damage = 0;
    public int playerNum = 0;

    private bool idle = true;

    private RectTransform rt;

    public Image glass;
    public Image character;

    public RectTransform container;

    public const int RED_PLAYER_NUM = 1;
    public const int YELLOW_PLAYER_NUM = 2;
    public const int BLUE_PLAYER_NUM = 3;
    public const int GREEN_PLAYER_NUM = 4;
   
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
    bool finishedSliding = true;
    float nextFrame = 0.0f;
    
    private class AnimationStage
    {
        public int imageIndex;
        public int nextIndex;
        public float time;

        public AnimationStage(int imageIndex, int nextIndex, float time)
        {
            this.imageIndex = imageIndex;
            this.nextIndex = nextIndex;
            this.time = time;
        }
    }
    
    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();

        switch (playerNum)
        {
            case RED_PLAYER_NUM:
                idleAnimationStages.Add(new AnimationStage(0, 1, 0.4f));
                idleAnimationStages.Add(new AnimationStage(1, 2, 0.4f));
                idleAnimationStages.Add(new AnimationStage(0, 3, 0.4f));
                idleAnimationStages.Add(new AnimationStage(1, 4, 0.4f));
                idleAnimationStages.Add(new AnimationStage(0, 5, 0.4f));
                idleAnimationStages.Add(new AnimationStage(1, 6, 0.4f));
                idleAnimationStages.Add(new AnimationStage(0, 7, 0.4f));
                idleAnimationStages.Add(new AnimationStage(1, 8, 0.4f));
                idleAnimationStages.Add(new AnimationStage(2, 0, 0.5f));
                break;
            case YELLOW_PLAYER_NUM:
                idleAnimationStages.Add(new AnimationStage(0, 1, 0.3f));
                idleAnimationStages.Add(new AnimationStage(1, 2, 0.3f));
                idleAnimationStages.Add(new AnimationStage(2, 3, 0.5f));
                idleAnimationStages.Add(new AnimationStage(1, 4, 0.3f));
                idleAnimationStages.Add(new AnimationStage(0, 0, 0.3f));
                break;
            case BLUE_PLAYER_NUM:
                idleAnimationStages.Add(new AnimationStage(0, 1, 3.0f));
                idleAnimationStages.Add(new AnimationStage(1, 2, 1.0f));
                idleAnimationStages.Add(new AnimationStage(2, 3, 1.0f));
                idleAnimationStages.Add(new AnimationStage(1, 0, 1.0f));
                break;
            case GREEN_PLAYER_NUM:
                idleAnimationStages.Add(new AnimationStage(0, 1, 2.0f));
                idleAnimationStages.Add(new AnimationStage(1, 2, 0.2f));
                idleAnimationStages.Add(new AnimationStage(2, 3, 0.7f));
                idleAnimationStages.Add(new AnimationStage(1, 0, 0.2f));
                break;
        }
    }

    IEnumerator AnimateDamage()
    {
        idle = false;
        character.sprite = damagedPortrait;
        yield return new WaitForSecondsRealtime(1.0f);
        idle = true;
    }

    public void TakeDamage()
    {
        damage++;

        if (damage < glassStages.Count)
        {
            glass.sprite = glassStages[damage];
        }
        StartCoroutine(AnimateDamage());
    }

    public void ResetDamage()
    {
        damage = 0;
        glass.sprite = glassStages[0];
        idle = true;
        currentAnimationStage = null;
    }

    public void SlideIn()
    {
        if (!slidingIn)
        {
            finishedSliding = false;
            ResetDamage();
            startTime = Time.time;
            slideInTime = UnityEngine.Random.Range(minSlideInTime, maxSlideInTime);
            slidingIn = true;
            gameObject.GetComponent<GameObjectShake>().enabled = false;
        }
    }

    public void SlideOut()
    {
        if (slidingIn)
        {
            finishedSliding = false;
            startTime = Time.time;
            slideOutTime = UnityEngine.Random.Range(minSlideOutTime, maxSlideOutTime);
            slidingIn = false;
            gameObject.GetComponent<GameObjectShake>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (idle && Time.time > nextFrame)
        {
            if (currentAnimationStage == null)
            {
                currentAnimationStage = idleAnimationStages[0];
                character.sprite = idlePortraits[0];
            }
            else
            {
                currentAnimationStage = idleAnimationStages[currentAnimationStage.nextIndex];
                character.sprite = idlePortraits[currentAnimationStage.imageIndex];
            }
            nextFrame = Time.time + currentAnimationStage.time;
        }

        if (!finishedSliding)
        {
            if (slidingIn)
            {
                float percent = slideInCurve.Evaluate((Time.time - startTime) / slideInTime);
                Vector2 newPos = startPos + ((endPos - startPos) * percent); //lerp is capped at 1
                container.anchoredPosition = newPos; //Vector2.Lerp(startPos, endPos, percent);

                if (percent == 1)
                {
                    gameObject.GetComponent<GameObjectShake>().enabled = true;
                    finishedSliding = true;
                }
            }
            else
            {
                float percent = slideOutCurve.Evaluate((Time.time - startTime) / slideOutTime);
                container.anchoredPosition = Vector2.Lerp(endPos, startPos, percent);

                if (percent == 1)
                {
                    finishedSliding = true;
                    transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
