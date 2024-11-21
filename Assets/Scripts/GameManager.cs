using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int poolSize;
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
        poolSize=3*GetPooledObjectUserCount("Tank");
        poolIndex=0;
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
        if(gameObject.activeInHierarchy==true) Debug.Log("Get is fucked");
        poolIndex++;
        return gameObject;
    }

    public void ReleaseObject(GameObject gameObject){
        if(gameObject.activeInHierarchy==false) Debug.Log("Release is fucked");
        gameObject.SetActive(false);
        GameObject latestActive = pools[poolIndex - 1];

        // Swapping logic here
        var releaseComponent = gameObject.GetComponent<PooledObject>();
        var activeComponent = latestActive.GetComponent<PooledObject>();
        int releaseIndex = releaseComponent.PoolIndex;
        int activeIndex = activeComponent.PoolIndex;

        // Swap indices and update pool
        releaseComponent.PoolIndex = activeIndex;
        activeComponent.PoolIndex = releaseIndex;
        pools[activeIndex] = gameObject;
        pools[releaseIndex] = latestActive;
        poolIndex--;
    }
}
