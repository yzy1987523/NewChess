/*文件名：SimpleMeshCreateTool.cs
 * 作者：YZY
 * 说明：简单的mesh生成工具：片/圆柱/方盒
 * 上次修改时间：
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleMeshCreateTool : MonoBehaviour
{  

   
    public Material[] mat;   
    private List<Vector2> m_points = new List<Vector2>();
    private Transform[] points;
    private Transform heightPoint;

    public Transform[] Points
    {
        get
        {
            if (points == null||points.Length==0)
            {
                points = transform.FindInAll("Points").GetChildren().ToArray();
            }
            return points;
        }

        set
        {
            points = value;
        }
    }

    public Transform HeightPoint
    {
        get
        {
            if (heightPoint == null)
            {
                heightPoint = transform.FindInAll("Height");
            }
            return heightPoint;
        }

        set
        {
            heightPoint = value;
        }
    }

    public GameObject CreateCube(ModelData data)
    {
        var height = data.height;
        float[] px = data.POINT_X;
        float[] py = data.POINT_Y;
        var pointLength = px.Length * 2;
        Vector3[] points = new Vector3[pointLength];
        points[0] = new Vector3(0, 0, 0);
        points[1] = points[0] + Vector3.up * height;

        var topMeshIndices = new Vector3[px.Length];
        topMeshIndices[0] = points[1];
        for (var i = 2; i < pointLength; i += 2)
        {
            var x = (float)((px[i / 2] - px[0]) );
            var y = (float)((py[i / 2] - py[0]) );
            points[i] = new Vector3(x, 0, y);
            points[i + 1] = points[i] + Vector3.up * height ;
            topMeshIndices[i / 2] = points[i + 1];
        }
        Triangulator(topMeshIndices);
        int[] tempIndex0 = Triangulate();
        var topUv = new Vector2[tempIndex0.Length];

        for (var i = 0; i < tempIndex0.Length; i++)
        {
            topUv[i] = new Vector2(0, 0);
        }
        //再绘制侧面      
        int[] tempIndex1 = new int[px.Length * 6];
        Vector2[] sideUv = new Vector2[tempIndex1.Length];
        var maxIndex = (px.Length - 1) * 2;//4边时为6
        for (int i = 0; i < tempIndex1.Length; i += 3)
        {
            if (i % 2 == 0)
            {
                tempIndex1[i] = i / 3;
                tempIndex1[i + 2] = tempIndex1[i] + 1;
                if (i == maxIndex * 3)
                {
                    tempIndex1[i + 1] = 0;
                }
                else
                {
                    tempIndex1[i + 1] = tempIndex1[i] + 2;
                }
                sideUv[i] = new Vector2(0, 0);
                sideUv[i + 2] = new Vector2(0, 1);
                sideUv[i + 1] = new Vector2(1, 0);
            }
            else
            {
                if (i == (maxIndex + 1) * 3)
                {
                    tempIndex1[i] = 1;
                    tempIndex1[i + 2] = 0;
                    tempIndex1[i + 1] = maxIndex + 1;
                    sideUv[i] = new Vector2(1, 1);
                    sideUv[i + 2] = new Vector2(1, 0);
                    sideUv[i + 1] = new Vector2(0, 1);
                }
                else
                {
                    tempIndex1[i] = tempIndex1[i - 3] + 2;
                    tempIndex1[i + 2] = tempIndex1[i] - 1;
                    tempIndex1[i + 1] = tempIndex1[i] + 1;
                    sideUv[i] = new Vector2(1, 0);
                    sideUv[i + 2] = new Vector2(0, 1);
                    sideUv[i + 1] = new Vector2(1, 1);
                }

            }       
        }

        Mesh mesh = new Mesh();
        var _count = tempIndex0.Length + tempIndex1.Length;
        var vertices = new Vector3[_count];
        var newArray = new int[_count];
        var uv = new Vector2[_count];
        for (var i = 0; i < _count; i++)
        {
            if (i >= tempIndex0.Length)
            {
                vertices[i] = points[tempIndex1[i - tempIndex0.Length]];
                uv[i] = sideUv[i - topUv.Length];
            }
            else
            {
                vertices[i] = points[tempIndex0[i] * 2 + 1];
                uv[i] = topUv[i];

            }
            newArray[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = newArray;
        mesh.uv = uv;
        var obj = new GameObject();
        obj.name = "Mesh";
        var meshfilter = obj.AddComponent<MeshFilter>();
        var mr = obj.AddComponent<MeshRenderer>();
        if(mat!=null&&mat.Length> data.typeValue)
        mr.material = mat[data.typeValue];
        meshfilter.mesh = mesh;     

        return obj;
    }

    public GameObject CreatePanel(ModelData data)
    {       
        float[] px = data.POINT_X;
        float[] py = data.POINT_Y;
        var pointLength = px.Length;
        Vector3[] points = new Vector3[pointLength];      
        var _normals = new Vector3[px.Length];       
        
        for (var i = 0; i < pointLength; i ++)
        {
            _normals[i] = new Vector3(0, 1, 0);
            points[i] = new Vector3(px[i], 0, py[i]);
        }
        Triangulator(points);
        int[] tempIndex0 = Triangulate();
        Mesh mesh = new Mesh();                 
        mesh.vertices = points;
        mesh.triangles = tempIndex0;
        mesh.uv = m_points.ToArray();
        mesh.normals = _normals;
        var obj = new GameObject();
        obj.name = "Mesh";
        var meshfilter = obj.AddComponent<MeshFilter>();
        var mr = obj.AddComponent<MeshRenderer>();
        if (mat != null && mat.Length > data.typeValue)
            mr.material = mat[data.typeValue];
        meshfilter.mesh = mesh;

        return obj;
    }
    [ContextMenu("CreateCube")]
    public  void CreateCube()
    {
        points = null;
        heightPoint = null;
        var _data = new ModelData();
        _data.typeValue = 0;
        _data.height = HeightPoint.position.y-Points[0].position.y;
        _data.POINT_X = new float[Points.Length];
        _data.POINT_Y = new float[Points.Length];
        for(var i=0;i< Points.Length; i++)
        {
            _data.POINT_X[i] = Points[i].localPosition.x;
            _data.POINT_Y[i] = Points[i].localPosition.z;
        }
        CreateCube(_data);
    }

    [ContextMenu("CreatePanel")]
    public void CreatePanel()
    {
        points = null;;
        var _data = new ModelData();
        _data.typeValue = 0;
        _data.POINT_X = new float[Points.Length];
        _data.POINT_Y = new float[Points.Length];
        for (var i = 0; i < Points.Length; i++)
        {
            _data.POINT_X[i] = Points[i].localPosition.x;
            _data.POINT_Y[i] = Points[i].localPosition.z;
        }
        CreatePanel(_data);
    }
    #region TopMeshDraw
    public void Triangulator(Vector3[] points)
    {
        var point2D = new Vector2[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            point2D[i] = new Vector2(points[i].x, points[i].z);
        }
        m_points = new List<Vector2>(point2D);
    }
    public int[] Triangulate()
    {
        List<int> indices = new List<int>();
        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();
        int[] V = new int[n];
        //保证面朝上
        if (Area() > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }
        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices.ToArray();
            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;
            if (Snip(u, v, w, nv, V))
            {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }
        indices.Reverse();
        return indices.ToArray();
    }
    //求mesh的面积，正为顺，负为逆
    private float Area()
    {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;//叉乘公式，求平行四边形面积；按顺序计算面积，有的是负，有的是正，和为多边形面积
        }
        return (A * 0.5f);
    }
    //但凡有一个点在三角形内，返回为false
    private bool Snip(int u, int v, int w, int n, int[] V)
    {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        //假如有2点很近，则返回为false
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }
    //判断点P是否在abc内
    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;
        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;
        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;
        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
    #endregion
}

//柱形结构，底面可以是任意形状
[System.Serializable]
public struct ModelData
{ 
    public int typeValue;//model类型，目前用来确认材质类型
    public float height;
    public float[] POINT_X;
    public float[] POINT_Y;
}