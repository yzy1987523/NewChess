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
    [HideInInspector]
    public bool hasCheakAction;//确认行为后置为true，执行完后置为false
    [HideInInspector]
    public ActionType nextActionType;
    [HideInInspector]
    public bool hasCheckLink;
    SkinnedMeshRenderer meshRenderer;
    Material mat;

    #endregion
    #region Properties
    public SkinnedMeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
            {
                meshRenderer =transform.FindInAll("Skeletonl_base").GetComponentInChildren<SkinnedMeshRenderer>();
            }
            return meshRenderer;
        }

        set
        {
            meshRenderer = value;
        }
    }

    public Material Mat
    {
        get
        {
            if (mat == null)
            {
                mat = MeshRenderer.material;
            }
            return mat;
        }

        set
        {
            mat = value;
        }
    }
    #endregion
    #region Private Methods  

    public IEnumerator IE_FollowRot(MoveDir _dir)
    {
        var _end = false;
        var _timer = 0f;
        var _v = 0f;
        //如果方向不一样就先转身
        if (curDir != _dir)
        {
            var _angle = StaticFun.GetRotAngle(curDir, _dir);
            StaticData.FollowRotTime = Mathf.Abs(_angle / rotSpeed);
            var _orgRot = ThisTrans.rotation;
            var _targetRot = ThisTrans.rotation * Quaternion.Euler(0, _angle, 0);
            while (!_end)
            {
                if (_v >= 1)
                {
                    _end = true;
                }
                else
                {
                    _timer += Time.deltaTime;
                }
                _v = Mathf.Clamp01(_timer / StaticData.FollowRotTime);
                ThisTrans.rotation = Quaternion.SlerpUnclamped(_orgRot, _targetRot, EasingManager.GetEaseProgress(rotEasingType, _v));
                yield return null;
            }
            curDir = _dir;
        }
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
                    StartCoroutine(_role.IE_FollowRot(_dir));
                    LinkInstance.Instance.MainPlayer.AddFollow(_role);
                    _role.CheckAround(_dir);
                }
            }
        }
    }
    #endregion
    #region Utility Methods
    public override void Init(SceneActorType _type)
    {
        base.Init(_type);
        if(_type==SceneActorType.Follow)
        CheckMat(SceneActorType.Follow);
    }
    public virtual ActionType CheckMoveDir(MoveDir _dir)
    {
        if (hasCheakAction) return nextActionType;
        hasCheakAction = true;
        var _moveDirPos = Vec2Pos.GetVec2ToDir(_dir);
        var _type = LinkInstance.Instance.SceneManager.GetSceneNodeTypeByVec2(_moveDirPos);
        switch (_type)
        {
            case SceneActorType.Null:
            case SceneActorType.Exit:
                //移动       
                if (ThisActorType == SceneActorType.Player)
                {
                    nextActionType = ActionType.Move;
                }
                else
                {
                    if (LinkInstance.Instance.MainPlayer.nextActionType == ActionType.Rot)
                    {
                        nextActionType = ActionType.Rot;
                    }
                    else
                    {
                        nextActionType = ActionType.Move;
                    }
                }
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
    public void CheckLink()
    {
        if (hasCheckLink) return;
        hasCheckLink = true;
        for (var i = 0; i < 4; i++)
        {
            var _obj = LinkInstance.Instance.SceneManager.GetNodeByVec2(Vec2Pos.GetVec2ToDir((MoveDir)i));
            if (_obj != null && _obj.ThisActorType == SceneActorType.Follow)
            {
                var _role = (RoleTemplateActor)_obj;
                if (LinkInstance.Instance.MainPlayer.RoleTemplateActors.Contains(_role))
                {
                    if (_role.nextActionType != ActionType.Move)
                    {
                        LinkInstance.Instance.MainPlayer.RemoveFollow(_role);
                    }
                    else
                    {                        
                        _role.CheckLink();
                    }
                }
            }
        }
        
    }
    public void CheckMat(SceneActorType _type)
    {
        switch (_type)
        {
            case SceneActorType.Player:
                Mat.color = StaticData.FollowColor_Follow;
                break;
            case SceneActorType.Follow:
                Mat.color = StaticData.FollowColor_Normal;
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
