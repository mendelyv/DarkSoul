using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : WeaponManager
/// description : 负责武器开关
/// /// time : 2018.8.28
/// @author : 杨浩然
/// </summary>
public class WeaponManager : IActorManagerInterface {

    private Collider weaponColL;
    private Collider weaponColR;

    //public ActorManager am;
    public GameObject whR;
    public GameObject whL;

    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;
        weaponColR = whR.GetComponentInChildren<Collider>();
        weaponColL = whL.GetComponentInChildren<Collider>();
    }

    public void WeaponEnable()
    {
        if (am.ac.CheckStatuTag("attackR"))
            weaponColR.enabled = true;
        else if (am.ac.CheckStatuTag("attackL"))
            weaponColL.enabled = true;
    }

    public void WeaponDisable()
    {
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }


}
