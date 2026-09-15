using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnityPool : MonoBehaviour
{
    private ObjectPool<SingleItem> pool;

    public SingleItem itemModel;

    int poolSize = 50;
    int poolMaxSize = 100;

    private void Awake()
    {
        pool = new ObjectPool<SingleItem>(OnCreate,OnGet,OnRelease,OnDes,true,poolSize,poolMaxSize);
    }

    public SingleItem Get()
    {
        return pool.Get();
    }

    public void Release(SingleItem obj)
    {
        pool.Release(obj);
    }


    private SingleItem OnCreate()
    {
        SingleItem go = Instantiate(itemModel);

        return go;
    }


    private void OnGet(SingleItem obj)
    {
        obj.gameObject.SetActive(true); 
    }

    private void OnRelease(SingleItem obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDes(SingleItem obj)
    {
        Destroy(obj.gameObject);    
    }

}
