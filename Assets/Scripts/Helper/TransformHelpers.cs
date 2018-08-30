using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : TransformHelpers
/// description : 节点选择辅助类
/// time : 2018.8.30
/// @author : 杨浩然
/// </summary>
public static class TransformHelpers  {
    /// <summary>
    /// 层层筛选Transform下相同名字的，找到第一个就返回
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="targetName"></param>
    public static Transform DeepFind(this Transform parent,string targetName)
    {
        Transform retTrans;
        foreach  (Transform child in parent)
        {
            if(child.name.Equals(targetName))
            {
                retTrans = child;
                return child;
            }
            else
            {
                retTrans = child.DeepFind(targetName);
                if(retTrans != null)
                {
                    return retTrans;
                }
            }
        }
        return null;
    }

}
