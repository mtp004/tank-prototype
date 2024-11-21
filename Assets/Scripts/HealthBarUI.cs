using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Camera activeCamera;
    private Transform HBtransform;

    void Awake()
    {
        HBtransform=gameObject.GetComponent<Transform>();
        activeCamera=GameManager.manager.activeCamera;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarToCamera();
    }

    void HealthBarToCamera(){
        HBtransform.LookAt(activeCamera.transform);
    }
}
