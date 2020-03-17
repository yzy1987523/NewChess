/*文件名：Bullet.cs
 * 作者：YZY
 * 说明：子弹，目前版本只有弓箭，重要做抛物线运动
 * 上次修改时间：2020/3/1 20:53:08 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float flySpeed = 10;
    [SerializeField]
    private Transform target;
    public void Shot(Vector3 _pos,Quaternion _rot,Vector3 _target)
    {
        transform.position = _pos;
        transform.rotation = _rot;
        StartCoroutine(IE_Move(_target));
    }
    public void Shot(Vector3 _target)
    {

        StartCoroutine(IE_Move(_target));
    }
    private IEnumerator IE_Move(Vector3 _target)
    {
        var _end = false;
        var _dis = Vector3.Distance(transform.position, _target);
        var _angle = 0f;
        var _tempDis = 0f;
        while (!_end)
        {
            transform.LookAt(_target);
            _tempDis = Vector3.Distance(transform.position, _target);
            _angle = Mathf.Min(1, _tempDis / _dis) * 45;
            transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-_angle, -42, 42), 0, 0);
            if (_tempDis < 0.1f)
            {
                _end = true;
            }
            transform.Translate(Vector3.forward * Mathf.Min(flySpeed * Time.deltaTime, _tempDis));
            yield return null;
        }
    }
    [ContextMenu("TestShot")]
    public void Shot()
    {
        Shot(transform.position,transform.rotation, target.position);
    }
}
