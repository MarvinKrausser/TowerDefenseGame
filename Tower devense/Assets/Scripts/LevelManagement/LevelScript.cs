using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelScript : MonoBehaviour
{
    private int width = 2;
    private int height = 2;
    public float tileSize { get; private set; }
    private MeshFilter meshObject;
    private int[,] obstacle;

    private void Start()
    {
        meshObject = GetComponent<MeshFilter>();
        tileSize = LevelData.tileSize;
    }

    public void changeSize(int x, int y, List<List<int>> map)
    {
        width = x;
        height = y;
        
        transform.position = new Vector3((width * -tileSize) / 2, (height * -tileSize) / 2, 0);
        
        createLevel(map);
    }
    
    public void changeSize(int x, int y, int[,] map, Vector2Int[] path)
    {
        width = x;
        height = y;
        
        transform.position = new Vector3((width * -tileSize) / 2, (height * -tileSize) / 2, 0);
        
        createLevel(map, path);
    }

    public Vector2Int coordinatesToField(Vector3 co)
    {
        return new Vector2Int((int)Mathf.Round((co.x - transform.position.x - (tileSize * 0.5f)) / tileSize), (int)Mathf.Round((co.y - transform.position.y - (tileSize * 0.5f)) / tileSize));
    }
    
    public Vector3 fieldToCoordinate(Vector2 co)
    {
        return new Vector3(co.x * tileSize + (tileSize * 0.5f) + transform.position.x,co.y * tileSize + (tileSize * 0.5f) + transform.position.y, 0);
    }

    public void createLevel(int[,] map, Vector2Int[] path)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * width * height];
        Vector2[] uv = new Vector2[4 * width * height];
        int[] triangles = new int[6 * width * height];


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x * height + y;
                
                vertices[index * 4 + 0] = new Vector3(tileSize * x, tileSize * y);
                vertices[index * 4 + 1] = new Vector3(tileSize * x, tileSize * (y + 1));
                vertices[index * 4 + 2] = new Vector3(tileSize * ( x + 1), tileSize * (y + 1));
                vertices[index * 4 + 3] = new Vector3(tileSize * (x + 1), tileSize * y);

                
                //map 0:no type 1:gras 2:obstacle 3:start 4:end 5:tower
                //obstacle 0:no type
                //sprite 1:white 2:gras 3:tree 4:water 5:stone 6:hole 7:mordor 8:hobbit home 9:path
                
                int i = 0;
                switch (map[x,y])
                {
                    case 1:
                        if (path.Contains(new Vector2Int(x, y)))
                        {
                            i = 9;
                        }
                        else
                        {
                            i = 2;
                        }

                        break;
                    case 5:
                        i = 2;
                        break;
                    case 3:
                        i = 7;
                        break;
                    case 4:
                        i = 8;
                        break;
                    case 2 :
                        i = obstacle[x,y];
                        break;
                }

                int imageWidth = 396;
                //Image bigger than selected UVs to avoid visual bugs; Image 44x44; Selected 40x40
                uv[index * 4 + 0] = convertPixelsToUv(44 * (i - 1) + 2, 0 + 2, imageWidth, 44);
                uv[index * 4 + 1] = convertPixelsToUv(44 * (i - 1) + 2, 44 - 2, imageWidth, 44);
                uv[index * 4 + 2] = convertPixelsToUv(44 * i - 2, 44 - 2, imageWidth, 44);
                uv[index * 4 + 3] = convertPixelsToUv(44 * i - 2, 0 + 2, imageWidth, 44);

                triangles[index * 6 + 0] = index * 4 + 0;
                triangles[index * 6 + 1] = index * 4 + 1;
                triangles[index * 6 + 2] = index * 4 + 2;
                triangles[index * 6 + 3] = index * 4 + 0;
                triangles[index * 6 + 4] = index * 4 + 2;
                triangles[index * 6 + 5] = index * 4 + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        meshObject.mesh = mesh;
    }

    public void calculateObstacle(int[,]map)
    {
        System.Random rnd = new System.Random();
        int[,] obstacle = new int[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == 2 && obstacle[x,y] == 0)
                {
                    obstacle[x, y] = rnd.Next(3, 7);
                    foreach (Vector2Int pos in getNeighbours(map, new Vector2Int(x,y), new List<Vector2Int>()))
                    {
                        obstacle[pos.x, pos.y] = obstacle[x, y];
                    }
                }
            }
        }

        this.obstacle = obstacle;
    }

    private List<Vector2Int> getNeighbours(int[,] map, Vector2Int pos, List<Vector2Int> obstacle)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }

                if (pos.x + x >= 0 && pos.x + x < map.GetLength(0) && pos.y + y >= 0 && pos.y + y < map.GetLength(1))
                {
                    if (map[pos.x + x, pos.y + y] == 2 && !obstacle.Contains(new Vector2Int(pos.x + x, pos.y + y)))
                    {
                        obstacle.Add(new Vector2Int(pos.x + x, pos.y + y));
                        obstacle = getNeighbours(map, new Vector2Int(pos.x + x, pos.y + y), obstacle);
                    }
                }
            }
        }

        return obstacle;
    }

    public void createLevel(List<List<int>> map)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * width * height];
        Vector2[] uv = new Vector2[4 * width * height];
        int[] triangles = new int[6 * width * height];


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x * height + y;
                
                vertices[index * 4 + 0] = new Vector3(tileSize * x, tileSize * y);
                vertices[index * 4 + 1] = new Vector3(tileSize * x, tileSize * (y + 1));
                vertices[index * 4 + 2] = new Vector3(tileSize * ( x + 1), tileSize * (y + 1));
                vertices[index * 4 + 3] = new Vector3(tileSize * (x + 1), tileSize * y);

                int i = map[x][y] + 1;

                uv[index * 4 + 0] = convertPixelsToUv(40 * (i -1), 0, 200, 40);
                uv[index * 4 + 1] = convertPixelsToUv(40 * (i -1), 40, 200, 40);
                uv[index * 4 + 2] = convertPixelsToUv(40 * i, 40, 200, 40);
                uv[index * 4 + 3] = convertPixelsToUv(40 * i, 0, 200, 40);

                triangles[index * 6 + 0] = index * 4 + 0;
                triangles[index * 6 + 1] = index * 4 + 1;
                triangles[index * 6 + 2] = index * 4 + 2;
                triangles[index * 6 + 3] = index * 4 + 0;
                triangles[index * 6 + 4] = index * 4 + 2;
                triangles[index * 6 + 5] = index * 4 + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        meshObject.mesh = mesh;
    }

    private Vector2 convertPixelsToUv(int x, int y, int width, int height)
    {
        return new Vector2((float)x / (float)width, (float)y / (float)height);
    }
}