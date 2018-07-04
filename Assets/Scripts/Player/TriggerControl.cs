using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : TriggerControl
/// description : 动画状态机的Trigger控制
/// time : 2018.7.4
/// @author : 杨浩然
/// </summary>
public class TriggerControl : MonoBehaviour {

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }

//class end
}
