/*文件名：VividActor.cs
 * 作者：YZY
 * 说明：活动对象的基类
 * 上次修改时间：2020/3/4 23：10：24 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VividActor : NodeActor
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
    public PlayerArmState curPlayerArmState;
    protected WeaponCtrl weaponCtrl;
    protected Vector3 attackTargetPos;   
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
    public WeaponCtrl WeaponCtrl
    {
        get
        {
            if (weaponCtrl == null)
            {
                weaponCtrl = GetComponentInChildren<WeaponCtrl>();
            }
            return weaponCtrl;
        }

        set
        {
            weaponCtrl = value;
        }
    }

    

    #endregion
    #region Private Methods       
    protected virtual IEnumerator IE_Rot(MoveDir _dir)
    {
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
        }
    }
    protected virtual IEnumerator IE_Move(MoveDir _dir)
    {
        yield return StartCoroutine(IE_Rot(_dir));
        PlayAnim("Move");
        var _end = false;
        var _timer = 0f;        
        var _v = 0f;       
        var _moveUseTime = moveDis / moveSpeed;
        var _orgPos = ThisTrans.position;
        var _targetPos = ThisTrans.position +ThisTrans.forward* moveDis;
       
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
        ChangeSceneNodes(_dir);
       
    }
    protected virtual void PlayAnim(string _animName)
    {
        ActorAnim.SetBool(_animName, true);
    }
    //每次移动都会造成寻路节点的状态变化及自身的位置变化
    protected virtual void ChangeSceneNodes(MoveDir _dir)
    {
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(Vec2Pos, SceneActorType.Null);
        Vec2Pos += _dir.MoveDirChangeToVec2();                   
    }
    protected IEnumerator IE_Attack(MoveDir _dir)
    {
        yield return StartCoroutine(IE_Rot(_dir));
        PoseType _attack_0 = PoseType.Attack_0;
        PoseType _attack_1 = PoseType.Attack_1;
        switch (curPlayerArmState)
        {
            case PlayerArmState.HasWeapon:
                _attack_0 = PoseType.Attack_HasWeapon_0;
                _attack_1 = PoseType.Attack_HasWeapon_1;
                break;
            case PlayerArmState.HasBow:
                _attack_0 = PoseType.Attack_HasBow_0;
                _attack_1 = PoseType.Attack_HasBow_1;
                break;
        }
        Pose.ChangePose(_attack_0);
        if (curPlayerArmState == PlayerArmState.HasBow)
        {
            WeaponCtrl.Shot_0();
        }
        yield return new WaitForSeconds(0.2f);
        if (curPlayerArmState == PlayerArmState.HasBow)
        {
            WeaponCtrl.Shot_1(attackTargetPos);
        }
        Pose.ChangePose(_attack_1);
        yield return new WaitForSeconds(0.2f);
        ToIdle();
      
    }
    protected void ToIdle()
    {
        PoseType _pose = PoseType.Idle;
        switch (curPlayerArmState)
        {
            case PlayerArmState.HasWeapon:
                _pose = PoseType.Idle_HasWeapon;
                break;
            case PlayerArmState.HasBow:
                _pose = PoseType.Idle_HasBow;
                break;
        }
        Pose.ChangePose(_pose);
    }
    #endregion
    #region Utility Methods
    public override void Init(SceneActorType _type)
    {
        base.Init(_type);
        //ToIdle();
        //WeaponCtrl.ChangeWeapon(curPlayerArmState);
    }
    public virtual void SetAttackTarget()
    {
        attackTargetPos = transform.forward * 8 + transform.position;
    }
    #endregion
    public enum PlayerArmState
    {
        Null,
        HasWeapon,
        HasBow,
    }

}
