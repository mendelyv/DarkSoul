using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : KeyboardInput
/// description : 玩家输入控制器
/// time : 2018.8.13
/// @author : 杨浩然
/// </summary>
public class KeyboardInput : MonoBehaviour {


    [Header("===== Keyboard Settings =====")]
    public string axisX = "Horizontal";
    public string axisY = "Vertical";
    public string mouseX = "Mouse X";
    public string mouseY = "Mouse Y";
    public string keyJump = "space";
    public string keyRun = "left shift";
    public string keyAttack = "k";

    [Header("===== Output Signal =====")]
    public float dUp;//前后方向的移动量，随键盘按键时间的长短从0增加到1
    public float dRight;//左右方向的移动量，随键盘按键时间的长短从0增加到1

    public float jUp;//前后箭头按键的点按量
    public float jRight;//左右箭头的点按量

    public float Dirmag;//两个方向键同时按下时的斜向行走量，注意根号2的问题
    public Vector3 planarVec;//行走方向向量

    // press signal
    public bool run = false;

    // trigger once signal
    public bool jump = false;
    public bool lastJump = false;
    public bool attack = false;
    public bool lastAttack = false;

    // double trigger

    [Header("===== Others =====")]
    public bool interactive = true;//是否可交互，控制模块的软开关
    private float targetDirForward;//前后移动方向的目标值
    private float targetDirRight;//左右
    private float velocityForward;
    private float velocityRight;

	
	// Update is called once per frame
	void Update () {

        //转动摄像机的输入量
        jUp = Input.GetAxis(mouseY);
        jRight = Input.GetAxis(mouseX);

        targetDirForward = Input.GetAxis(axisY);
        targetDirRight = Input.GetAxis(axisX);

        if (!interactive)//使用软开关控制该模块是否可以使用，直接将组件勾选掉缺陷就是当重新开启时组件中的数据会有可能混乱
        {
            targetDirForward = 0.0f;
            targetDirRight = 0.0f;
        }

        //按方向键的输入量
        dUp = Mathf.SmoothDamp(dUp, targetDirForward, ref velocityForward, 0.1f);
        dRight = Mathf.SmoothDamp(dRight, targetDirRight, ref velocityRight, 0.1f);

        //方形数据转圆形，解决斜方向根号2的问题
        Vector2 tempDirAxis = RectToCircle(new Vector2(dRight, dUp));
        float _dirForward = tempDirAxis.y;
        float _dirRight = tempDirAxis.x;

        //计算多方向键入的位移量
        Dirmag = Mathf.Sqrt((_dirForward * _dirForward) + (_dirRight * _dirRight));

        //计算多方向键入的方向向量，比如 w a 一起按为左前方
        planarVec = _dirRight * transform.right + _dirForward * transform.forward;

        run = Input.GetKey(keyRun);

        bool tempJump = Input.GetKey(keyJump);
        if (tempJump != lastJump && tempJump)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = tempJump;

        bool tempAttack = Input.GetKey(keyAttack);
        //bool tempAttack = Input.GetMouseButtonDown(0);
        if (tempJump != lastAttack && tempAttack)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastAttack = tempAttack;

    }


    private Vector2 RectToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }


//class end
}
