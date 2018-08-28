using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour {

    private Animator anim;
    private ActorController ac;
    public Vector3 angle = new Vector3(0.0f, -58.0f, 15.0f);

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ac = GetComponentInParent<ActorController>();
    }

    private void OnAnimatorIK()
    {
        if(ac.leftIsShield)
        {
            if (anim.GetBool("defense")) return;   //如果是防御状态就不需要一下代码修正Idle状态的盾牌位置
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArm.localEulerAngles += angle;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));

        }
    }

    //class end
}
