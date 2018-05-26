using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public bool interactive = true;//是否可交互，控制模块的软开关

    #region====变量区

    public string keyForward = "w";
    public string keyBack = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public float dirForward;//前后方向的移动量
    public float dirRight;//左右方向的移动量

    public float Dirmag;//行走量
    public Vector3 Dirvec;//行走方向向量

    private float targetDirForward;//前后移动方向的目标值
    private float targetDirRight;//左右
    private float velocityForward;
    private float velocityRight;

    #endregion

    void Start () {
        
	}
	
	void Update () {

        targetDirForward = (Input.GetKey(keyForward) ? 1.0f : 0.0f) - (Input.GetKey(keyBack) ? 1.0f : 0.0f);
        targetDirRight = (Input.GetKey(keyRight) ? 1.0f : 0.0f) - (Input.GetKey(keyLeft) ? 1.0f : 0.0f);

        if( ! interactive)//使用软开关控制该模块是否可以使用，直接将组件勾选掉缺陷就是当重新开启时组件中的数据会有可能混乱
        {
            targetDirForward = 0.0f;
            targetDirRight = 0.0f;
        }

        dirForward = Mathf.SmoothDamp(dirForward, targetDirForward, ref velocityForward, 0.1f);
        dirRight = Mathf.SmoothDamp(dirRight, targetDirRight, ref velocityRight, 0.1f);

        Dirmag = Mathf.Sqrt((dirForward * dirForward) + (dirRight * dirRight));
        Dirvec = dirRight * transform.right + dirForward * transform.forward;

	}

    
}
