/*文件名：StaticFun.cs
 * 作者：YZY
 * 说明：常用的静态方法
 * 上次修改时间：2020/3/4 23：08：43 *
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFun
{
    //获取转角
    public static float GetRotAngle(MoveDir _cur, MoveDir _target)
    {
        var _angle = 90 * (_target - _cur);
        if (_angle > 180)
        {
            _angle -= 360;
        }
        else if (_angle <= -180)
        {
            _angle += 360;
        }
        return _angle;
    }       
   
    //将List随机排序
    public static List<T> RandomSort<T>(List<T> _array)
    {        
        for (var i = 0; i < _array.Count; i++)
        {
            var _temp = _array[i];
            var _index = UnityEngine.Random.Range(0, _array.Count);//可能会包含当前元素，这时相当于没有换，不过没关系
            _array[i] = _array[_index];
            _array[_index] = _temp;
        }
        return _array;
    }

}
