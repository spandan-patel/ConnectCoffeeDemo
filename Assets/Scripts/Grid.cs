using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float tileRadius;

    Tile[,] grid;

    float tileDiameter;
    int gridSizeX, gridSizeY;

    public Transform gridBase;

    RectTransform gridRectTransform;
    GridLayoutGroup gridLayoutGroup;

    public GameObject tileButtonPrefab;

    void Awake()
    {
        tileDiameter = tileRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / tileDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / tileDiameter);

        gridRectTransform = gridBase.GetComponent<RectTransform>();

        if(gridRectTransform != null )
        {
            gridBase.GetComponent<RectTransform>().sizeDelta = gridWorldSize;
        }
        
        gridLayoutGroup = gridBase.GetComponent<GridLayoutGroup>();

        if(gridLayoutGroup != null )
        {
            gridLayoutGroup.cellSize = new Vector2(tileDiameter, tileDiameter);
            gridLayoutGroup.constraintCount = gridSizeY;
        }

        //CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        
        for (int y = 0; y < gridSizeX; y++)
        {
            for (int x = 0; x < gridSizeY; x++)
            {
                int tempIndex = UnityEngine.Random.Range(0, 3);

                GameObject newTileButton = Instantiate(tileButtonPrefab, gridBase);
                //Debug.Log(newTileButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text);
                //newTileButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (x+1).ToString() + ", " + (y+1).ToString();

                newTileButton.GetComponent<Tile>().AssignValue(x, y, tempIndex);
                grid[x, y] = newTileButton.GetComponent<Tile>();
            }
        }
    }

    public List<Tile> GetNeighbours(Tile node)
    {
        List<Tile> neighbours = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Tile TileFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    internal void FillGrid(List<GameObject> emptyTileIndex)
    {
        int[] emptyTileInColumn = new int[gridSizeX];

        for(int i = 0; i < gridSizeX; i++)
        {
            emptyTileInColumn[i] = 0;

            for(int j = 0; j < emptyTileIndex.Count; j++)
            {
                if(i == emptyTileIndex[j].GetComponent<Tile>().gridX)
                {
                    emptyTileInColumn[i] += 1;
                }
            }

            for(int j = 0, count = 0; j < gridSizeY; j++)
            {
                if (emptyTileInColumn[i] == 0)
                    break;

                //Debug.Log(grid[i, j].gridX + " , " + grid[i, j].gridY);
            }

            Debug.Log(emptyTileInColumn[i]);
        }
    }
}
