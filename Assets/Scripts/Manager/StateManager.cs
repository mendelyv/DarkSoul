using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : StateManager
/// description : 人物状态管理员，负责人物的属性
/// time : 2018.8.31
/// @author : 杨浩然
/// </summary>
public class StateManager : IActorManagerInterface {

    public float HPMax = 15.0f;
    public float HP = 15.0f;

    [Header("===== first order state flags =====")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;

    [Header("===== second order state flags =====")]
    public bool isAllowDefense;

    private void Start()
    {
        //HP = Mathf.Clamp(HP, 0, HPMax);
        HP = HPMax;
    }

    private void Update()
    {
        isGround = am.ac.CheckStatu("Ground");
        isJump = am.ac.CheckStatu("Jump");
        isFall = am.ac.CheckStatu("Falling");
        isRoll = am.ac.CheckStatu("Roll");
        isJab = am.ac.CheckStatu("Jab");
        isAttack = am.ac.CheckStatuTag("attackR") || am.ac.CheckStatuTag("attackL");
        isHit = am.ac.CheckStatu("hit");
        isDie = am.ac.CheckStatu("death");
        isBlocked = am.ac.CheckStatu("blocked");
        //isDefense = am.ac.CheckStatu("defense1h","defense");

        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckStatu("defense1h", "defense");
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);

    }

//end class
}
