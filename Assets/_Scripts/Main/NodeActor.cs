/*文件名：NodeActor.cs
 * 作者：YZY
 * 说明：在场景中所有对象都继承自它：主角，墙，宝箱，敌人;【墙直接就用该脚本】
 * 上次修改时间：2020/3/4 21：42：55 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeActor : MonoBehaviour
{
    Vector2Int vec2Pos;
    SceneActorType thisActorType;
    public Vector2Int Vec2Pos
    {
        get
        {
            return vec2Pos;
        }

        set
        {
            vec2Pos = value;
        }
    }

    public SceneActorType ThisActorType
    {
        get
        {
            return thisActorType;
        }

        set
        {
            thisActorType = value;
        }
    }

    public virtual void Init(SceneActorType _type)
    {        
        ThisActorType = _type;
    }
    public void SetPos(Vector2Int _pos)
    {
        vec2Pos = _pos;
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(this, vec2Pos, ThisActorType);
    }
}
