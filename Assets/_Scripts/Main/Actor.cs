using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    #region Parameters
    protected bool isMoving;
    Transform thisTrans;
    protected MoveDir curDir= MoveDir.Down;
    public EasingEquation rotEasingType=EasingEquation.QuartEaseInOut;
    public float rotSpeed=90;
    public EasingEquation moveEasingType = EasingEquation.QuartEaseInOut;
    public float moveDis = 1;
    public float moveSpeed = 1;
    Animator actorAnim;
    Pose pose;

    #endregion
    #region Properties
    public Transform ThisTrans
    {
        get
        {
            if (thisTrans == null)
            {
                thisTrans = transform;
            }
            return thisTrans;
        }

        set
        {
            thisTrans = value;
        }
    }

    public Animator ActorAnim
    {
        get
        {
            if (actorAnim == null)
            {
                actorAnim = GetComponentInChildren<Animator>();
            }
            return actorAnim;
        }

        set
        {
            actorAnim = value;
        }
    }

    public Pose Pose
    {
        get
        {
            if (pose == null)
            {
                pose = GetComponentInChildren<Pose>();
                pose.LoadPosesData();
            }
            return pose;
        }

        set
        {
            pose = value;
        }
    }


    #endregion
    #region Private Methods    
   
    protected virtual IEnumerator IE_Move(MoveDir _dir)
    {
        isMoving = true;
        var _end = false;
        var _timer = 0f;
        
        var _v = 0f;
        //如果方向不一样就先转身
        if (curDir != _dir)
        {
            var _angle =StaticFun.GetRotAngle(curDir, _dir);
            var _useTime =Mathf.Abs(  _angle / rotSpeed );
            var _orgRot = ThisTrans.rotation;
            var _targetRot = ThisTrans.rotation*Quaternion.Euler(0,_angle,0) ;
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
                ThisTrans.rotation= Quaternion.SlerpUnclamped(_orgRot, _targetRot, EasingManager.GetEaseProgress(rotEasingType, _v));
                yield return null;
            }
            curDir = _dir;
            _end = false;
            _timer = 0f;
            _v = 0f;
        }        
        var _moveUseTime = moveDis / moveSpeed;
        var _orgPos = ThisTrans.position;
        var _targetPos = ThisTrans.position +ThisTrans.forward* moveDis;
        ActorAnim.SetBool("Jump", true);
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
