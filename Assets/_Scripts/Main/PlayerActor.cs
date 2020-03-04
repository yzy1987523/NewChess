using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    #region Parameters
    public PlayerArmState curPlayerArmState;
    WeaponCtrl weaponCtrl;
    Vector3 attackTargetPos;

    #endregion
    #region Properties
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
    private void Start()
    {
        ToIdle();
        WeaponCtrl.ChangeWeapon(curPlayerArmState);
    }
    private void Update()
    {
        if (isMoving) return;
        var _dir = MoveDir.None;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _dir = MoveDir.Left;
            StartCoroutine(IE_Move(_dir));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _dir = MoveDir.Right;
            StartCoroutine(IE_Move(_dir));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _dir = MoveDir.Up;
            StartCoroutine(IE_Move(_dir));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _dir = MoveDir.Down;
            StartCoroutine(IE_Move(_dir));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetAttackTarget();
            StartCoroutine(IE_Attack());
        }
    }
    IEnumerator IE_Attack()
    {        
        isMoving = true;
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
        if(curPlayerArmState== PlayerArmState.HasBow)
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
        isMoving = false;
    }
    void ToIdle()
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
    public void SetAttackTarget()
    {
        attackTargetPos = transform.forward*8 + transform.position;
    }
    #endregion

    public enum PlayerArmState
    {
        Null,
        HasWeapon,
        HasBow,
    }
   
}

