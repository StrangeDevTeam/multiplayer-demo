using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar; // a link to the players avatar

    // when the player joins the game, create an avatar for them from a prefab
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnpoints.Length);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnpoints[spawnPicker].position,
                GameSetup.GS.spawnpoints[spawnPicker].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
