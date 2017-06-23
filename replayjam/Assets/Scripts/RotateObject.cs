using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{

    public Transform target;

    public bool doRotate = true;

    public float startingRotation = 0.0f;

    public float rotationPerSecond = 10.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DoRotation();
    }

    public void DoRotation()
    {
        if (doRotate && target != null)
        {
            target.Rotate(new Vector3(0.0f, 0.0f, rotationPerSecond * Time.deltaTime));
        }
    }

    public void ResetRotation()
    {
        if (target != null)
        {
            target.rotation = Quaternion.identity;
            if (startingRotation != 0.0f)
            {
                target.Rotate(new Vector3(0.0f, 0.0f, startingRotation));
            }
        }
    }


}
