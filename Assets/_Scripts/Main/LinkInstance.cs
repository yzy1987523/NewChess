/*文件名：LinkInstance.cs
 * 作者：YZY
 * 说明：唯一单例
 * 上次修改时间：2020/3/4 23：09：39 *
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkInstance
{
    #region Parameters
    private static LinkInstance instance;
    private SceneManager sceneManager;

    private PlayerActor mainPlayer;
    GameLevelManager levelManager;
    #endregion
    #region Properties
    public static LinkInstance Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LinkInstance();
            }
            return instance;
        }
    }
    public PlayerActor MainPlayer
    {
        get
        {
            if (mainPlayer == null)
            {
                mainPlayer =SceneManager.GetMainPlayer();
            }
            return mainPlayer;
        }

        set
        {
            mainPlayer = value;
        }
    }
    public SceneManager SceneManager
    {
        get
        {
            if (sceneManager == null)
            {
                sceneManager = Object.FindObjectOfType<SceneManager>();
            }
            return sceneManager;
        }

        set
        {
            sceneManager = value;
        }
    }

    public GameLevelManager LevelManager
    {
        get
        {
            if (levelManager == null)
            {
                levelManager= Object.FindObjectOfType<GameLevelManager>();
            }
            return levelManager;
        }

        set
        {
            levelManager = value;
        }
    }
    #endregion




}
