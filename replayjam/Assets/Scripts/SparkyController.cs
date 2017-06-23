using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkyController : MonoBehaviour {

    SliderJoint2D sj2d;

    public SparkySlider attachedSlider;

    public bool attachToSlider = false;
    Rigidbody2D rb2d;

    public bool randomVelocity = false;

    // Use this for initialization
    void Start()
    {
        sj2d = gameObject.GetComponent<SliderJoint2D>();
        if (sj2d == null)
        {
            Debug.LogWarning(gameObject.name + ":SparkyController does not have a SliderJoin2D attached.");
            this.enabled = false;
        }
        sj2d.enabled = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (attachToSlider)
        {
            AttachToSlider(attachedSlider, transform.position);
            attachToSlider = false;
        }

        if (randomVelocity)
        {
            if (!(rb2d.velocity.magnitude > 0))
            {
                float x = Random.Range(-1.0f, 1.0f);
                float y = Random.Range(-1.0f, 1.0f);
                rb2d.velocity = new Vector2(x, y) * 2;
            }
        } else
        {
            rb2d.velocity = Vector2.zero;
        }
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;

        SparkySlider slider = other.GetComponent<SparkySlider>();

        if (slider != null)
        {
            AttachToSlider(slider, transform.position);
        }
    }

    public void AttachToSlider(SparkySlider slider, Vector3 attachPos)
    {
        Vector2 point1 = slider.point1.position;
        Vector2 point2 = slider.point2.position;

        /*Vector3 sliderNormal = (point2 - point1).normalized;

        //place our object on the line by projecting our current point along the vector from A->B
        Vector3 v1 = attachPos - point1;
        Vector3 v2 = Vector3.Project(v1, sliderNormal);

        Vector3 attachedPos = point1 + v2;

        transform.position = attachedPos;
        */

        Vector2 connectedAnchor = slider.anchor.localPosition; //offset from SparkySlider
        Vector2 distance = point2 - point1;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg; //angle from point1 -> point2

        Vector2 currentPos = transform.position;
        Vector2 point1Dir = point1 - currentPos;
        Vector2 point2Dir = point2 - currentPos;

        float dot1 = Vector2.Dot(rb2d.velocity, point1Dir);
        float dot2 = Vector2.Dot(rb2d.velocity, point2Dir);

        Vector2 targetPos;

        if (dot1 == 0)
        {
            //we're at a right angle, move towards farthest point
            float dist1 = point1Dir.sqrMagnitude;
            float dist2 = point2Dir.sqrMagnitude;

            if (dist1 > dist2)
            {
                targetPos = point1;
            }
            else
            {
                targetPos = point2;
            }
        } else
        {
            if (dot1 > dot2)
            {
                targetPos = point1;
                //move towards dot1
            } else
            {
                targetPos = point2;
                //move towards dot2
            }
        }

        //set up translation limits
        Vector2 targetAnchor = slider.anchor.position;
        float limit1 = (targetAnchor - point1).magnitude;
        float limit2 = (targetAnchor - point2).magnitude * -1;

        sj2d.connectedBody = slider.gameObject.GetComponent<Rigidbody2D>();
        sj2d.connectedAnchor = connectedAnchor;
        sj2d.angle = angle - transform.eulerAngles.z;

        JointTranslationLimits2D jtl2d = new JointTranslationLimits2D();
        jtl2d.min = limit2;
        jtl2d.max = limit1;
        sj2d.limits = jtl2d;

        sj2d.enabled = true;





        //may not need this later?
        //Vector2 moveDirection = (targetPos - (Vector2)currentPos).normalized;

        //float speed = rb2d.velocity.magnitude;

        //rb2d.velocity = moveDirection * speed;  

    }
}
