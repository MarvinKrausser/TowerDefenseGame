using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LevelManagerGameScript : MonoBehaviour
{
    private int[,] map;
    private FileManager fileManager = new FileManager();
    private AStar aStar = new AStar();

    private Vector2Int[] path;
    public static List<GameObject> activeEnemys = new List<GameObject>();
    public static int towerMode = 3;
    public PlayerMoneyScript playerMoneyScript;

    public GameObject[] enemys;
    public PlayerHealthScript player;
    public GameObject[] tower;
    public TowerData[] towerData;
    private int towerIndex = -1;
    public Image[] towerButtons;

    public Camera camera;
    
    private Color bright = new Color(1, 1, 1);
    private Color dark = new Color(0.5f, 0.5f, 0.5f);

    [SerializeField] private LevelScript level;

    private int waveCount = 0;
    private bool waveIsSpawned = true;

    // Start is called before the first frame update
    void Start()
    {
        LevelData.score = 0;
        towerMode = 0;
        activeEnemys = new List<GameObject>();
        List<List<int>> listMap = fileManager.loadFileJson(LevelData.name);
        map = new int[listMap.Count, listMap[0].Count];

        for (int x = 0; x < listMap.Count; x++)
        {
            for (int y = 0; y < listMap[0].Count; y++)
            {
                map[x, y] = listMap[x][y];
            }
        }
        
        aStar.setNodes(map);
        path = aStar.calculatePath();
        
        level.calculateObstacle(map);
        level.changeSize(map.GetLength(0), map.GetLength(1), map, path);

        for(int i = 0; i < towerButtons.Length; i++)
        {
            TextMeshProUGUI text = towerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            text.text = towerData[i].cost.ToString();
        }
    }

    public void EnemyDead(GameObject enemy, EnemyInterface enemyScript, bool reachedDestiny)
    {
        activeEnemys.Remove(enemy);
        if (reachedDestiny)
        {
            player.getDamage(enemyScript.getData().damage);
        }
        else
        {
            LevelData.score += enemyScript.getData().reward;
            playerMoneyScript.changeMoney(enemyScript.getData().reward);
        }
        Destroy(enemy);
    }

    public void changeTowerMode(float mode)
    {
        towerMode = (int)mode;
    }

    public void changeTowerType(int type)
    {
        if (type == towerIndex)
        {
            towerIndex = -1;
            towerButtons[type].color = bright;
        }
        else
        {
            foreach (Image button in towerButtons)
            {
                button.color = bright;
            }

            towerButtons[type].color = dark;
            towerIndex = type;
        }
    }

    private void placeTower()
    {
        if (isWaveOver() && towerIndex != -1 && Input.GetMouseButtonDown(0) && 
            !EventSystem.current.IsPointerOverGameObject() && 
            playerMoneyScript.getMoney() >= towerData[towerIndex].cost)
        {
            //Get MapCoordinates of Mouse
            Vector2Int index = level.coordinatesToField(camera.ScreenToWorldPoint(Input.mousePosition));
            if (isTowerPlaceable(index))
            {
                Instantiate(tower[towerIndex], level.fieldToCoordinate(index), transform.rotation);
                level.changeSize(map.GetLength(0), map.GetLength(1), map, path);
                playerMoneyScript.changeMoney(-towerData[towerIndex].cost);
            }
        }
    }

    public bool isTowerPlaceable(Vector2Int index)
    {
        //Check if Coordinates are out of Array
        if (index.x >= 0 && index.x < map.GetLength(0) && index.y >= 0 && index.y < map.GetLength(1))
        {
            //Check if Tile is Grass
            if (map[index.x, index.y] == 1)
            {
                //Create Copy of Map and Check if new Map would be walkable
                int[,] mapCopy = (int[,])map.Clone();
                mapCopy[index.x, index.y] = 5;
                aStar.setNodes(mapCopy);
                if (aStar.calculatePath() is null)
                {
                    //path was not valid
                    aStar.setNodes(map);
                    return false;
                }
                else
                {
                    //path was valid Tower gets placed
                    path = aStar.calculatePath();
                    map = mapCopy;
                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        placeTower();
    }

    private bool isWaveOver()
    {
        return activeEnemys.Count == 0 && waveIsSpawned;
    }

    public void nextWave()
    {
        if (isWaveOver())
        {
            waveIsSpawned = false;
            waveCount++;
            StartCoroutine(instantiateEnemy());
        }
    }
    
    IEnumerator instantiateEnemy()
    {
        for (int j = 0; j < enemys.Length; j++)
        {
            for (int i = 0; i < waveCount * 10; i++)
            {
                GameObject enemyCopy = Instantiate(enemys[j], level.fieldToCoordinate(path[0]), transform.rotation);
                EnemyInterface enemyScript = enemyCopy.GetComponent<EnemyInterface>();
                enemyScript.setData(path, level, this);
                activeEnemys.Add(enemyCopy);
                yield return new WaitForSeconds(enemyScript.getData().SpawnRate / waveCount);
            }
        }
        waveIsSpawned = true;
    }
}