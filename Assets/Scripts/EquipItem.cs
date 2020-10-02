using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour
{
    public void OnEquipItemButtonClicked()
    {
        int i = InventoryMenuItem.SelectedItemID;
        if (i == PlayerData.data.equippedWeaponIndex)
        {
        }
        else
        {

            PlayerData.data.equippedWeaponIndex = i;
            InventoryMenu.singleton.UpdateInventoryUI();

            this.GetComponentInChildren<Text>().text = "Equipped";
            this.GetComponentInChildren<Text>().color = new Color(255, 255, 255,255);
            this.GetComponentInChildren<Button>().interactable = false;
            this.GetComponentInChildren<Image>().color = new Color(255, 0, 99,255);
        }
        Avatar.singleton.UpdateEquipedWeaponSprite();
    }
}
