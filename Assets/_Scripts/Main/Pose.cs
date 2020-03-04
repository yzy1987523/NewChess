/*文件名：Pose
 * 作者：YZY
 * 说明：Actor的pose，用finalIK key的pose
 * 上次修改时间：2020/2/27 16：14：20 *
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    public Transform[] handles;
    public Dictionary<PoseType, PoseData> poses=new Dictionary<PoseType, PoseData>();
    public PoseType tempPoseType;

    //使用pose前需要加载poses
    public void LoadPosesData()
    {
        var _array = Enum.GetValues(typeof(PoseType));
        for (var i = 0; i < _array.Length; i++)
        {
            var _type = (PoseType)_array.GetValue(i);
            poses.Add(_type, SerializeTool.DeSerializeFromFile<PoseData>(StaticData.GetDataPath(StaticData.FilePathType.PoseData) + _type.ToString()));
        }
    }
    public void ChangePose(PoseType _type)
    {
        for (var i = 0; i < handles.Length; i++)
        {
            if (poses[_type].vector3s.Length > i)
            {
                handles[i].localPosition = poses[_type].vector3s[i].ChangeToVec3();
                handles[i].localRotation = poses[_type].quaternions[i].ChangeToQunaternion();
            }
        }
    }
    [ContextMenu("SetHandles")]
    private void SetHandles()
    {
        handles = GetComponentsInChildren<Transform>();
    }
    [ContextMenu("AddPose")]
    private void AddPose()
    {
        var _pose = new PoseData();
        _pose.quaternions = new QuaternionStruct[handles.Length];
        _pose.vector3s = new Vector3Struct[handles.Length];
        for (var i = 0; i < handles.Length; i++)
        {
            _pose.vector3s[i] =new Vector3Struct( handles[i].localPosition);
            _pose.quaternions[i] =new QuaternionStruct( handles[i].localRotation);
        }
        SerializeTool.SerializeToFile(_pose, StaticData.GetDataPath(StaticData.FilePathType.PoseData)+ tempPoseType.ToString());
    }
    [ContextMenu("ChangePose")]
    private void ChangePose()
    {
        ChangePose(tempPoseType);
    }

}
[System.Serializable]
public struct PoseData
{
    public PoseType poseName;
    public QuaternionStruct[] quaternions;
    public Vector3Struct[] vector3s;
}

public enum PoseType
{
    Idle,
    Idle_HasWeapon,
    Idle_HasBow,
    Attack_0,
    Attack_1,
    Attack_HasWeapon_0,
    Attack_HasWeapon_1,
    Attack_HasBow_0,
    Attack_HasBow_1,
}
