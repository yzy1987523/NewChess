/*文件名：RoleTemplateActor.cs
 * 作者：YZY
 * 说明：主角模板：主要是作为随从存在
 * 上次修改时间：2020/3/8 18:02:25 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTemplateActor : VividActor
{
    #region Parameters    
    public delegate void NetDelegate(string val);
    public bool hasCheakAction;//确认行为后置为true，执行完后置为false
    public ActionType nextActionType;
    #endregion
    #region Properties

    #endregion
    #region Private Methods  

   
   
    protected override void ChangeSceneNodes(MoveDir _dir)
    {
        base.ChangeSceneNodes(_dir);
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(this, Vec2Pos, SceneActorType.Player);
    }
    protected virtual IEnumerator IE_Interact(MoveDir _dir)
    {
        yield return null;
    }
    //移动后检查周围是否有随从，有就加入到主角的随从list中
    protected void CheckAround(MoveDir _dir)
    {
        for (var i = 0; i < 4; i++)
        {
            var _obj = LinkInstance.Instance.SceneManager.GetNodeByVec2(Vec2Pos.GetVec2ToDir((MoveDir)i));
            if (_obj != null&& _obj.ThisActorType == SceneActorType.Follow) { 
                var _role= (RoleTemplateActor)_obj;
                if (!LinkInstance.Instance.MainPlayer.RoleTemplateActors.Contains(_role)) {
                    StartCoroutine(_role.IE_Rot(_dir));
                    LinkInstance.Instance.MainPlayer.RoleTemplateActors.Add(_role);
                }
            }
        }
    }
    #endregion
    #region Utility Methods
    public ActionType CheckMoveDir(MoveDir _dir)
    {
        if (hasCheakAction) return nextActionType;
        hasCheakAction = true;
        var _moveDirPos = Vec2Pos.GetVec2ToDir(_dir);
        var _type = LinkInstance.Instance.SceneManager.GetSceneNodeTypeByVec2(_moveDirPos);
        switch (_type)
        {
            case SceneActorType.Null:
                //移动
                nextActionType = ActionType.Move;
                break;
            case SceneActorType.Player:
                //判断角色是否会朝该方向移动，是则转向并移动；否则只转向
                nextActionType = LinkInstance.Instance.MainPlayer.nextActionType;
                break;
            case SceneActorType.Enemy:
                //攻击
                nextActionType = ActionType.Attack;
                break;
            case SceneActorType.Obstacle:
                //只转向
                nextActionType = ActionType.Rot;
                break;
            case SceneActorType.Box:
                //打开
                nextActionType = ActionType.Interact;
                break;
            case SceneActorType.Follow:
                //判断角色是否会朝该方向移动，是则转向并移动；否则只转向
                var _obj = (RoleTemplateActor)LinkInstance.Instance.SceneManager.GetNodeByVec2(_moveDirPos);
                nextActionType = _obj.CheckMoveDir(_dir);
                break;
        }
        return nextActionType;
    }

    public IEnumerator IE_Action(MoveDir _dir)
    {
        switch (nextActionType)
        {
            case ActionType.Move:
               yield return StartCoroutine(IE_Move(_dir));
                CheckAround(_dir);
                break;
            case ActionType.Rot:
                yield return StartCoroutine(IE_Rot(_dir));
                break;
            case ActionType.Attack:
                yield return StartCoroutine(IE_Attack(_dir));
                break;
            case ActionType.Stop:
                break;
            case ActionType.Interact:
                yield return StartCoroutine(IE_Interact(_dir));
                break;
        }

    }
    #endregion
    public enum ActionType
    {
        Move,
        Rot,
        Attack,
        Stop,
        Interact,
    }
}
