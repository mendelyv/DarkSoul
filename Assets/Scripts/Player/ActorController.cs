using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class name : ActorController
/// description : 动作控制器
/// time : 2018.7.2
/// @author : 杨浩然
/// </summary>
public class ActorController : MonoBehaviour {

    //模型
    public GameObject model;
    public PlayerInput pInput;
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;
    //跳跃冲量
    public float jumpVelocity = 3.0f;
    //翻滚冲量
    public float rollVelocity = 1.0f;
    //后跳冲量
    public float jabVelocity = 2.0f;

    [Space(10)]
    [Header("===== Friction Setting =====")]
    public PhysicMaterial friction_ZERO;
    public PhysicMaterial friction_ONE;

    [SerializeField]
    private Animator anim;
    private Rigidbody rig;
    private bool canAttack;

    private Vector3 movingVec;
    //冲量的方向向量
    private Vector3 thrustVec;
    private CapsuleCollider capsuleCollider;

    //用于控制由Base层到Attack层的lerp的目标值
    private float lerpTarget;
    //用于attack1hC动画的 root motion 位移量
    private Vector3 deltaPos;
    //移动锁定
    public bool lockPlanar = false;


    //状态机动画图层index
    private int attackLayerIndex;

	void Awake () {
        anim = model.GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
        rig = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        attackLayerIndex = anim.GetLayerIndex("Attack");
	}
	

	void Update () {
        anim.SetFloat("forward", pInput.Dirmag * Mathf.Lerp(anim.GetFloat("forward"), ((pInput.run) ? 2.0f : 1.0f),0.3f));
        //如果角色的下落量较大，就播放前滚翻
        if (rig.velocity.magnitude > 5.0f)
            anim.SetTrigger("roll");

        if (pInput.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
            
        if (pInput.attack && CheckStatu("Ground") && canAttack)
            anim.SetTrigger("attack");

        if(pInput.Dirmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pInput.planarVec, 0.25f); ;
        }
        if(!lockPlanar)
            movingVec = pInput.Dirmag * model.transform.forward * walkSpeed * ((pInput.run)?runMultiplier:1.0f);
	}

    void FixedUpdate()
    {
        //加入attack1hC的root motion位移量
        rig.position += deltaPos;
        deltaPos = Vector3.zero;
        rig.velocity = new Vector3(movingVec.x, rig.velocity.y, movingVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    /// <summary>
    /// 检查是否是这个状态
    /// </summary>
    /// <param name="stateName">动画状态名字</param>
    /// <param name="layerName">动画图层名字</param>
    /// <returns></returns>
    public bool CheckStatu(string stateName,string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    //由动画状态机上的FSMOnEnter类调用
    public void OnJumpEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
    }



   
    //由传感器发送的信息调用
    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    //由传感器发送的信息调用
    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }


    //由动画状态机上的FSMOnEnter类调用
    public void OnGroundEnter()
    {
        pInput.interactive = true;
        lockPlanar = false;
        canAttack = true;
        capsuleCollider.material = friction_ONE;
    }

    public void OnGroundExit()
    {
        capsuleCollider.material = friction_ZERO;
    }

    //由动画状态机上的FSMOnEnter类调用
    public void OnFallEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
    }

    public  void OnJabEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
    }

    //后跳的冲量由动画曲线实时控制并实时调用
    public void OnJabUpdate()
    {
        //如果直接这样后跳，会一瞬间完成后跳的距离
        //thrustVec = model.transform.forward * (-jabVelocity);
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        //使用差值法使动画图层的权重缓慢增加
        anim.SetLayerWeight(attackLayerIndex, Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), lerpTarget, 0.4f));
    }

    public void OnAttack1hAEnter()
    {
        pInput.interactive = false;
        lerpTarget = 1.0f;
    }

    public void OnAttackLayerIdleEnter()
    {
        pInput.interactive = true;
        lerpTarget = 0.0f;
    }

    public void OnAttackLayerIdleUpdate()
    {
        //使用差值法使动画图层的权重缓慢增加
        anim.SetLayerWeight(attackLayerIndex, Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), lerpTarget, 0.4f));
    }

    public void OnRootMotionUpdate(object _deltaPos)
    {
        if (CheckStatu("Attack1hC", "Attack"))
        {
            //这里注意变量的装箱拆箱操作
            deltaPos += (Vector3)_deltaPos;
        }
    }

//class end
}

