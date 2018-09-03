using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : BattleManager
/// description : 负责检测碰撞
/// /// time : 2018.8.28
/// @author : 杨浩然
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]//这行特性  会让这个脚本在挂载场景物体时  如果没有这个组件就加载一个
public class BattleManager : IActorManagerInterface {

    private CapsuleCollider defCol;
    //public ActorManager am;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = new Vector3(0.0f,1.0f,0.0f);
        defCol.height = 2.0f;
        defCol.radius = 0.25f;
        defCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            am.TryDoDamage();
        }
    }


}
