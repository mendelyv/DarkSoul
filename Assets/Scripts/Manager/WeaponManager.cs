using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public CapsuleCollider weaponCol;

    public void WeaponEnable()
    {
        weaponCol.enabled = true;
    }

    public void WeaponDisable()
    {
        weaponCol.enabled = false;
    }


}
