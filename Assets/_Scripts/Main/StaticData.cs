﻿/*文件名：StaticData.cs
 * 作者：YZY
 * 说明：专门放静态数据
 * 上次修改时间：2020/3/4 23：09：13 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData
{
    public enum FilePathType
    {
        PoseData,
    }
    public static string GetDataPath(FilePathType _type)
    {
        return FilePath[(int)_type];
    }
    public static string GetDataFloderPath(FilePathType _type)
    {
        return FilePath[(int)_type];
    }
    public static string[] FilePath =
{
        "Assets/StreamingAsset/Data/PoseData/",
    };
   
}
#region 全局的枚举也放这
public enum SceneActorType
{
    Null,
    Player,
    Enemy,
    Obstacle,
    Box,
    Follow,//随从
}
public enum MoveDir
{
    None = -1,
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}
#endregion