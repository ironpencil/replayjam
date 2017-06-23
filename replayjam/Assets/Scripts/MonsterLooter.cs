using UnityEngine;
using System.Collections;

public class MonsterLooter : MonoBehaviour
{
    public LootHandler monsterLoot;

    public SoundEffectHandler lootSound;

    // Use this for initialization
    void Start()
    {
        if (monsterLoot == null)
        {
            monsterLoot = gameObject.GetComponent<LootHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        Collider2D other = coll.collider;

        Lootable lootable = other.gameObject.GetComponent<Lootable>();

        if (lootable != null && monsterLoot != null)
        {
            monsterLoot.carrying.Add(lootable.gameObject);
            lootable.transform.parent = monsterLoot.transform;
            lootable.gameObject.SetActive(false);

            if (lootSound != null)
            {
                lootSound.PlayEffect();
            }
        }
    }
}
