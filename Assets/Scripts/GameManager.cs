// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEditor.EditorTools;
// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     private int poolSize;
//     //drag the object to pool in the game menu to this slot
//     public GameObject ObjectToPool;
//     public static GameManager manager;
//     public GameObject[] pools;
//     public short poolIndex=0;
//     public GameObject activePlayer;
//     public Camera activeCamera;
//     void Awake()
//     {
//         //if the static instance of the class does not exist, create one
//         if(manager==null){
//             manager=this;
//         }

//         //tag is hardcoded but i will change that later
//         poolSize=20*GetPooledObjectUserCount("Tank");
//         InitializePool();
//     }

//     private int GetPooledObjectUserCount(string tag){
//         return GameObject.FindGameObjectsWithTag(tag).Length;
//     }
    
//     private void InitializePool(){
//         pools=new GameObject[poolSize];
//         GameObject tmp;
//         for(short i=0; i<poolSize; i++){
//             tmp=Instantiate(ObjectToPool);
//             tmp.SetActive(false);
//             tmp.AddComponent<PooledObject>().PoolIndex = i;
//             pools[i]=tmp;
//         }
//     }

//     public GameObject GetObjectFromPool(){
//         GameObject gameObject=pools[poolIndex];
//         poolIndex++;
//         return gameObject;
//     }

//     public void ReleaseObject(GameObject gameObject){
//         GameObject latestActive = pools[poolIndex - 1];

//         // Swapping logic here
//         var releaseComponent = gameObject.GetComponent<PooledObject>();
//         var activeComponent = latestActive.GetComponent<PooledObject>();
//         int releaseIndex = releaseComponent.PoolIndex;
//         int activeIndex = activeComponent.PoolIndex;

//         // Swap indices and update pool
//         releaseComponent.PoolIndex = activeIndex;
//         activeComponent.PoolIndex = releaseIndex;
//         pools[activeIndex] = gameObject;
//         pools[releaseIndex] = latestActive;
//         poolIndex--;
//     }
// }

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
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public static GameManager manager;
    public short poolIndex=0;
    public GameObject activePlayer;
    public Camera activeCamera;

    void Awake(){
        manager=this;
    }
    void Start()
    {
        poolSize=10*GetPooledObjectUserCount("Tank");
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }   
    }

    private int GetPooledObjectUserCount(string tag){
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }

    public GameObject GetObjectFromPool(){
        for(int i = 0; i < poolSize; i++)
    {
        if(!pooledObjects[i].activeInHierarchy)
        {
            return pooledObjects[i];
        }
    }
    return null;
    }
}
