using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestValidSaveGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Validate Map:");
        FileManager fileManager = new FileManager();
        if (fileManager.validateMap("valid", fileManager.loadFileJson("valid")))
        {
            Debug.Log("Test 1: success");
        }
        else
        {
            Debug.Log("Test 1: failed");
        }

        if (fileManager.validateMap("invalid", fileManager.loadFileJson("invalid")))
        {
            Debug.Log("Test 2: failed");
        }
        else
        {
            Debug.Log("Test 2: success");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}