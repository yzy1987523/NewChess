/*文件名：RoleTemplateActor.cs
 * 作者：YZY
 * 说明：主角模板：主要是作为随从存在
 * 上次修改时间：2020/3/8 18:02:25 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTemplateActor : VividActor
{
    #region Parameters    


    #endregion
    #region Properties

    #endregion
    #region Private Methods  
    
   
    protected override void ChangeSceneNodes(MoveDir _dir)
    {
        base.ChangeSceneNodes(_dir);
        LinkInstance.Instance.SceneManager.ChangeSceneNodes(Vec2Pos, SceneActorType.Player);
    }

    #endregion
    #region Utility Methods
    
    #endregion
}
