using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class LevelManagerEditorScript : MonoBehaviour
{
    public Slider sliderX;
    public Slider sliderY;
    private int width = 2;
    private int height = 2;
    private int brushPower = 0;
    public GameObject level;
    public LevelScript levelScript;
    public TMP_Dropdown saveList;

    public GameObject textX;
    public GameObject textY;

    public Button[] buttons;

    private List<List<int>> map;
    
    private Color bright = new Color(128f/255f, 253f/255f, 232f/255f);
    private Color dark = new Color(105f/255f, 215f/255f, 196f/255f);
    
    private int fieldType = 0;
    public Camera camera;

    public GameObject inputField;
    
    FileManager fileManager = new FileManager();
    
    private AStar aStar = new AStar();

    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        if (fieldType != 0 && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !saveList.IsExpanded)
        {
            Vector2Int co = levelScript.coordinatesToField(camera.ScreenToWorldPoint(pos));
            if (brushPower == 0)
            {
                if (co.x >= 0 && co.x < map.Count && co.y >= 0 && co.y < map[0].Count)
                {
                    //if (map[(int)co.x + x][(int)co.y + y] != fieldType)
                    //{
                    map[co.x][co.y] = fieldType;
                    //}
                }
            }
            for (int x = - brushPower; x < brushPower; x++)
            {
                for (int y = -brushPower; y < brushPower; y++)
                {
                    if (co.x + x >= 0 && co.x + x < map.Count && co.y + y >= 0 && co.y + y < map[0].Count)
                    {
                        //if (map[(int)co.x + x][(int)co.y + y] != fieldType)
                        //{
                            map[co.x + x][co.y + y] = fieldType;
                        //}
                    }
                }
            }
            levelScript.createLevel(map);
        }
    }

    public void changeBrushPower(float power)
    {
        brushPower = (int)power;
    }

    public void changeFieldType(int i)
    {
        if (fieldType == i)
        {
            fieldType = 0;
            buttons[i - 1].image.color = bright;
        }
        else
        {
            if (fieldType != 0)
            {
                buttons[fieldType - 1].image.color = bright;
            }
            fieldType = i;
            buttons[i - 1].image.color = dark;
        }
    }

    public void changeSize()
    {
        width = (int)sliderX.value;
        height = (int)sliderY.value;
        
        textX.GetComponent<TextMeshProUGUI>().SetText(width.ToString());
        textY.GetComponent<TextMeshProUGUI>().SetText(height.ToString());

        
        while(width < map.Count)
        {
            map.RemoveAt(map.Count - 1);
        }

        while (width > map.Count)
        {
            map.Add(new List<int>());
            for (int i = 0; i < map[0].Count; i++)
            {
                map[map.Count - 1].Add(0);
            }
        }

        while (height < map[0].Count)
        {
            for (int i = 0; i < map.Count; i++)
            {
                map[i].RemoveAt(map[i].Count - 1);
            }
        }

        while (height > map[0].Count)
        {
            for (int i = 0; i < map.Count; i++)
            {
                map[i].Add(0);
            }
        }


        levelScript.changeSize(width, height, map);
    }
    
    public void deleteFile()
    {
        if (saveList.value >= saveList.options.Count)
        {
            return;
        }
        string file = saveList.options[saveList.value].text;
        
        fileManager.deleteFile(file);

        laodSaves();
    }

    private void Start()
    {
        laodSaves();
        
        foreach(Button button in buttons)
        {
            button.image.color = bright;
        }
        map = new List<List<int>>();
        map.Add(new List<int>());
        map.Add(new List<int>());
        map[0].Add(0);
        map[0].Add(0);
        map[1].Add(0);
        map[1].Add(0);

        levelScript = level.GetComponent<LevelScript>();
        
        levelScript.changeSize(width, height, map);
        textX.GetComponent<TextMeshProUGUI>().SetText(2.ToString());
        textY.GetComponent<TextMeshProUGUI>().SetText(2.ToString());
    }

    public void save()
    {
        //Get Filename from the InputField
        string name = inputField.GetComponent<TMP_InputField>().text;
        
        //Check if the Map is valid
        if (fileManager.validateMap(name, map))
        {
            //Save the Map as a Json File
            fileManager.saveFileJson(name, map);
            
            //Rewrite the FileMenu
            laodSaves();

            //Indicate the save by Colourchange of the InputField
            StartCoroutine(indicateSave());
        }
        else
        {
            //Indicate the not save by Colourchange of the InputField
            StartCoroutine(indicateError());
        }
    }

    IEnumerator indicateSave()
    {
        inputField.GetComponent<Image>().color = Color.green;
        
        yield return new WaitForSeconds(0.1f);
        
        inputField.GetComponent<Image>().color = Color.white;
    }
    
    IEnumerator indicateError()
    {
        inputField.GetComponent<Image>().color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        
        inputField.GetComponent<Image>().color = Color.white;
    }
    

    private void laodSaves()
    {
        saveList.options.Clear();

        foreach (string name in fileManager.laodSaves())
        {
            saveList.options.Add(new TMP_Dropdown.OptionData(name));
        }
    }

    public void loadSelectedSave()
    {
        if (saveList.value >= saveList.options.Count)
        {
            return;
        }
        string name = saveList.options[saveList.value].text;

        map = fileManager.loadFileJson(name);
        
        levelScript.changeSize(map.Count, map[0].Count, map);
    }
}