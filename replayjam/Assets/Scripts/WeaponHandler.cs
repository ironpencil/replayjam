using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour {

    public Weapon currentWeapon;
    public Animator playerAnimator;

    public Vector2 weaponDownPosition;
    public Vector2 weaponUpPosition;
    public Vector2 weaponLeftPosition;
    public Vector2 weaponRightPosition;

	// Use this for initialization
	void Start () {
        //currentWeapon.weaponChargeHandler.weaponChargeBar = Globals.Instance.weaponChargeBar;
        currentWeapon.Equipped = true;
        currentWeapon.attackerAnimator = playerAnimator;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack(Vector2 facing)
    {
        Vector3 rotation = new Vector3();

        if (facing.x > 0)
        {
            rotation.z = 0;
            transform.localPosition = weaponRightPosition;
        }
        else if (facing.x < 0)
        {
            rotation.y = 180;   //rotate on y axis to flip sprite horizontally without changing y position
            transform.localPosition = weaponLeftPosition;
        }
        else if (facing.y > 0)
        {
            rotation.z = 90;
            transform.localPosition = weaponUpPosition;
        }
        else
        {
            rotation.z = -90;
            transform.localPosition = weaponDownPosition;
        }

        transform.localEulerAngles = rotation;

        currentWeapon.Attack();
    }

    public bool IsAttacking()
    {
        return currentWeapon.isAttacking;
    }
}
