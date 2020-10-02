using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static InventoryMenu singleton; // singleton
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private InventoryMenuItem _itemListing;

    private  float nextListingOffset= 25;
    private  float heightOflisting = 25.7f;

    public List<InventoryMenuItem> prefabList;


    private void Start()
    {
        singleton = this;
    }
    public void UpdateInventoryUI()
    {
        foreach(InventoryMenuItem imi in prefabList)
        {
            DestroyImmediate(imi.gameObject);
        }
        prefabList = new List<InventoryMenuItem>();
        int index = 0;
        foreach( InventorySlot item in PlayerData.data.playerInv.inv)
        {
            InventoryMenuItem itemListing = Instantiate(_itemListing, _content);
            itemListing.SetItemListingInfo(item, PlayerData.data.playerInv.inv.IndexOf(item));
            itemListing.transform.Translate(new Vector2(0, (-nextListingOffset * index)));
            prefabList.Add(itemListing);


            index++;
        }
        RectTransform r = _content.GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(
            r.sizeDelta.x,
            heightOflisting * index);
    }

    public void OnScroll()
    {
        DestroyImmediate(InventoryMenuItem.EquipButtonGameObject);
    }


}
