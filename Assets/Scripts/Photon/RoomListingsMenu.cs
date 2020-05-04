using Photon.Pun;
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
    public float nextListingOffset = 45.4622f; // (139.18f - 46.4f)*0.7f*0.7f;


    int index = 0;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            RoomListing listing = Instantiate(_roomListing, _content);
            if(listing != null)
            {
                listing.SetRoomInfo(info);
                listing.transform.position = new Vector2(listing.transform.position.x, listing.transform.position.y - nextListingOffset * (float)(index));
                index++;
            }
        }
        if(PhotonLobby.lobby.FirstRun)
        {
            //PhotonLobby.lobby.onHideGamesClicked(); // hide the loby menu on startup
        }
    }

}
