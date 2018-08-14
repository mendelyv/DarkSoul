using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : KeyboardInput
/// description : 玩家输入控制器
/// time : 2018.8.13
/// @author : 杨浩然
/// </summary>
public class KeyboardInput : IUserInput {


    [Header("===== Keyboard Settings =====")]
    public string axisX = "Horizontal";
    public string axisY = "Vertical";
    public string mouseX = "Mouse X";
    public string mouseY = "Mouse Y";
    public string keyJump = "space";
    public string keyRun = "left shift";
    public string keyAttack = "k";
    public string keyDefense = "e";
	
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

        defense = Input.GetKey(keyDefense);

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





//class end
}
