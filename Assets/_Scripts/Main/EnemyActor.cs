/*文件名：EnemyActor.cs
 * 作者：YZY
 * 说明：敌人
 * 上次修改时间：2020/2/28 18：49：32 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;
public class EnemyActor : VividActor
{
    #region Parameters

    #endregion
    #region Properties
    #endregion
    #region Private Methods   

    protected override void PlayAnim(string _animName)
    {
        
    }
    protected override void ChangeSceneNodes(MoveDir _dir)
    {
        base.ChangeSceneNodes(_dir);
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(Vec2Pos, SceneActorType.Enemy);
    }
    #endregion
    #region Utility Methods
    public override void Init(SceneActorType _type)
    {
        curPlayerArmState =(PlayerArmState) Random.Range(0, 3);
        base.Init(_type);
    }
    //每回合执行一个行为
    public virtual IEnumerator IE_OnceAction()
    {
        //获取下一步的方向
        var _nextPos = AStarTool.GetNextPos( LinkInstance.Instance.MainPlayer.Vec2Pos,Vec2Pos);
        var _dir = Vec2Pos.GetDir(_nextPos, curDir);
        var _type = LinkInstance.Instance.SceneManager.GetSceneNodeTypeByVec2(Vec2Pos.GetVec2ToDir(_dir));
        switch (_type)
        {
            case SceneActorType.Null:
                //如果是弓箭手，判断前方3格内是否有player，否则就移动
                //移动
                yield return StartCoroutine(IE_Move(_dir));
                
                break;
            case SceneActorType.Enemy:                         
                //只是转方向
                yield return StartCoroutine(IE_Rot(_dir));
                break;         
            case SceneActorType.Follow:
            case SceneActorType.Player:
                //攻击
                break;
        }
    }
    #endregion
}
