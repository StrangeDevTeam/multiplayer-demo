using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> itemDatabase = new List<Item>()
    {
        new Item
            (
            0,// make sure this is unique
            "example item",
            "this is an example item being added to the database",
            10,
            new Dictionary<string, int>
            {
                {"Useless", 10},
                {"Example", 9000 }
            },
            true,
            8
            )
        ,

        new Weapon
            (
            1,// make sure this is unique
            "Maha",
            "This is the legendary poleaxe used by the mighty warrior Aran.",
            99999,
            new Dictionary<string, int>
            {
                {"Example", 9000 }
            },
            "Sprites/Weapons/MAHA",
            Weapon.WeaponType.Polearm,
            10,
            1
            )
        ,

        new Weapon
            (
            2,// make sure this is unique
            "Crescent Rose",
            "This is the legendary scythe is used by the huntress Ruby Rose.",
            99999,
            new Dictionary<string, int>
            {
                {"Example", 9000 }
            },
            "Sprites/Weapons/crescent rose",
            Weapon.WeaponType.Polearm,
            10,
            2
            )
        ,

        new Weapon
            (
            3,// make sure this is unique
            "Stick",
            "i don't know what you expected to see here. it's a stick. nothing interesting about it",
            5,
            new Dictionary<string, int>
            {
                {"Example", 9000 }
            },
            "Sprites/Weapons/stick",
            Weapon.WeaponType.Sword,
            1,
            0.5f
            )
    };

    public static Item SearchDatabaseByID(int ID)
    {
        for(int i = 0; i < itemDatabase.Count; i++)
        {
            if(ID == itemDatabase[i].ID)
            {
                return itemDatabase[i];
            }
        }
        return null;
    }

    public static Item SearchDatabaseByName(string Name)
    {
        for(int i = 0; i< itemDatabase.Count; i++)
        {
            if(Name == itemDatabase[i].itemName)
            {
                return itemDatabase[i];
            }
        }
        return null;
    }
}
