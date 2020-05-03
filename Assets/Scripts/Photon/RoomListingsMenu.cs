﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList) // loop through all the sessions and create a button for them
        {
            RoomListing listing = Instantiate(_roomListing, _content);
            if(listing != null)
            {
                listing.SetRoomInfo(info);
            }
        }
        if(PhotonLobby.lobby.FirstRun) // run during startup, when the sessions are added into
        {
            PhotonLobby.lobby.roomListingsPanel.transform.position = PhotonLobby.lobby.GamesPanelPosition_Hidden;
        }
    }

}
