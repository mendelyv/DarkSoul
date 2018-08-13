using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class name : PlayerInput
/// description : 玩家输入控制器
/// time : 2018.7.2
/// @author : 杨浩然
/// </summary>
public class PlayerInput : IUserInput
{

    #region====变量区

    [Header("===== Key Setting =====")]
    public string keyForward = "w";
    public string keyBack = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    //键盘的方向键
    public string upArrow = "up";
    public string downArrow = "down";
    public string leftArrow = "left";
    public string rightArrow = "right";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;




    #endregion


	void Update () {

        //转动摄像机的输入量
        jUp = (Input.GetKey(upArrow) ? 1.0f : 0.0f) - (Input.GetKey(downArrow) ? 1.0f : 0.0f);
        jRight = (Input.GetKey(rightArrow) ? 1.0f : 0.0f) - (Input.GetKey(leftArrow) ? 1.0f : 0.0f);        

        targetDirForward = (Input.GetKey(keyForward) ? 1.0f : 0.0f) - (Input.GetKey(keyBack) ? 1.0f : 0.0f);
        targetDirRight = (Input.GetKey(keyRight) ? 1.0f : 0.0f) - (Input.GetKey(keyLeft) ? 1.0f : 0.0f);

        if( ! interactive)//使用软开关控制该模块是否可以使用，直接将组件勾选掉缺陷就是当重新开启时组件中的数据会有可能混乱
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

        run = Input.GetKey(keyA);

        bool tempJump = Input.GetKey(keyB);
        if(tempJump != lastJump && tempJump)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = tempJump;

        bool tempAttack= Input.GetKey(keyC);
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
    
}
