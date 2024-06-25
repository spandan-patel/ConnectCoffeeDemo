using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [SerializeField]
    List<Tile> selectedTiles = new List<Tile>();

    [SerializeField]
    ColorEnum colorToCollect = ColorEnum.NONE;

    [SerializeField]
    Grid grid;

    [SerializeField]
    List<Tile> tempTiles = new List<Tile>();

    public bool inAddMode = false;

    [SerializeField]
    ColorEnum currentPointerTileColor = ColorEnum.NONE;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;

            grid.CreateGrid();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentPointerTileColor == colorToCollect)
        {
            inAddMode = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            inAddMode = false;
            colorToCollect = ColorEnum.NONE;
            ClearTileList();
        }
    }

    public void ChangePointedTileColor(ColorEnum clickedTile)
    {
        //Debug.Log(clickedTile.ToString());
        currentPointerTileColor = clickedTile;
    }

    public void SetColorToCollect(ColorEnum clickedTile)
    {
        colorToCollect = clickedTile;
    }

    public bool GetCurrentMode()
    {
        return inAddMode;
    }

    public void AddTileToTheList(Tile tile)
    {
        if (!selectedTiles.Contains(tile) && tile.color == colorToCollect)
        {
            if(selectedTiles.Count == 0) 
            {
                selectedTiles.Add(tile);
            }

            for (int i = 0; i < selectedTiles.Count; i++)
            {
                tempTiles.AddRange(grid.GetNeighbours(selectedTiles[i]));
                
                if(tempTiles.Contains(tile))
                {
                    selectedTiles.Add(tile);
                    tempTiles.Clear();
                    break;
                }
                    
                tempTiles.Clear();
            }
        }
    }

    public void ClearTileList()
    {
        List<GameObject> emptyTileIndex = new List<GameObject>();

        foreach (Tile tile in selectedTiles)
        {
            emptyTileIndex.Add(tile.gameObject);

            //Destroy(grid[tile.gridX, tile.gridY]);
        }
        
        grid.FillGrid(emptyTileIndex);

        selectedTiles.Clear();
    }
}
