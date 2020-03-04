/*文件名：PlayerActor.cs
 * 作者：YZY
 * 说明：主角控制器
 * 上次修改时间：2020/3/4 23：10：52 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : VividActor
{
    #region Parameters    
    

    #endregion
    #region Properties
   
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
        //判断_dir方向上是什么：       
        var _type= LinkInstance.Instance.SceneManager.GetSceneNodeTypeByVec2(Vec2Pos.GetVec2ToDir(_dir));
        switch (_type)
        {
            case SceneActorType.Null:
                //移动
                yield return StartCoroutine(IE_Move(_dir));
                break;
            case SceneActorType.Enemy:
                //攻击               
                yield return StartCoroutine(IE_Attack(_dir));
                break;
            case SceneActorType.Obstacle:
                //只是转方向
                yield return StartCoroutine(IE_Rot(_dir));
                break;
            case SceneActorType.Box:
                //打开箱子
                break;            
            case SceneActorType.Follow:
                //让该方向上的随从判断前进方向的情况
                break;
        }
        yield return StartCoroutine( LinkInstance.Instance.SceneManager.IE_EnemyAction());
        isMoving = false;
    }
    protected override void ChangeSceneNodes(MoveDir _dir)
    {
        base.ChangeSceneNodes(_dir);
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(Vec2Pos, SceneActorType.Player);
    }
    #endregion
    #region Utility Methods
    public void SetAttackTarget()
    {
        attackTargetPos = transform.forward*8 + transform.position;
    }
    #endregion
    public enum ActionType
    {
        Move,//移动
        Interact,//交互
    }



}

