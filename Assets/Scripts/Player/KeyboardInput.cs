using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : PlayerInput
/// description : 玩家输入控制器
/// time : 2018.7.2
/// @author : 杨浩然
/// </summary>
public class KeyboardInput : IUserInput
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

    public string keyA = "left shift";
    public string keyB = "space";
    public string keyC = "mouse 0";
    public string keyD = "mouse 1";
    public string keyE = "p";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonE = new MyButton();


    [Header("===== Mouse Setting =====")]
    public bool mouseEnable = true;
    public float mouseSensitivity = 1.0f;

    #endregion


	void Update () {

        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE));

        print(buttonE.IsExtending && buttonE.OnPressed);

        //转动摄像机的输入量
        if (mouseEnable)
        {
            jUp = Input.GetAxis("Mouse Y") * mouseSensitivity;
            jRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        }
        else
        {
            jUp = (Input.GetKey(upArrow) ? 1.0f : 0.0f) - (Input.GetKey(downArrow) ? 1.0f : 0.0f);
            jRight = (Input.GetKey(rightArrow) ? 1.0f : 0.0f) - (Input.GetKey(leftArrow) ? 1.0f : 0.0f);
        }

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
        roll = buttonA.OnReleased && buttonA.IsDelaying;//如果按下去很快松开就翻滚
        //奔跑键按下会有延迟开始跑动，并且松开时还会有跑动的延迟，方便连击
        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        defense = buttonD.IsPressing;
        jump = buttonA.OnPressed && buttonA.IsExtending;
        attack = buttonC.OnPressed;

	}
    
}
