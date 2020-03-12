/*文件名：PlayerActor.cs
 * 作者：YZY
 * 说明：主角控制器
 * 上次修改时间：2020/3/4 23：10：52 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : RoleTemplateActor
{
    #region Parameters    
    List<RoleTemplateActor> roleTemplateActors;
    bool hasAddFollow;

    #endregion
    #region Properties
    public List<RoleTemplateActor> RoleTemplateActors
    {
        get
        {
            if (roleTemplateActors == null)
            {
                roleTemplateActors = new List<RoleTemplateActor>();
            }
            return roleTemplateActors;
        }

        set
        {
            roleTemplateActors = value;
        }
    }
    #endregion
    #region Private Methods  

    private void Update()
    {
        if (isMoving) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine( IE_OnceAction(MoveDir.Left));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(IE_OnceAction(MoveDir.Right));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(IE_OnceAction(MoveDir.Up));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(IE_OnceAction(MoveDir.Down));
        }
       
    }
   
    protected IEnumerator IE_OnceAction(MoveDir _dir)
    {
        isMoving = true;
        nextActionType = ActionType.Stop;
        nextActionType=CheckMoveDir(_dir);
        for(var i = 0; i < RoleTemplateActors.Count; i++)
        {
            RoleTemplateActors[i].hasCheckLink = false;            
            RoleTemplateActors[i].CheckMoveDir(_dir);
        }        
        if (nextActionType == ActionType.Move) {
            hasCheckLink = false;
            CheckLink();
            var _step = 0;
            while(_step< RoleTemplateActors.Count)
            {
                if (!RoleTemplateActors[_step].hasCheckLink)
                {
                    RemoveFollow(RoleTemplateActors[_step]);
                }
                else
                {
                    _step++;
                }
            }
        }
        for (var i = 0; i < RoleTemplateActors.Count; i++)
        {   
            StartCoroutine(RoleTemplateActors[i].IE_Action(_dir));
        }
         yield return StartCoroutine(IE_Action(_dir));
        //判断所有随从，是否脱离了队伍：与主角断开连接（三消算法）
        for(var i = 0; i < RoleTemplateActors.Count; i++)
        {
            RoleTemplateActors[i].hasCheakAction = false;
        }
        if (hasAddFollow)
        {
            yield return new WaitForSeconds(StaticData.FollowRotTime);
            hasAddFollow = false;
        }
        if (nextActionType==ActionType.Move|| nextActionType == ActionType.Attack)
            yield return StartCoroutine(LinkInstance.Instance.SceneManager.IE_EnemyAction());
        isMoving = false;
        hasCheakAction = false;        
    }

    //让所有随从转向正方向
    //protected IEnumerator FollowRot()
    //{
    //    for (var i = 0; i < RoleTemplateActors.Count; i++)
    //    {         
    //       yield return StartCoroutine(RoleTemplateActors[i].IE_Rot(curDir));
    //    }        
    //}
   
    #endregion
    #region Utility Methods
    public void StartGame()
    {
       CheckAround(curDir);
    }
    public void AddFollow(RoleTemplateActor _follow)
    {
        hasAddFollow = true;
        _follow.CheckMat(SceneActorType.Player);
        RoleTemplateActors.Add(_follow);
    }
    public void RemoveFollow(RoleTemplateActor _follow)
    {
        _follow.CheckMat(SceneActorType.Follow);
        RoleTemplateActors.Remove(_follow);
    }
    //检测已有的随从是否还能跟上
   
    #endregion

}

