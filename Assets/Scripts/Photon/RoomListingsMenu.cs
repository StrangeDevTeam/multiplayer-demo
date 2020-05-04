using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;
    private float nextListingOffset = 45f;
    private float heightOfListing = 92.78403f;


    private List<RoomInfo> list_of_rooms = new List<RoomInfo>(); // the list of photon rooms
    private List<RoomListing> list_of_prefabs = new List<RoomListing>(); // the  lit of prefabs on the roomListings menu
    int index = 0;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //check if the updated rooms are already in my list
       
        for(int r = 0; r< roomList.Count;r++)
        {
            bool isAlreadyInList = false;
            for (int i = 0; i < list_of_rooms.Count; i++)
            {
                //if they are already in the list:
                if (list_of_rooms[i].Name == roomList[r].Name)
                {
                    // if empty, remove it
                    if ((roomList[r].MaxPlayers == 0)&&(roomList[r].PlayerCount == 0))
                    {
                        list_of_rooms.RemoveAt(i);
                        Debug.Log("removed " + roomList[r].Name);
                    }
                    else// otherwise, update the details
                    {
                        list_of_rooms[i] = roomList[r];
                    }
                    isAlreadyInList = true;
                }
            }
            // if updated rooms are not already in the list, then add it
            if (!isAlreadyInList)
            {
                list_of_rooms.Add(roomList[r]);
                Debug.Log("added " + roomList[r].Name);
            }
        }


        UpdateGUI();
    }

    public void UpdateGUI()
    {
        // remove all the prefabs
        for (int i = 0; i < list_of_prefabs.Count; i++)
        {
            Debug.Log("destroyed " + list_of_prefabs[i].GetComponentInChildren<Text>().text);
            DestroyImmediate(list_of_prefabs[i].gameObject);
        }
        // clear the array of prefabs
        list_of_prefabs = new List<RoomListing>();
        index = 0; // reset the index

        // loop through all the active photon rooms/sessions
        foreach (RoomInfo info in list_of_rooms)
        {
            // create a prefab for them
            RoomListing listing = Instantiate(_roomListing, _content);
            list_of_prefabs.Add(listing);
            if (listing != null)
            {
                listing.SetRoomInfo(info);
                Debug.Log("added " + listing.GetComponentInChildren<Text>().text);
                listing.transform.Translate(new Vector2(0, (-nextListingOffset * index)));
                index++;
            }
        }
        // expand the content window (for the scroll)
        RectTransform r = _content.GetComponent<RectTransform>();
        _content.GetComponent<RectTransform>().sizeDelta = new Vector2(
            r.sizeDelta.x,
            heightOfListing * index);
    }

}
