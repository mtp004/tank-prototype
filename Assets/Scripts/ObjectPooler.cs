using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public short poolSize=20;
    //drag the object to pool in the game menu to this slot
    public GameObject ObjectToPool;
    public static ObjectPooler poolerInstance;
    public GameObject[] pools;
    public short poolIndex;
    // Start is called before the first frame update
    void Awake()
    {
        //if the static instance of the class does not exist, create one
        if(poolerInstance==null){
            poolerInstance=this;
        }
    }

    void Start(){
        poolIndex=0;
        createPool();
    }

    private void createPool(){
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
        while(true){
            if(poolIndex!=poolSize){
                GameObject gameObject=pools[poolIndex];
                poolIndex++;
                return gameObject;
            }
        }
    }

    public void ReleaseObject(GameObject gameObject){
        if(poolIndex!=0){
        PooledObject pooledObject=gameObject.GetComponent<PooledObject>();
        GameObject tmp=pools[poolIndex-1];
        PooledObject lastPooledObject=tmp.GetComponent<PooledObject>();
        short objectIndex=pooledObject.PoolIndex;
        short lastIndex=lastPooledObject.PoolIndex;

        //swap their indices
        pooledObject.PoolIndex=lastIndex;
        lastPooledObject.PoolIndex=objectIndex;

        //switch their place in the pool
        pools[poolIndex-1]=gameObject;
        pools[objectIndex]=tmp;
        gameObject.SetActive(false);
        poolIndex--;
        }
    }
    
    


}
