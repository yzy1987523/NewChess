/*文件名：EnemyActor.cs
 * 作者：YZY
 * 说明：敌人
 * 上次修改时间：2020/2/28 18：49：32 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor
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
    #endregion
    #region Utility Methods

    #endregion
}
