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
