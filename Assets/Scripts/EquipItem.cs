using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    public void OnEquipItemButtonClicked()
    {
        int i = InventoryMenuItem.SelectedItemID;
        Debug.Log(i);
        PlayerData.equippedWeaponIndex = i;
        InventoryMenu.singleton.UpdateInventoryUI();
    }
}
