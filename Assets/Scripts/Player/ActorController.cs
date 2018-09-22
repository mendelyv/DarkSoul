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
    public CameraController cameraController;
    public IUserInput pInput;
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
    //追踪方向，用于摄像机锁定时的跳跃以及翻滚按照planarVec的方向
    public bool trackDirection = false;

    //状态机动画图层index
    //private int attackLayerIndex;

    public bool leftIsShield = true;

	void Awake () {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if(input.enabled)
            {
                pInput = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //attackLayerIndex = anim.GetLayerIndex("Attack");
	}
	

	void Update () {

        //                如果角色的下落量较大，就播放前滚翻
        if(pInput.roll || rig.velocity.magnitude > 5.0f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if(pInput.lockon)
        {
            cameraController.LockUnlock();
        }

        if(!cameraController.lockState)
        {
            anim.SetFloat("forward", pInput.Dirmag * Mathf.Lerp(anim.GetFloat("forward"), ((pInput.run) ? 2.0f : 1.0f), 0.3f));
            anim.SetFloat("right", 0.0f);
        }
        else
        {
            Vector3 localVec = transform.InverseTransformVector(pInput.planarVec);
            anim.SetFloat("forward",localVec.z * ((pInput.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localVec.x * ((pInput.run) ? 2.0f : 1.0f));            
        }
        //anim.SetBool("defense", pInput.defense);

        if(leftIsShield)
        {
            if (CheckStatu("Ground") || CheckStatu("blocked"))
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
                anim.SetBool("defense", pInput.defense);
            }
            else
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"),0);
        }

        if (pInput.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        //                    检查在地面防止空中攻击，检查动画tag触发连击
        if ((pInput.rb || pInput.lb) && (CheckStatu("Ground") || CheckStatuTag("attackR") || CheckStatuTag("attackL")) && canAttack)
        {
            //分左右手攻击输入
            if(pInput.rb)
            {
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            else if(pInput.lb && !leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }

        }

        if(!cameraController.lockState)//不是锁定状态自由移动
        {
            if(pInput.Dirmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pInput.planarVec, 0.25f); ;
            }
            if(!lockPlanar)
                movingVec = pInput.Dirmag * model.transform.forward * walkSpeed * ((pInput.run)?runMultiplier:1.0f);

        }
        else
        {
            if (!trackDirection)
                model.transform.forward = transform.forward;
            else
                model.transform.forward = movingVec.normalized;
            if(!lockPlanar)
            {
                movingVec = pInput.planarVec * walkSpeed * ((pInput.run) ? runMultiplier : 1.0f);
            }
        }

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

    public bool CheckStatuTag(string tagName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }

    //由动画状态机上的FSMOnEnter类调用
    public void OnJumpEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        trackDirection = true;
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
        trackDirection = false;
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
        trackDirection = true;
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
        //anim.SetLayerWeight(attackLayerIndex, Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), lerpTarget, 0.4f));
    }

    public void OnAttack1hAEnter()
    {
        pInput.interactive = false;
        //lerpTarget = 1.0f;
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnHitEnter()
    {
        pInput.interactive = false;
        pInput.planarVec = Vector3.zero;
    }

    public void OnBlockedEnter()
    {
        pInput.interactive = false;
    }

    public void OnDeathEnter()
    {
        pInput.interactive = false;
        pInput.planarVec = Vector3.zero;
    }


    /// <summary>
    /// 拿出动画的root motion位移量
    /// </summary>
    /// <param name="_deltaPos"></param>
    public void OnRootMotionUpdate(object _deltaPos)
    {
        if (CheckStatu("Attack1hC"))
        {
            //这里注意变量的装箱拆箱操作
            deltaPos += (deltaPos + (Vector3)_deltaPos) / 2.0f;//做位移量模糊处理
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

//class end
}

