using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Runtime : MonoBehaviour
{
    [ContextMenu("InitFolder")]
    public void InitFolder()
    {
        var _enum = Enum.GetValues(typeof(StaticData.FilePathType));
        foreach (StaticData.FilePathType n in _enum)
        {
            var _path = StaticData.GetDataFloderPath(n);
            DirectoryInfo i = new DirectoryInfo(_path);
            if (!i.Exists)
            {
                Directory.CreateDirectory(_path);
            }
        }
    }

   
}
