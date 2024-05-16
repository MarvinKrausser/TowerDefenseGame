using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManagerScript : MonoBehaviour
{
    public TMP_Dropdown saveList;

    private FileManager fileManager = new FileManager();

    public LevelLoader levelLoader;

    public Image buttonImage;
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SoundManagerScript>().play("GameMusic");
        laodSaves();
    }

    public void startGame()
    {
        if (saveList.options.Count > 0)
        {
            string name = saveList.options[saveList.value].text;
            if (fileManager.validateMap(name, fileManager.loadFileJson(name)))
            {
                LevelData.name = saveList.options[saveList.value].text;
                levelLoader.startTransition(2);
            }
            else
            {
                StartCoroutine(indicateError());
            }
        }
    }
    
    IEnumerator indicateError()
    {
        buttonImage.color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        
        buttonImage.color = Color.white;
    }
    
    private void laodSaves()
    {
        saveList.options.Clear();

        foreach (string name in fileManager.laodSaves())
        {
            saveList.options.Add(new TMP_Dropdown.OptionData(name));
        }
    }
}
