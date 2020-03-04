/*文件名：AStarTool.cs
 * 作者：YZY
 * 说明：A*算法
 * 注意：
 * 1.算法是按斜线走，如果是不能走斜线，需要角色朝目标点移动（而不是移动到目标点），按最小旋转方向移动
 * 2.当出现特殊角落（对角位置是NormalNode）时，可能会卡在角落，所以在算法中需要添加判断：角落的Node的相邻两个node如果是障碍，则该node移入closelist
 * 上次修改时间：2020/3/3 22:40:13 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTool : MonoBehaviour
{
    static List<NodeClass> nodes;
    static NodeClass startNode;
    static NodeClass endNode;
    static Vector2Int size = new Vector2Int(16, 10);
    #region Test
    public Vector2Int obstacleNodeCount = new Vector2Int(5, 10);
    public GameObject[] nodeObj;
    //随机生成若干格子，包括起点，终点，障碍
    [ContextMenu("RCreateNode")]
    public void RCreateNode()
    {
        Destroy();
        var _nodeCount = size.x * size.y;
        //var _indexList = StaticFun.GetRandomSortList(size.x * size.y);
        var _tempNodes = new List<NodeClass>();
        //设置起点
        startNode = new NodeClass();
        startNode.nodeType = NodeType.StartNode;
        //startNode.nodePos = GetPosByIndex(_indexList[0]);
        _tempNodes.Add(startNode);
        //设置终点
        endNode = new NodeClass();
        endNode.nodeType = NodeType.EndNode;
        //endNode.nodePos = GetPosByIndex(_indexList[1]);
        _tempNodes.Add(endNode);
        //设置障碍物
        var _obstacleNodeCount = Random.Range(obstacleNodeCount.x, obstacleNodeCount.y);
        for (var i = 2; i < _obstacleNodeCount + 2; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.ObstacleNode;
            //_temp.nodePos = GetPosByIndex(_indexList[i]);
            _tempNodes.Add(_temp);
        }
        //设置正常node
        for (var i = _obstacleNodeCount + 2; i < _nodeCount; i++)
        {
            var _temp = new NodeClass();
            _temp.nodeType = NodeType.NormalNode;
            //_temp.nodePos = GetPosByIndex(_indexList[i]);
            _tempNodes.Add(_temp);
        }
        //给nodes赋值
        nodes = StaticFun.RandomSort(_tempNodes);
        for (var i = 0; i < nodes.Count; i++)
        {
            nodes[i].nodePos = GetPosByIndex(i);
            CreateNode(nodes[i]);
        }
        Test();

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
    void CreateNode(NodeClass _node)
    {
        var _obj = Instantiate(nodeObj[(int)_node.nodeType]);
        _obj.transform.SetParent(transform);
        _obj.transform.localPosition = GetPosByVec2(_node.nodePos);
    }
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

    #region Test    
    void Test()
    {
        var _endNode = CoreAlgorithm(startNode);
        while (_endNode != null)
        {
            Debug.Log(_endNode.nodePos);
            _endNode = _endNode.parentNode;
        }
    }
    #endregion
    #endregion

    #region Core Algorithm
    static List<NodeClass> openList = new List<NodeClass>();
    static List<NodeClass> closeList = new List<NodeClass>();
    static NodeClass CoreAlgorithm(NodeClass _node)
    {

        openList.Remove(_node);
        closeList.Add(_node);
        //将起点放入openList
        //计算起点的8个方向上的F值，并放入openList(排除在closeLsit的node及障碍)，并将起点放入closeList【注意判断是否包含end】
        for (var i = 0; i < 8; i++)
        {
            var _pos = GetNodePos(_node.nodePos, i);
            var _index = GetNodeIndex(_pos);
            if (_index >= 0 && _index < nodes.Count)
            {
                if (closeList.Contains(nodes[_index]) || nodes[_index].nodeType == NodeType.ObstacleNode)
                {
                    continue;
                }
                //如果是角落,需要特殊判断
                if (i % 2 == 0)
                {
                    var _left = i == 0 ? 7 : i - 1;
                    var _right = i + 1;
                    var _tempLeftIndex = GetNodeIndex(GetNodePos(_node.nodePos, _left));
                    var _tempRightIndex = GetNodeIndex(GetNodePos(_node.nodePos, _right));
                    if (nodes[_tempLeftIndex].nodeType == NodeType.ObstacleNode && nodes[_tempRightIndex].nodeType == NodeType.ObstacleNode)
                    {
                        closeList.Add(nodes[_index]);
                        continue;
                    }
                }

                if (nodes[_index].nodeType == NodeType.EndNode)
                {
                    //找到终点，最短路径为当前node开始回溯parentNode
                    nodes[_index].parentNode = _node;
                    return nodes[_index];
                }
                else
                {
                    var _G = GetG(_pos, _node);
                    var _H = GetH(_pos);
                    //已经在openlist中了
                    if (openList.Contains(nodes[_index]))
                    {
                        if (nodes[_index].G > _G)
                        {
                            nodes[_index].G = _G;
                            nodes[_index].F = _G + _H;
                            nodes[_index].parentNode = _node;
                            openList.Remove(nodes[_index]);
                            AddToOpenlist(nodes[_index], 0, openList.Count - 1);
                        }
                    }
                    //不再就加入openlist
                    else
                    {
                        nodes[_index].G = _G;
                        nodes[_index].F = _G + _H;
                        nodes[_index].parentNode = _node;
                        AddToOpenlist(nodes[_index], 0, openList.Count - 1);
                    }
                }
            }
        }


        //判断openlist是否为空
        if (openList.Count == 0)
        {
            return null;
        }

        //选取F值最小的node（排除在closeLsit的node），重复上述操作
        return CoreAlgorithm(openList[0]);
    }
    //按照一定的顺序返回8个方向的坐标（从左上开始顺时针绕）
    static Vector2Int GetNodePos(Vector2Int _pos, int _index)
    {
        switch (_index)
        {
            case 0:
                return new Vector2Int(_pos.x - 1, _pos.y + 1);
            case 1:
                return new Vector2Int(_pos.x, _pos.y + 1);
            case 2:
                return new Vector2Int(_pos.x + 1, _pos.y + 1);
            case 3:
                return new Vector2Int(_pos.x + 1, _pos.y);
            case 4:
                return new Vector2Int(_pos.x + 1, _pos.y - 1);
            case 5:
                return new Vector2Int(_pos.x, _pos.y - 1);
            case 6:
                return new Vector2Int(_pos.x - 1, _pos.y - 1);
            case 7:
                return new Vector2Int(_pos.x - 1, _pos.y);
        }
        return default;
    }
    //根据坐标获取NodeIndex
    static int GetNodeIndex(Vector2Int _pos)
    {
        return _pos.x + _pos.y * size.x;
    }
    //节点到终点的距离，用曼哈顿方法
    static int GetH(Vector2Int _pos)
    {
        return (Mathf.Abs(_pos.x - endNode.nodePos.x) + Mathf.Abs(_pos.y - endNode.nodePos.y)) * 10;
    }
    //节点到父节点的距离+父节点的G
    static int GetG(Vector2Int _a, NodeClass _b)
    {
        return ((_a.x == _b.nodePos.x || _a.y == _b.nodePos.y) ? 10 : 14) + _b.G;
    }

    //加入到openlist，注意需要按从小到大排序
    static void AddToOpenlist(NodeClass _node, int _start, int _end)
    {

        if (openList.Count == 0)
        {
            openList.Add(_node);
            return;
        }
        var _mid = (_start + _end) / 2;

        if (_node.F < openList[_mid].F)
        {
            _end = _mid;
        }
        else if (_node.F >= openList[_mid].F)
        {
            _start = _mid;
        }
        if (_start == _end || _end - _start == 1)
        {
            openList.Insert(_start, _node);
        }
        else
        {
            AddToOpenlist(_node, _start, _end);
        }

    }
    #endregion   

    #region ToUse    
    static NodeClass GetClosePath(List<NodeClass> _nodes, Vector2Int _size, Vector2Int _start, Vector2Int _end)
    {
        nodes = _nodes;
        size = _size;
        openList.Clear();
        closeList.Clear();
        startNode = new NodeClass();
        startNode.nodePos = _start;
        endNode = new NodeClass();
        endNode.nodeType = NodeType.EndNode;
        endNode.nodePos = _end;
        return CoreAlgorithm(startNode);
    }

    //用前需要设置Nodes
    public static void SetNodes(List<NodeClass> _nodes, Vector2Int _size)
    {
        nodes = _nodes;
        size = _size;
    }
    //获取最短路径
    public static NodeClass GetClosePath(Vector2Int _start, Vector2Int _end)
    {
        return GetClosePath(nodes, size, _start, _end);
    }
    //灵活的敌人一般用该方法：只朝向目标移动一次，下次再重新计算最短路径
    public static Vector2Int GetNextPos(Vector2Int _start, Vector2Int _end)
    {
        return GetClosePath(_start, _end).parentNode.nodePos;
    }
    #endregion
}

public class NodeClass
{
    public Vector2Int nodePos;
    public NodeType nodeType;
    public int F;
    public int G;
    public int H;
    public NodeClass parentNode;
}
public enum NodeType
{
    NormalNode,
    StartNode,
    EndNode,
    ObstacleNode,
    HardNode,//可以通行，但是耗费更多体力
}