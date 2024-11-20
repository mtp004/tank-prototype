using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class FollowTank : MonoBehaviour
{
    private bool zoomed=false;
    private float defaultView;
    public float zoomView;
    public LayerMask Tank;
    public Transform mainCamera;
    public Camera main;
    public float hRotateSpeed;
    public float vRotateSpeed;
    public GameObject tank;
    private float horizontalRotation=90;
    private float verticalRotation=0;
    public Transform pointer;
    // Start is called before the first frame update
    void Start()
    {
        defaultView=main.fieldOfView;
        Cursor.lockState=CursorLockMode.Locked;
        verticalRotation=mainCamera.eulerAngles.x;
    }

    void Update()
    {
        horizontalRotation+=Input.GetAxis("Mouse X")*Time.deltaTime*hRotateSpeed;
        verticalRotation-=Input.GetAxis("Mouse Y")*Time.deltaTime*vRotateSpeed;

        Quaternion cameraPivot=Quaternion.Euler(0,horizontalRotation,0);
        Quaternion camera=Quaternion.Euler(verticalRotation,0,0);
        transform.rotation=Quaternion.Slerp(transform.rotation,cameraPivot,0.3f);
        mainCamera.rotation=Quaternion.Euler(verticalRotation,mainCamera.eulerAngles.y,0);

        //code to detect if the player pressed T to zoom in and out
        if(Input.GetKeyDown(KeyCode.T)){
            if(zoomed){
                main.fieldOfView=defaultView;
                zoomed=false;
            } else{
                main.fieldOfView=zoomView;
                zoomed=true;
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Ray ray = main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hit,100.0f,~Tank))
        {
            if(hit.collider.gameObject.tag=="Ground"){
                pointer.position=hit.point;
            }
        }
        transform.position=tank.transform.position+new Vector3(0,2.68f, 0);
    }
}
