/*文件名：
 * 作者：YZY
 * 说明：控制弓和刀的显隐，及弓的控制
 * 上次修改时间：
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    public Transform weaponHandle_L;
    public Transform weaponHandle_R;
    GameObject sword;
    GameObject bow;//弓
    //Transform stringHandle;//弦
    GameObject quiver;//箭囊
    GameObject arrowOrgObj;//箭
    Bullet replicaArrow;//复制体

    public GameObject Sword
    {
        get
        {
            if (sword == null)
            {
                sword = weaponHandle_R.FindInAll("Sword").gameObject;
            }
            return sword;
        }

        set
        {
            sword = value;
        }
    }

    public GameObject Bow
    {
        get
        {
            if (bow == null)
            {
                bow = weaponHandle_L.FindInAll("Bow").gameObject;
            }
            return bow;
        }

        set
        {
            bow = value;
        }
    }

    public GameObject Quiver
    {
        get
        {
            if (quiver == null)
            {
                quiver = transform.FindInAll("Quiver").gameObject;
            }
            return quiver;
        }

        set
        {
            quiver = value;
        }
    }

    public GameObject ArrowOrgObj
    {
        get
        {
            if (arrowOrgObj == null)
            {
                arrowOrgObj = weaponHandle_R.FindInAll("Arrow").gameObject;
            }
            return arrowOrgObj;
        }

        set
        {
            arrowOrgObj = value;
        }
    }

    public Bullet ReplicaArrow
    {
        get
        {
            if (replicaArrow == null)
            {
                replicaArrow = weaponHandle_R.FindInAll("ReplicaArrow").GetComponentInChildren<Bullet>();
                replicaArrow.transform.SetParent(null);
            }
            return replicaArrow;
        }

        set
        {
            replicaArrow = value;
        }
    }

    public void ChangeWeapon(PlayerActor.PlayerArmState _state)
    {
        switch (_state)
        {
            case PlayerActor.PlayerArmState.Null:
                Sword.SetActive(false);
                EquipBow(false);
                break;
            case PlayerActor.PlayerArmState.HasWeapon:
                Sword.SetActive(true);
                EquipBow(false);
                break;
            case PlayerActor.PlayerArmState.HasBow:
                Sword.SetActive(false);
                EquipBow(true);
                break;
        }
    }
    public void EquipBow(bool _true)
    {
        ArrowOrgObj.SetActive(false);
        ReplicaArrow.gameObject.SetActive(false);
        Bow.SetActive(_true);
        Quiver.SetActive(_true);       
    }
    public void Shot_0()
    {
        ArrowOrgObj.SetActive(true);
    }
    public void Shot_1(Vector3 _target)
    {
        ArrowOrgObj.SetActive(false);
        ReplicaArrow.gameObject.SetActive(true);
        ReplicaArrow.transform.position = ArrowOrgObj.transform.position;
        ReplicaArrow.transform.rotation = ArrowOrgObj.transform.rotation;
        ReplicaArrow.Shot(_target);
    }
}
