using UnityEngine;
using System.Collections;

public class DamageOnTouch : MonoBehaviour
{

    public float damage = 5.0f;

    public EffectSource damageType = EffectSource.Enemy;

    public int targetLayer = -1;

    public bool bounceWhenDealingDamage = true;

    public float bounceMagnitude = 100.0f;

    protected Collider2D thisCollider;
    protected Rigidbody2D thisRigidbody;

    public bool dealDamageOnStay = false;

    public void Start()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        thisRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (!thisCollider.isTrigger)
        {
            CollideWithObject(coll);
        }
    }

    public void OnCollisionStay2D(Collision2D coll)
    {
        if (!thisCollider.isTrigger && dealDamageOnStay)
        {
            CollideWithObject(coll);
        }
    }

    private void CollideWithObject(Collision2D coll)
    {
        if (targetLayer == -1 || coll.gameObject.layer == targetLayer)
        {
            TakesDamage touched = coll.gameObject.GetComponent<TakesDamage>();

            if (touched != null)
            {
                Collider2D thisCollider = gameObject.GetComponent<Collider2D>();
                bool damageDone = touched.ApplyDamage(damage, damageType, coll, thisCollider);

                if (bounceWhenDealingDamage && damageDone)
                {
                    Vector2 contactPoint = coll.contacts[0].point;

                    Vector2 direction = (contactPoint - (Vector2)transform.position).normalized;

                    thisRigidbody.AddForce(direction * bounceMagnitude, ForceMode2D.Impulse);
                }
            }
        }
    }
}
