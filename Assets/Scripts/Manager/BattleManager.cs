using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]//这行特性  会让这个脚本在挂载场景物体时  如果没有这个组件就加载一个
public class BattleManager : MonoBehaviour {

    private CapsuleCollider defCol;
    public ActorManager am;

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
        //Debug.Log(other.name);
        if (other.tag.Equals("Weapon"))
        {
            am.DoDamage();
        }
    }


}
