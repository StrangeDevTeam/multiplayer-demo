using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> itemDatabase = new List<Item>()
    { 
        new Item
            (
            0,
            "example item",
            "this is an example item being added to the database",
            10,
            new Dictionary<string, int>
            {
                {"Useless", 10},
                {"Example", 9000 }
            }
            )
        ,
        new Item
            (
            1,
            "sword",
            "this is a stick",
            99,
            new Dictionary<string, int>
            {
                {"durability", 10},
                {"Example", 9000 }
            }
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
