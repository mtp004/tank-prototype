using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private short poolSize;
    //drag the object to pool in the game menu to this slot
    public GameObject ObjectToPool;
    public static GameManager manager;
    public GameObject[] pools;
    public short poolIndex;
    public GameObject activePlayer;
    public Camera activeCamera;
    void Awake()
    {
        //if the static instance of the class does not exist, create one
        if(manager==null){
            manager=this;
        }

        //tag is hardcoded but i will change that later
        poolSize=(short)(3*GetPooledObjectUserCount("Tank"));
        poolIndex=1;
        InitializePool();
    }

    private int GetPooledObjectUserCount(string tag){
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
    
    private void InitializePool(){
        pools=new GameObject[poolSize];
        GameObject tmp;
        for(short i=0; i<poolSize; i++){
            tmp=Instantiate(ObjectToPool);
            tmp.SetActive(false);
            tmp.AddComponent<PooledObject>().PoolIndex = i;
            pools[i]=tmp;
        }
    }

    public GameObject GetObjectFromPool(){
        GameObject gameObject=pools[poolIndex];
        poolIndex++;
        return gameObject;
    }

    public void ReleaseObject(GameObject gameObject){
        GameObject latestActive=pools[poolIndex-1];
        poolIndex--;

        //getting release and lastest active object index in pool by getting their component
        PooledObject releaseComponent=gameObject.GetComponent<PooledObject>();
        PooledObject activeComponent=latestActive.GetComponent<PooledObject>();
        short releaseIndex=releaseComponent.PoolIndex;
        short activeIndex=activeComponent.PoolIndex;

        //swap their index component value before swapping them in the array
        releaseComponent.PoolIndex=activeIndex;
        activeComponent.PoolIndex=releaseIndex;

        //switch their place in the pool
        pools[activeIndex]=gameObject;
        pools[releaseIndex]=latestActive;

        gameObject.SetActive(false);
    }
    
    


}