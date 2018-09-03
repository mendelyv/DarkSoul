using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : StateManager
/// description : 人物状态管理员，负责人物的属性
/// /// time : 2018.8.31
/// @author : 杨浩然
/// </summary>
public class StateManager : IActorManagerInterface {

    public float HPMax = 15.0f;
    public float HP = 15.0f;

    private void Start()
    {
        //HP = Mathf.Clamp(HP, 0, HPMax);
        HP = HPMax;
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);

        if(HP > 0)
        {
            am.Hit();
        }
        else
        {
            am.Die();
        }
    }

//end class
}
