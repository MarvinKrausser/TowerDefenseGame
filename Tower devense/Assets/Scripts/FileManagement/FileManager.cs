using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    public List<string> laodSaves()
    {
        List<string> files = new List<string>();
        DirectoryInfo saves = new System.IO.DirectoryInfo("Saves");

        foreach (FileInfo file in saves.GetFiles())
        {
            string name = file.Name;
            name = name.Substring(0, name.Length - 5);
            
            files.Add(name);
        }

        return files;
    }

    public void saveFileJson(string name, List<List<int>> map)
    {
        string json = "{\'map\':[";

        for (int x = 0; x < map.Count; x++)
        {
            json += "[";
            for (int y = 0; y < map[x].Count; y++)
            {
                switch (map[x][y])
                {
                    case 1:
                        json += "\'GRASS\'";
                        break;
                    case 2:
                        json += "\'OBSTACLE\'";
                        break;
                    case 3:
                        json += "\'START\'";
                        break;
                    case 4:
                        json += "\'END\'";
                        break;
                }

                json += ",";
            }

            json = json.Substring(0, json.Length - 1);
            json += "],";
        }

        json = json.Substring(0, json.Length - 1);
        json += "]}";

        StreamWriter os = new StreamWriter("Saves\\" + name + ".json", false);

        os.Write(json);

        os.Close();
    }
    
    public void deleteFile(string file)
    {
        File.Delete("Saves\\" + file + ".json");
    }

    public List<List<int>> loadFileJson(string name)
    {
        string input = "";
        try
        {
            using (StreamReader sr = new StreamReader("Saves\\" + name + ".json"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    input += line;
                }
            }
        }
        catch (Exception e) {}
        
        
        List<List<int>> map = new List<List<int>>();
        map.Add(new List<int>());
        map[0].Add(0);
        

        int x = 0;
        int y = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '\'' || input[i] == '\"')
            {
                if (input[i + 1] == 'G')
                {
                    map[x][y] = 1;
                    i += 6;
                }
                else if (input[i + 1] == 'O')
                {
                    map[x][y] = 2;
                    i += 9;
                }
                else if (input[i + 1] == 'S')
                {
                    map[x][y] = 3;
                    i += 6;
                }
                else if (input[i + 1] == 'E')
                {
                    map[x][y] = 4;
                    i += 4;
                }
            }
            else if (input[i] == ',')
            {
                if (input[i - 1] == '\'' || input[i - 1] == '\"')
                {
                    y++;
                    while (y >= map[0].Count)
                    {
                        for (int j = 0; j < map.Count; j++)
                        {
                            map[j].Add(0);
                        }
                    }
                }
                else if (input[i - 1] == ']')
                {
                    x++;
                    y = 0;
                    while(x >= map.Count)
                    {
                        map.Add(new List<int>());
                        for (int j = 0; j < map[0].Count; j++)
                        {
                            map[map.Count - 1].Add(0);
                        }
                    }
                }
            }
        }
        return map;
    }

    public bool validateMap(string text, List<List<int>> map)
    {
        AStar aStar = new AStar();
        bool end = false;
        bool start = false;

        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[0].Count; y++)
            {
                //check for invalid FieldTypes
                if (map[x][y] < 1 || map[x][y] > 4)
                {
                    return false;
                }
                else if (map[x][y] == 3)
                {
                    //Check if the StartTile is at the Edge of the Map
                    if (x != 0 && x != map.Count - 1 && y != 0 && y != map[0].Count - 1)
                    {
                        return false;
                    }

                    //check if there already is a StartTile
                    if (start)
                    {
                        return false;
                    }
                    else
                    {
                        start = true;
                    }
                }
                else if (map[x][y] == 4)
                {
                    //Check if the EndTile is at the Edge of the Map
                    if (x != 0 && x != map.Count - 1 && y != 0 && y != map[0].Count - 1)
                    {
                        return false;
                    }

                    //check if there already is a EndTile
                    if (end)
                    {
                        return false;
                    }
                    else
                    {
                        end = true;
                    }
                }
            }
        }

        if (start && end && (text.Length > 0))
        {
            //check if there is a valid way from Start to End
            aStar.setNodes(map);
            if (aStar.calculatePath() != null)
            {
                return true;
            }
        }

        return false;
    }
}
