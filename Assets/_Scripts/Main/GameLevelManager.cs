/*文件名：GameLevelManager.cs
 * 作者：Yzy
 * 说明：管理关卡
 * 上次修改时间：
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public void StartLevel(int _index)
    {
        var _data = new LevelData();
        _data.followCount =new Vector2Int( _index+2,_index+2);
        //_data.wallCount = new Vector2Int();
        LinkInstance.Instance.SceneManager.RCreateNode(_data);
        LinkInstance.Instance.MainPlayer.StartGame();
    }
    public void LevelEnd()
    {
        StartLevel(LinkInstance.Instance.MainPlayer.RoleTemplateActors.Count);
    }
}
