using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStickDemo : MonoBehaviour {

    public float minRotation = -90.0f;
    public float maxRotation = 90.0f;

    public float loopTime = 2.0f;

    bool startLoop = true;

    public float rotDistance = 1.0f;

    public Transform stick;

    public DemoPlayer demoPlayer;
    public bool moveInput = true;
    public bool aimInput = false;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        startLoop = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (startLoop)
        {
            startLoop = false;
            StartCoroutine(DoRotationLoop());
        }

        if (demoPlayer != null)
        {
            Vector2 input = GetStickPosition();
            if (moveInput)
            {
                demoPlayer.moveInput = input;
            }

            if (aimInput)
            {
                demoPlayer.aimInput = input;
            }
        }
	}

    IEnumerator DoRotationLoop()
    {
        float elapsedTime = 0.0f;

        float rotateTime = loopTime * 0.5f;

        float currRot = minRotation;

        stick.localPosition = DegreeToVector(currRot) * rotDistance;

        while (elapsedTime < rotateTime)
        {
            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;

            currRot = Mathf.Lerp(minRotation, maxRotation, elapsedTime / rotateTime);

            stick.localPosition = DegreeToVector(currRot) * rotDistance;
        }

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0.0f;

        currRot = maxRotation;

        stick.localPosition = DegreeToVector(currRot) * rotDistance;

        while (elapsedTime < rotateTime)
        {
            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;

            currRot = Mathf.Lerp(maxRotation, minRotation, elapsedTime / rotateTime);

            stick.localPosition = DegreeToVector(currRot) * rotDistance;
        }

        yield return new WaitForSeconds(0.1f);

        startLoop = true;

    }

    Vector2 DegreeToVector(float degree)
    {
        return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }

    public Vector2 GetStickPosition()
    {
        return stick.localPosition;
    }

}
