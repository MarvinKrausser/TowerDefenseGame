using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerPlacing : MonoBehaviour
{

    private int testCounter = 0;
    public LevelManagerGameScript levelManager;
    
    
    void Start()
    {
        //Check if right Testlevel is used
        if (!LevelData.name.Equals("TowerPlacingTest"))
        {
            return;
        }
        Debug.Log("Tower Placing:");
        test(new Vector2Int(0,0), true);
        test(new Vector2Int(2,0), false);
        test(new Vector2Int(3,2), false);
    }

    private void test(Vector2Int co, bool expected)
    {
        testCounter++;
        if (levelManager.isTowerPlaceable(co) == expected)
        {
            Debug.Log("Test " + testCounter + ": success");
        }
        else
        {
            Debug.Log("Test " + testCounter + ": failed");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
