// Copyright(c) 2020 arcturus125 & StrangeDevTeam
// Free to use and modify as you please, Not to be published, distributed, licenced or sold without permission from StrangeDevTeam
// Requests for the above to be made here: https://www.reddit.com/r/StrangeDev/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int ID;
    public string itemName;
    public string info;
    public int worth;
    public bool isStackable = false;
    public int stackLimit = 999;
    //public Sprite icon;
    public Dictionary<string, int> itemStats = new Dictionary<string, int>(); // "stats" that come in pairs, a name (string) and a number. for example ("weight", 22)

    public Item(int pID, string pName, string pInfo, int pWorth, Dictionary<string,int> pStats)
    {
        ID = pID;
        itemName = pName;
        info = pInfo;
        worth = pWorth;
        itemStats = pStats;
    }
    
    public Item(int pID, string pName, string pInfo, int pWorth, Dictionary<string, int> pStats, bool pIsStackable, int pStackLimit = 999)
    {
        ID = pID;
        itemName = pName;
        info = pInfo;
        worth = pWorth;
        itemStats = pStats;
        isStackable = pIsStackable;
        stackLimit = pStackLimit;
    }


    public static Weapon convertToWeapon(Item i)
    {
        try
        {
            Weapon temp = (Weapon)(i);
            return temp;
        }
        catch (Exception)
        {
            return null;
        }
    }

}
public class Weapon : Item
{
    public string spritePath;
    public int damage;
    public float critChance;
    public float critMultiplier;
    public enum WeaponType { Polearm, Sword }
    public WeaponType type;
    public float range;


    public Weapon(int pID, string pName, string pInfo, int pWorth, Dictionary<string, int> pStats, string pSpritePath, WeaponType pType, int pDamage, float pRange): base(pID, pName, pInfo,pWorth,pStats)
    {
        spritePath = pSpritePath;
        type = pType;
        damage = pDamage;
        range = pRange;
    }
}
