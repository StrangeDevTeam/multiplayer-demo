using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraScript;


    public GameObject cameraTarget;
    public float defaultCameraZ;


    // Start is called before the first frame update
    private void Awake()
    {
        cameraScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTarget)
        {
            this.transform.position = new Vector3(
                cameraTarget.transform.position.x,
                cameraTarget.transform.position.y,
                -defaultCameraZ);
        }
    }
}
