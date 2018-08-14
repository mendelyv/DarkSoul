using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : IUserInput
/// description : 玩家输入控制器 基类 抽取控制器的公有变量及方法，方便其他设备接入时的控制
/// time : 2018.8.13
/// @author : 杨浩然
/// </summary>
public abstract class IUserInput : MonoBehaviour {

    [Header("===== Output Signal =====")]
    public float dUp;//前后方向的移动量，随键盘按键时间的长短从0增加到1
    public float dRight;//左右方向的移动量，随键盘按键时间的长短从0增加到1

    public float jUp;//前后箭头按键的点按量
    public float jRight;//左右箭头的点按量

    public float Dirmag;//两个方向键同时按下时的斜向行走量，注意根号2的问题
    public Vector3 planarVec;//行走方向向量

    // press signal
    public bool run = false;
    public bool defense;

    // trigger once signal
    public bool jump = false;
    public bool lastJump = false;
    public bool attack = false;
    public bool lastAttack = false;

    // double trigger

    [Header("===== Others =====")]
    public bool interactive = true;//是否可交互，控制模块的软开关

    protected float targetDirForward;//前后移动方向的目标值
    protected float targetDirRight;//左右
    protected float velocityForward;
    protected float velocityRight;


    protected Vector2 RectToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

//class end
}
