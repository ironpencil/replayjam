using UnityEngine;
using System.Collections;

public class PixelPerfectSprite : MonoBehaviour
{

    public Transform parentTransform;

    // Use this for initialization
    void Start()
    {
        if (parentTransform == null)
        {
            parentTransform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Vector2 position = parentTransform.position;

        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);

        transform.position = position;
    }
}
