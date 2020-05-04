using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Button joinButton;
    private string RoomName;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _text.text = roomInfo.PlayerCount+"/"+roomInfo.MaxPlayers + "   " + roomInfo.Name;
        RoomName = roomInfo.Name;
        if(roomInfo.PlayerCount< roomInfo.MaxPlayers)
        {
            if(roomInfo.IsOpen)
            { 
                joinButton.image.color = new Color(0, 204, 205);
                joinButton.GetComponentInChildren<Text>().text = "Join";
                joinButton.interactable = true;
            }
            else
            {
                joinButton.image.color = new Color(199, 0, 0);
                joinButton.GetComponentInChildren<Text>().text = "Locked";
                joinButton.interactable = false;
            }
        }
        else
        {
            joinButton.image.color = new Color(199, 0, 0);
            joinButton.GetComponentInChildren<Text>().text = "Full";
            joinButton.interactable = false;
            
        }
    }

    public void JoinButtonClicked()
    {
        Debug.Log("join button clicked");
        PhotonLobby.lobby.joinRoom(RoomName);
    }
}
