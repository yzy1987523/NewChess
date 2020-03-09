/*文件名：ExpandMethods.cs
 * 作者：YZY
 * 说明：里面放置自定义的扩展方法
 * 上次修改时间：2019/12/20 17：09：11 *
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class ExpandMethods
{
    public static bool IsClose(this Vector3 self, Vector3 target, float dis)
    {
        return self.GetHorizontalDistance(target) <= dis;
    }
    //批量显隐蒙皮网格
    public static void ShowModel(this SkinnedMeshRenderer[] meshRenderer, bool show)
    {
        foreach (var mesh in meshRenderer)
        {
            mesh.enabled = show;
        }
    }
    //获取水平方向的距离
    public static float GetHorizontalDistance(this Vector3 self, Vector3 target)
    {
        target.y = self.y;
        return Vector3.Distance(target, self);
    }
    //获取直线距离
    public static float GetDistance(this Vector3 self, Vector3 target)
    {
        return Vector3.Distance(target, self);
    }
    //水平方向上朝向目标点
    public static void LookAtHorizontal(this Transform self, Vector3 target)
    {
        target.y = self.position.y;
        self.LookAt(target);
    }
    //获取在self到target方向上，距离target有dis长的点，其中默认为水平方向
    public static Vector3 GetPointCloseTarget(this Vector3 self, Vector3 target, float dis, bool isHorizontal = true)
    {
        if (isHorizontal)
        {
            target.y = self.y;
        }
        var dir = target - self;
        return target - dir.normalized * dis;
    }
    //获取在self到target方向上，距离self有dis长的点，其中默认为水平方向
    public static Vector3 GetPointByDir(this Vector3 self, Vector3 dir, float dis, bool isHorizontal = true)
    {
        if (isHorizontal)
        {
            dir.y = self.y;
        }
        return self + dir.normalized * dis;
    }
  
    //获取点在水平面上的倒影
    public static Vector3 PosOfReverseY(this Vector3 self)
    {
        var _pos = self;
        _pos.y = -_pos.y;
        return _pos;
    }
    //顺时针时更近
    public static bool IsCloseClockwise(this Vector3 _from, Vector3 _to)
    {
        var _rot = Quaternion.FromToRotation(_from, _to);
        return _rot.eulerAngles.y < 180;
    }
    public static void SetPosToSameHeight(this Transform _trans, Vector3 _target)
    {
        var _p = _target;
        _p.y = _trans.position.y;
        _trans.position = _p;
    }
    //点在对象前方一定角度内
    public static bool IsInFrontArea(this Transform _trans, Vector3 _target, float _angle)
    {
        return (Vector3.Angle(_target - _trans.position, _trans.forward) <= _angle);
    }
    //弹簧:this与_mid差距越大，返回值差值越大,_resilience=0.02f比较好，_mid一般都为0
    public static float ChangeBySpring(this float _this, float _resilience, float _mid)
    {
        return _this - (_this - _mid) * _resilience;
    }

    public static bool IsSameAnimState(this Animator _this, int _hash)
    {
        return _this.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(_hash);
    }
    //获取夹角（按在以水平面上的情况,从-180到180）
    public static float GetAngleByHorizontal(this Vector3 _from, Vector3 _to)
    {
        if (Vector3.Cross(_from, _to).y > 0)
        {
            return -Vector3.Angle(_from, _to);//在左侧
        }
        return Vector3.Angle(_from, _to);//在右侧
    }
    //获取字符串后面的数字，假如没有数字则返回为null
    public static int? GetLastNum(this string _v)
    {
        var match = System.Text.RegularExpressions.Regex.Match(_v, "[0-9]+$");
        if (match.Length > 0)
        {
            return int.Parse(match.Value);
        }
        return null;

    }
    public static void SetGroupAlpha(this CanvasGroup _this,float _v)
    {
        if (_this == null)  return; 
        _this.alpha = _v;
        _this.interactable = _this.blocksRaycasts = _v > 0;

    }
    //将秒数转换为标准时间
    public static string NormalTime(this float _this)
    {
        var hour = Mathf.FloorToInt(_this / 3600);
        var min = Mathf.FloorToInt(_this % 3600);
        return hour.GetPointLength() + ":" + Mathf.FloorToInt(min/60).GetPointLength();

    }
    //获取指定长度
    public static string GetPointLength(this int _this)
    {        
        if (_this < 10)
        {
            return 0 + "" + _this;
        }
        else
        {
            return _this + "";
        }
    }

    //获取次级子
    public static List<Transform> GetChildren(this Transform _this)
    {
        var count = _this.childCount;
        var list = new List<Transform>();
        for(var i = 0; i < count; i++)
        {
            list.Add(_this.GetChild(i));
        }
        return list;
    }
    //获取次级子
    public static List<RectTransform> GetChildren(this RectTransform _this)
    {
        var count = _this.childCount;
        var list = new List<RectTransform>();
        for (var i = 0; i < count; i++)
        {
            list.Add(_this.GetChild(i).GetComponent<RectTransform>());
        }
        return list;
    }
    //在时间设置中用到，获取当前年份所处十年的左区间
    public static int GetDateFirstYear(this DateTime _this)
    {
       return _this.Year / 10 * 10;
    }
    //设置数组值，因为数组是引用类型
    public static double[] EqualsValue(this double[] _targetArray)
    {
        double[] _array = new double[_targetArray.Length];
        for (var i = 0; i < _targetArray.Length; i++)
        {
            _array[i] = _targetArray[i];
        }
        return _array;

    }
    //设置数组值，因为数组是引用类型
    public static float[] EqualsValue(this float[] _targetArray)
    {
        float[] _array = new float[_targetArray.Length];
        for (var i = 0; i < _targetArray.Length; i++)
        {
            _array[i] = _targetArray[i];
        }
        return _array;

    }
    //设置数组值，因为数组是引用类型
    public static Vector3[] EqualsValue(this Vector3[] _targetArray)
    {
        Vector3[] _array = new Vector3[_targetArray.Length];
        for (var i = 0; i < _targetArray.Length; i++)
        {
            _array[i] = _targetArray[i];
        }
        return _array;

    }
    //获取前几位，去掉最后一位
    public static double[] GetFront(this double[] _targetArray,int lastCount=1)
    {
        double[] _array = new double[_targetArray.Length- lastCount];
        for (var i = 0; i < _array.Length; i++)
        {
            _array[i] = _targetArray[i];
        }
        return _array;

    }
    //获取水平方向的四元数
    public static Quaternion GetHQuaternion(this Quaternion _v)
    {
        var _new = _v;
        var _angle=_new.eulerAngles;
        _angle.x = 0;
        _angle.z = 0;
        _new =Quaternion.Euler(_angle);
        return _new;

    }
    //根据秒转为小时
    public static float GetHour(this float _v)
    {        
         var _h=(int) _v / 3600;
       
        return _h;

    }
    //将字节转为G
    public static float GetG(this float _v)
    {
        return Mathf.FloorToInt(_v / 1024 / 1024 / 1024*10)/10f;
    }
    //将字节转为流量（MB）
    public static float GetMB(this float _v)
    {

        return Mathf.FloorToInt(_v / 1024 / 1024*100)/100f;

    }
    //将字节转为速率（Mbps）
    public static float GetMbps(this float _v)
    {

        return Mathf.FloorToInt(_v / 1024 / 1024*100)/100f;

    }
    //获取精确到2位的float
    public static string GetFloat2(this float _v)
    {
        return _v.ToString("0.##");
    }
    //获取精确到2位的float
    public static float AngleClamp(this float angle, float min=-360,float max=360)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    public static Vector2 GetVector2(this Vector3 _v)
    {
        var v= new Vector2(_v.x,_v.y);
        return v;
    }

    //寻找子对象
    public static Transform FindInAll(this Transform _this,string _v)
    {
        var _t = _this.Find(_v);
        if (_t == null)
        {
            for(var i = 0; i < _this.childCount;i++)
            {
                var _temp = _this.GetChild(i).FindInAll(_v);
                if (_temp != null)
                {
                    return _temp;
                }
                else
                {
                    continue;
                }               
            }
        }        
        return _t;        
    }   
   

    //在数组前加上一个元素
    public static string[] AddFront(this string[] _v,string _addV)
    {
        var _strs = new string[_v.Length + 1];
        _strs[0] = _addV;
        for(var i = 1; i < _strs.Length; i++)
        {
            _strs[i] = _v[i - 1];
        }
        return _strs;
    }
    //将时间戳转换为字符串
    public static string ChangeTimeStampToDate(this string _v)
    {      
        return "";
    }
   
    //判断奇偶
    public static bool IsEvenNumber(this int _v)
    {
        return (_v&1)==0;
    }

    public static Vector3 ChangeToVec3(this Vector3Struct _v)
    {
        return new Vector3(_v.x,_v.y,_v.z);
    }
    public static Quaternion ChangeToQunaternion(this QuaternionStruct _v)
    {
        return new Quaternion(_v.x, _v.y, _v.z,_v.w);
    }

    //获取指定方向的位置:当前位置在某个方向上的坐标
    public static Vector2Int GetVec2ToDir(this Vector2Int _curPos, MoveDir _dir)
    {        
        return _dir.MoveDirChangeToVec2() + _curPos;
    }
    //将MoveDir转换为相对坐标
    public static Vector2Int MoveDirChangeToVec2(this MoveDir _dir)
    {
        switch (_dir)
        {
            case MoveDir.None:
                return new Vector2Int(0, 0);
            case MoveDir.Up:
                return new Vector2Int(0, 1);
            case MoveDir.Right:
                return new Vector2Int(1, 0);
            case MoveDir.Down:
                return new Vector2Int(0, -1);
            case MoveDir.Left:
                return new Vector2Int(-1, 0);
            default:
                break;
        }
        return default;
    }
    //获取下一步的朝向:转为寻路设计，根据当前方向和下个节点的位置，确认下一步的朝向
    public static MoveDir GetDir(this Vector2Int _curPos, Vector2Int _target, MoveDir _curDir)
    {
        if (_curPos.x == _target.x)
        {
            if (_curPos.y > _target.y)
            {
                return MoveDir.Down;
            }
            return MoveDir.Up;
        }
        else if(_curPos.y == _target.y)
        {
            if (_curPos.x > _target.x)
            {
                return MoveDir.Left;
            }
            return MoveDir.Right;
        }
        //下个点朝向斜向
        else
        {
            var _dir = _target - _curPos;
            //_curDir.MoveDirChangeToVec2() - _dir ==============？？？？
            var _curdir = _curDir.MoveDirChangeToVec2();
             var _angle=Vector2.Angle(_curdir, _dir);
            if (_angle == 45||_angle==315)
            {
                return _curDir;
            }
            if (_angle == 135)
            {                
                return (new Vector2Int(-_curdir.y, _curdir.x)).Vec2ChangeToMoveDir();
            }
            if (_angle == 225)
            {
                return (new Vector2Int(_curdir.y, _curdir.x)).Vec2ChangeToMoveDir();
            }
        }
        return default;
    }
    //将相对坐标转换为MoveDir
    public static MoveDir Vec2ChangeToMoveDir(this Vector2Int _vec)
    {
        if (_vec.x == 0)
        {
            if (_vec.y > 0)
            {
                return MoveDir.Up;
            }
            return MoveDir.Down;
        }
        else
        {
            if (_vec.x > 0)
            {
                return MoveDir.Right;
                
            }
            return MoveDir.Left;
        }
    }
}
[System.Serializable]
public struct QuaternionStruct
{
    public float x;
    public float y;
    public float z;
    public float w;
    public QuaternionStruct(Quaternion _v)
    {
        x = _v.x;
        y = _v.y;
        z = _v.z;
        w = _v.w;
    }
}
[System.Serializable]
public struct Vector3Struct
{
    public float x;
    public float y;
    public float z;
    public Vector3Struct(Vector3 _v)
    {
        x = _v.x;
        y = _v.y;
        z = _v.z;
    }
}
