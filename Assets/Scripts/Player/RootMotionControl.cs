using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : RootMotionControl
/// description : 动画 root motion 采集器
/// time : 2018.7.5
/// @author : 杨浩然
/// </summary>
public class RootMotionControl : MonoBehaviour {

    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnRootMotionUpdate", (object)anim.deltaPosition);
    }

//class end
}
