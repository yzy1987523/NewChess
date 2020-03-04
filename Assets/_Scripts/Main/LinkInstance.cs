using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkInstance
{
    private static LinkInstance instance;   
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
}
