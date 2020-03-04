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
    protected override IEnumerator IE_Move(MoveDir _dir)
    {
        isMoving = true;
        var _end = false;
        var _timer = 0f;

        var _v = 0f;
        //如果方向不一样就先转身
        if (curDir != _dir)
        {
            var _angle = StaticFun.GetRotAngle(curDir, _dir);
            var _useTime = Mathf.Abs(_angle / rotSpeed);
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
                _v = Mathf.Clamp01(_timer / _useTime);
                ThisTrans.rotation = Quaternion.SlerpUnclamped(_orgRot, _targetRot, EasingManager.GetEaseProgress(rotEasingType, _v));
                yield return null;
            }
            curDir = _dir;
            _end = false;
            _timer = 0f;
            _v = 0f;
        }
        var _moveUseTime = moveDis / moveSpeed;
        var _orgPos = ThisTrans.position;
        var _targetPos = ThisTrans.position + ThisTrans.forward * moveDis;
        //ActorAnim.SetBool("Jump", true);
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
            _v = Mathf.Clamp01(_timer / _moveUseTime);
            ThisTrans.position = Vector3.LerpUnclamped(_orgPos, _targetPos, EasingManager.GetEaseProgress(moveEasingType, _v));
            yield return null;
        }
        isMoving = false;
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
        var _endNodePos = LinkInstance.Instance.MainPlayer.Vec2Pos;
        //获取下一步的方向
        var _nextPos = AStarTool.GetNextPos(Vec2Pos, _endNodePos);
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
