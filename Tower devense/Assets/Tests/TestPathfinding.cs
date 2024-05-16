using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{

    private int[,] map;

    private Vector2Int[] path;
    private AStar aStar = new AStar();

    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Pathfinding:");
        map = new int[,] {      { 3, 1, 1 } ,
                                { 1, 1, 1 } ,
                                { 4, 1, 1 } };
        aStar.setNodes(map);
        path = new Vector2Int[] { new Vector2Int(0, 0) , new Vector2Int(1,0), new Vector2Int(2,0)};
        calculate();
        
        map = new int[,] {      { 3, 1, 1 } ,
                                { 2, 1, 1 } ,
                                { 4, 1, 1 } };
        aStar.setNodes(map);
        path = new Vector2Int[] { new Vector2Int(0, 0) , new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1), new Vector2Int(2,0)};
        calculate();

        map = new int[,] {      { 3, 1, 1 } ,
                                { 2, 5, 1 } ,
                                { 4, 1, 1 } };
        aStar.setNodes(map);
        path = new Vector2Int[]
        {
            new Vector2Int(0, 0) , new Vector2Int(0,1), new Vector2Int(0,2), new Vector2Int(1,2), new Vector2Int(2,2),
            new Vector2Int(2,1), new Vector2Int(2,0)
        };
        calculate();
        
        map = new int[,] {      { 3, 1, 1 } ,
                                { 1, 1, 1 } ,
                                { 1, 1, 1 } ,
                                { 4, 1, 1 } };
        aStar.setNodes(map);
        path = new Vector2Int[] { new Vector2Int(0, 0) , new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(3,0)};
        calculate();
        
        map = new int[,] {      { 1, 1, 3 } ,
                                { 1, 1, 1 } ,
                                { 1, 4, 1 } };
        aStar.setNodes(map);
        path = new Vector2Int[] { new Vector2Int(0, 2) , new Vector2Int(1,2), new Vector2Int(1,1), new Vector2Int(2,1)};
        calculate();
    }

    private void calculate()
    {
        counter++;
        if (equalsVector(path, aStar.calculatePath()))
        {
            Debug.Log("Test " + counter + ": success");
        }
        else
        {
            Debug.Log("Test " + counter + ": failed");
        }
    }

    private bool equalsVector(Vector2Int[] p, Vector2Int[] q)
    {
        if (p.Length != q.Length)
        {
            return false;
        }

        for (int i = 0; i < q.Length; i++)
        {
            if (q[i] != p[i])
            {
                return false;
            }
        }

        return true;
    }
}
