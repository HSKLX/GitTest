using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SingleItem : MonoBehaviour
{
    public List<Sprite> sprites;
    public int type;
    public SpriteRenderer itemSprite;
    public Vector3 startPos = new Vector3(0, 0, 0);
    public int col;
    public int row;

    public float width;
    public float height;

    public event Action<SingleItem> mouseButton;

    public GameObject block;

    public void BirthItem(int row, int col)
    {
        SetItemIndex(row, col);

        SetItemWorldPos(row, col);

        SetItemSpriteRandom();
    }

    public void SetItemIndex(int r, int c)
    {
        col = c;
        row = r;
    }


    public void SetItemWorldPos(int r, int c)
    {

        transform.position = startPos+ new Vector3(+c * width, - r * height, 0);
    }

    private void SetItemSpriteRandom()
    {
        int index=UnityEngine.Random.Range(0, sprites.Count);
        type=index;
        itemSprite.sprite = sprites[index];
    }

    private void OnMouseDown()
    {
        if (mouseButton != null)
        {
            mouseButton(this);
        }

    }

    public bool CompareTypeWith(SingleItem compareItem)
    {
        if(compareItem.col==col&&Math.Abs(compareItem.row-row)==1)
        {
            return true;

        }

        if (compareItem.row == row && Math.Abs(compareItem.col - col) == 1)
        {
            return true;

        }

        return false;
    }


    public void MoveTo(int r,int c,float t)
    {
        SetItemIndex(r, c);
        Vector3 pos=startPos+ new Vector3(+c*width,-r*height,0);
        transform.DOMove(pos, t);
    }


    public void RegisterMouseButtonAction(Action<SingleItem> itemAction)
    {
        mouseButton += itemAction;
    }

    public void SetSelect(bool value)
    {
        block.SetActive(value);
    }




}
