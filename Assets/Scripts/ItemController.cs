using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    
    public int col;//列数
    public int row;//行数

    public float time;

    private SingleItem[,] itemList;
    private UnityPool itemPool;

    private SingleItem itemFitst;

    private List<SingleItem> deleteList;

    private void Start()
    {
        deleteList=new List<SingleItem>();

        itemList =new SingleItem[row, col];  
        itemPool=GetComponent<UnityPool>();

        for(int i=0;i<row;i++)
        {
            for(int j=0;j<col;j++)
            {
                SingleItem item = NewTiem(i,j);
                itemList[i,j] = item;
            }
        }

        bool canDelete = CheckDelete();
        if(canDelete)
        {
            DeleteItem();
        }

    }


    SingleItem NewTiem(int row,int col)
    {
        SingleItem item = itemPool.Get();
        item.transform.SetParent(transform);
        item.BirthItem(row,col);

        Action<SingleItem> itemAction = SelectedItem;
        item.RegisterMouseButtonAction(itemAction);
        return item;
    }

    private void SelectedItem(SingleItem item)
    {
        if (itemFitst == null)
        {      
            itemFitst = item;
            itemFitst.SetSelect(true);
        }
        else
        {
            if(item.CompareTypeWith(itemFitst))
            {
                //Exchange(itemFitst, item);
                StartCoroutine(ExchangeItem(itemFitst, item));
            }
            itemFitst.SetSelect(false);
            itemFitst = null;
        }
    }

    private void Exchange(SingleItem item1, SingleItem item2)
    {
        itemList[item1.row,item1.col]=item2;
        itemList[item2.row,item2.col]=item1;

        int temp1_col, temp1_row, temp2_col, temp2_row;

        temp1_col=item1.col;
        temp1_row=item1.row;
        temp2_col=item2.col;
        temp2_row=item2.row;


        item1.SetItemIndex(temp2_row,temp2_col);
        item2.SetItemIndex(temp1_row,temp1_col);

        item1.MoveTo(item1.row, item1.col, time);
        item2.MoveTo(item2.row, item2.col, time);

    }

    IEnumerator ExchangeItem(SingleItem item1, SingleItem item2)
    {
        Exchange(item1 , item2);

        yield return new WaitForSeconds(time);

        bool canDelete = CheckDelete();

        if (canDelete)
        {
            DeleteItem();
        }
        else
        {
            Exchange(item1, item2);
        }


    }

    private void DeleteItem()
    {
        int count=deleteList.Count;
        for (int i = 0; i < count; i++)
        {
            SingleItem item= deleteList[i];
            int tmpR=item.row;
            int tmpC=item.col;
            itemPool.Release(item);
            itemList[tmpR,tmpC] = null;
            for (int j = tmpR-1; j >=0; j--)
            {
                SingleItem itemTmp = itemList[j, tmpC];
                itemList[j+1,itemTmp.col] = itemTmp;
                itemList[j,tmpC] = null;
                itemTmp.SetItemIndex(j+1,itemTmp.col);
                itemTmp.MoveTo(itemTmp.row, itemTmp.col, time);

            }

            SingleItem newItem = itemPool.Get();
            newItem.transform.SetParent(transform);
            newItem.BirthItem(-1,item.col);
            newItem.MoveTo(0,newItem.col,time);
            itemList[0,newItem.col] = newItem;

        }

        deleteList = new List<SingleItem>();

        StartCoroutine(CheckDeleteNext());

    }

    IEnumerator CheckDeleteNext()
    {
        yield return new WaitForSeconds(time);

        bool canDelete = CheckDelete();
        if (canDelete)
        {
            DeleteItem();
        }
    }


    private bool CheckDelete()
    {
        bool canDelete = false;
        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                if (r < row - 2 && itemList[r, c] != null && itemList[r + 1, c] != null && itemList[r + 2, c] != null)
                {
                    if (itemList[r, c].type == itemList[r + 1, c].type && itemList[r, c].type == itemList[r + 2, c].type)
                    {
                        AddToDeleteList(itemList[r, c]);
                        AddToDeleteList(itemList[r + 1, c]);
                        AddToDeleteList(itemList[r + 2, c]);
                        canDelete = true;
                    }

                }

                if (c < col - 2 && itemList[r, c] != null && itemList[r, c + 1] != null && itemList[r, c + 2] != null)
                {
                    if (itemList[r, c].type == itemList[r, c + 1].type && itemList[r, c].type == itemList[r, c + 2].type)
                    {
                        AddToDeleteList(itemList[r, c]);
                        AddToDeleteList(itemList[r, c + 1]);
                        AddToDeleteList(itemList[r, c + 2]);
                        canDelete = true;
                    }

                }

            }

        }

        return canDelete;

    }

    private void AddToDeleteList(SingleItem newItem)
    {
        int index=deleteList.FindIndex(item=>item.row==newItem.row&&item.col==newItem.col);
        if(index==-1)
        {
            deleteList.Add(newItem);
        }


    }
}
