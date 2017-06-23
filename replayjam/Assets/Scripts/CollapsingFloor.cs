using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingFloor : MonoBehaviour {

    public GameObject pitPrefab;

    public bool doCollapse = false;

    public float collapseDuration = 1.0f;
    public float collapseMaxRotation = 360.0f;
    SpriteRenderer floorSprite;
    public Sprite floorImage;

    public SoundEffectHandler collapseSound;

    bool doDestroy = false;

	// Use this for initialization
	void Start () {
        floorSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        floorSprite.sprite = floorImage;
	}
	
	// Update is called once per frame
	void Update () {
		if (doCollapse)
        {
            doCollapse = false;
            Collapse();
        }

        if (doDestroy)
        {
            Destroy(gameObject);
        }
	}

    public void Collapse()
    {
        StartCoroutine(DoCollapse());
    }

    IEnumerator DoCollapse()
    {
        float startTime = Time.time;
        float elapsed = 0.0f;
        float duration = collapseDuration;

        if (collapseSound != null)
        {
            collapseSound.PlayEffect();
        }

        GameObject pit = GameObject.Instantiate(pitPrefab, transform);
        pit.transform.parent = transform.parent;
        PitController pitController = pit.GetComponent<PitController>();
        pitController.ActivatePit(false);

        Vector2 floorScale = floorSprite.transform.localScale;
        float randomRotation = Random.Range(collapseMaxRotation * -1, collapseMaxRotation);
        Vector3 floorAngles = floorSprite.transform.eulerAngles;
        Vector3 finalAngles = floorAngles;
        finalAngles.z = randomRotation;

        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            floorSprite.transform.localScale = Vector2.Lerp(floorScale, Vector2.zero, elapsed / duration);
            floorSprite.transform.eulerAngles = Vector3.Lerp(floorAngles, finalAngles, elapsed / duration);
            yield return new WaitForSeconds(0.1f);
        }

        pitController.ActivatePit(true);
        
    }
}
