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
        _text.text = roomInfo.PlayerCount+"/"+roomInfo.MaxPlayers + "   " + roomInfo.Name; // set the text for the icon in the lobby
        RoomName = roomInfo.Name; // save the room name
        if(roomInfo.PlayerCount< roomInfo.MaxPlayers) // loop through the sessions and change the style of the button to match
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

    /// <summary>
    /// run when the join button for a session is clicked
    /// </summary>
    public void JoinButtonClicked()
    {
        Debug.Log("join button clicked");
        PhotonLobby.lobby.joinRoom(RoomName);
    }
}
