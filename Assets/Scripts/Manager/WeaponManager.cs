using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    private Collider weaponCol;

    public ActorManager am;
    public GameObject whR;
    public GameObject whL;

    private void Start()
    {
        weaponCol = whR.GetComponentInChildren<Collider>();
        Debug.Log(transform.DeepFind("weaponHandleR"));
    }

    public void WeaponEnable()
    {
        weaponCol.enabled = true;
    }

    public void WeaponDisable()
    {
        weaponCol.enabled = false;
    }


}
