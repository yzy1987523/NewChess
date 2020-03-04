/*文件名：
 * 作者：YZY
 * 说明：常用的静态方法
 * 上次修改时间：
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
    ////将数列随机排序
    //public static void RandomSort(List<int> _array)
    //{
    //    if (_array == null) return;
    //    for(var i=0; i < _array.Count; i++)
    //    {
    //        var _temp = _array[i];
    //        var _index = UnityEngine.Random.Range(0, _array.Count);//可能会包含当前元素，这时相当于没有换，不过没关系
    //        _array[i] = _array[_index];
    //        _array[_index] = _temp;
    //    }
    //}
    //生成一个0-n的list
    public static List<int> GetSortList(int _length)
    {
        var _list = new List<int>();
        for(var i = 0; i < _length; i++)
        {
            _list.Add(i);
        }
        return _list;
    }
    //生成一个随机排序的list
    public static List<int> GetRandomSortList(int _length)
    {
        var _list = GetSortList(_length);
        RandomSort(_list);
        return _list;
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
public enum MoveDir
{
    None = -1,
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}