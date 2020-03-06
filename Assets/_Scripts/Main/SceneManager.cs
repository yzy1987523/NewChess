/*文件名：SceneManager.cs
 * 作者：YZY
 * 说明：生成一个场景
 * 上次修改时间：2020/3/4 23:09:51 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;
public class SceneManager : MonoBehaviour
{
    #region Parameters
    public Vector2Int size = new Vector2Int(16, 10);
    //int nodeCount;
    public PlayerActor playerObj;
    public Vector2Int wallCount = new Vector2Int(30, 80);
    public NodeActor wallObj;
    public Vector2Int enemyCount = new Vector2Int(1, 3);
    public EnemyActor enemyObj;
    public Vector2Int boxCount = new Vector2Int(1, 2);
    public BoxCtrl boxObj;
    List<NodeClass> nodes = new List<NodeClass>();
    List<SceneActorType> sceneStateList = new List<SceneActorType>();
         
    List<EnemyActor> enemyList = new List<EnemyActor>();
    List<BoxCtrl> boxList = new List<BoxCtrl>();
    PlayerActor mainPlayer;
    #endregion
    #region Properties
    #endregion

    #region 创建初始场景

    //随机生成若干格子，包括起点，终点，障碍
    [ContextMenu("Create")]
    public void RCreateNode()
    {
        nodes.Clear();
        enemyList.Clear();
        boxList.Clear();
        Destroy();
        var _nodeCount = size.x * size.y;       
        var _tempNodes = new List<NodeClass>();
        //设置主角
        var _node = new NodeClass();
        _node.nodeType = NodeType.NormalNode;
        _node.nodeObj = Instantiate(playerObj);
        mainPlayer= (PlayerActor)_node.nodeObj;
        _node.nodeObj.Init(SceneActorType.Player);        
        _tempNodes.Add(_node);
        //设置墙
        var _wallCount = Random.Range(wallCount.x, wallCount.y);
        for (var i = 0; i < _wallCount; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.ObstacleNode;
            _temp.nodeObj = Instantiate(wallObj);
            _temp.nodeObj.Init(SceneActorType.Obstacle);
            _tempNodes.Add(_temp);
        }
        //设置敌人
        var _enemyCount = Random.Range(enemyCount.x, enemyCount.y);
        for (var i = 0; i < _enemyCount; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.NormalNode;
            _temp.nodeObj = Instantiate(enemyObj);
            _temp.nodeObj.Init(SceneActorType.Enemy);
            enemyList.Add((EnemyActor)_temp.nodeObj);
            _tempNodes.Add(_temp);
        }
        //设置宝箱       
        var _boxCount = Random.Range(boxCount.x, boxCount.y);
        for (var i = 0; i < _boxCount; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.ObstacleNode;
            _temp.nodeObj = Instantiate(boxObj);        
            _temp.nodeObj.Init(SceneActorType.Box);
            boxList.Add((BoxCtrl)_temp.nodeObj);
            _tempNodes.Add(_temp);
        }
        //设置正常node
        for (var i = _wallCount+_boxCount+_enemyCount+1; i < _nodeCount; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.NormalNode;          
            _tempNodes.Add(_temp);
        }
        //给nodes赋值
        nodes = StaticFun.RandomSort(_tempNodes);
        for (var i = 0; i < nodes.Count; i++)
        {
            nodes[i].nodePos = GetPosByIndex(i);
            sceneStateList.Add(SceneActorType.Null);
            if (nodes[i].nodeObj != null)
            {
                nodes[i].nodeObj.transform.SetParent(transform);
                nodes[i].nodeObj.transform.localPosition = GetPosByVec2(nodes[i].nodePos);
                nodes[i].nodeObj.transform.localRotation =Quaternion.Euler(0,180,0);
                sceneStateList[i]=nodes[i].nodeObj.ThisActorType;
                nodes[i].nodeObj.SetPos(nodes[i].nodePos);
            }
        }
        AStarTool.SetNodes(nodes,size);
    }
    Vector2Int GetPosByIndex(int _index)
    {
        var _temp = new Vector2Int(_index % size.x, _index / size.x);
        return _temp;
    }
    Vector3 GetPosByVec2(Vector2Int _vec2)
    {
        return new Vector3(_vec2.x, 0, _vec2.y);
    }
    //生成节点对象   
    [ContextMenu("Destroy")]
    private void Destroy()
    {
        while (transform.childCount > 0)
        {
            var _temp = transform.GetChild(0);
            _temp.SetParent(null);
            DestroyImmediate(_temp.gameObject);
        }
    }
    #endregion

    #region get方法
    public PlayerActor GetMainPlayer()
    {
        return mainPlayer;
    }
    //改变某个节点的状态
    public void ChangeSceneNodes(Vector2Int _pos, SceneActorType _targetType)
    {
        sceneStateList[GetIndexByVec2(_pos)] = _targetType;
    }

    int GetIndexByVec2(Vector2Int _v)
    {
        var _temp = _v.x+_v.y*size.x;        
        return _temp;
    }
    public SceneActorType GetSceneNodeTypeByVec2(Vector2Int _v)
    {
        var _temp = GetIndexByVec2(_v);
        if (_temp < 0 || _temp >= sceneStateList.Count)
        {
            return SceneActorType.Obstacle;
        }
        return sceneStateList[_temp];
    }
    #endregion


    #region Enemy Manager
    public IEnumerator IE_EnemyAction()
    {
        for(var i = 0; i < enemyList.Count; i++)
        {
            yield return StartCoroutine( enemyList[i].IE_OnceAction());
        }
    }
    #endregion



}
