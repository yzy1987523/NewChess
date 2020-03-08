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
        var _type = LinkInstance.Instance.SceneManager.GetSceneNodeTypeByVec2(Vec2Pos.GetVec2ToDir(_dir));
        var _enemyCanMove = true;
        switch (_type)
        {
            case SceneActorType.Null:
                //移动
                yield return StartCoroutine(IE_Move(_dir));
                //移动后判断3个方向是否有随从，有的话就让随从跟随
                break;
            case SceneActorType.Enemy:
                //攻击               
                yield return StartCoroutine(IE_Attack(_dir));
                break;
            case SceneActorType.Obstacle:
                //只是转方向
                yield return StartCoroutine(IE_Rot(_dir));
                _enemyCanMove = false;
                break;
            case SceneActorType.Box:
                //打开箱子
                _enemyCanMove = false;
                break;
            case SceneActorType.Follow:
                //让该方向上的随从判断前进方向的情况
                break;
        }
        if (_enemyCanMove)
            yield return StartCoroutine(LinkInstance.Instance.SceneManager.IE_EnemyAction());
        isMoving = false;
    }
    #endregion
    #region Utility Methods

    #endregion

}

