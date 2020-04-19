using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController myCC;

    public float MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                myCC.Move(new Vector2(0, MovementSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.S))
            {
                myCC.Move(new Vector2(0, -MovementSpeed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.D))
            {
                myCC.Move(new Vector2(MovementSpeed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                myCC.Move(new Vector2(-MovementSpeed * Time.deltaTime, 0));
            }
        }
    }
}
