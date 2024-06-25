using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ColorEnum
{
    RED,
    GREEN,
    BLUE,
    NONE
};

[Serializable]
public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int gridX;
    public int gridY;

    public ColorEnum color;

    public GameManager gameManager;

    public Image buttonImage;

    public void AssignValue(int _gridX, int _gridY, int _color)
    {
        gridX = _gridX;
        gridY = _gridY;

        switch (_color) 
        {
            case 0:
                color = ColorEnum.RED;
                buttonImage.color = Color.red;
                break;

            case 1:
                color = ColorEnum.GREEN;
                buttonImage.color = Color.green;
                break;

            case 2:
                color = ColorEnum.BLUE;
                buttonImage.color = Color.blue;
                break;

            default:
                break;
        }

        gameManager = GameManager.gameManagerInstance;
    }

    public void AddTile()
    {
        gameManager.AddTileToTheList(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.ChangePointedTileColor(this.color);

        if (!gameManager.GetCurrentMode())
        {
            gameManager.SetColorToCollect(this.color);
            return;
        }

        gameManager.AddTileToTheList(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.ChangePointedTileColor(this.color);

        if (!gameManager.GetCurrentMode())
            return;

        gameManager.AddTileToTheList(this);
    }
}
