using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public static Avatar singleton;
    private PhotonView PV;
    public TextMesh text; // the playerName text that appears below the players avatar
    public string name;

    public GameObject Two_Handed_weapon;
    public GameObject One_Handed_weapon;
    public GameObject hair;
    public GameObject hairBehind;

    private float animationTime = 0.5f;
    private float timeSinceAnimationStart = 0;





    AvatarAnimationController aac;
    // Start is called before the first frame update
    void Start()
    {
        aac = GetComponent<AvatarAnimationController>();
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            singleton = this;
            text.text = PlayerData.data.Name;
            name = PlayerData.data.Name;

            // load the players hair in
            Object[] sprites = Resources.LoadAll<Sprite>(PlayerData.hairStylePaths[PlayerData.data.hairID]);
            hair.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[0];
            hairBehind.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[1];
            // load the players weapon in
            UpdateEquipedWeaponSprite();


            //set my name - and send it to other players
            PV.RPC("RPC_AddPlayerName", RpcTarget.AllBuffered, name);
            //set my hair - and send it to other players
            PV.RPC("RPC_AddPlayerHair", RpcTarget.AllBuffered, PlayerData.data.hairID);


        }
        else
        {
            text.text = name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            timeSinceAnimationStart += Time.deltaTime;
            if(timeSinceAnimationStart > animationTime)
            {
                timeSinceAnimationStart = 0;
                // if Two Hand Attack animation is playing, reset the bool
                if (aac.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_Attack"))
                    aac.TwoHandedAttack = false;
                // if One hand attack animatin is playing, reset the bool
                else if (aac.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_OneHand_Attack"))
                    aac.OneHandedAttack = false;
            }

            // if no attack animation is playing, detect inputs for attacks
            else
            {
                if (Input.GetKey(KeyBinds.attackKey)&&
                    GetComponent<PlayerMovement>().onDroppablePlatform &&
                    GetComponent<PlayerMovement>().isOnGround &&
                    !(GetComponent<PlayerMovement>().currentPlayerState == PlayerMovement.playerState.climbing))
                {
                    // double check a weapon is equipped
                    if (PlayerData.data.equippedWeaponIndex >= 0)
                    {
                        if (Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item) != null)
                        {
                            Weapon w = Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item);
                            if (w.type == Weapon.WeaponType.Polearm)
                                aac.TwoHandedAttack = true;
                            else if (w.type == Weapon.WeaponType.Sword)
                                aac.OneHandedAttack = true;
                        }
                    }
                }
            }
        }
    }

    // updates the sprite of the weapon in the avatar's hand
    public void UpdateEquipedWeaponSprite()
    {
        if (PlayerData.data.equippedWeaponIndex != -1)
        {
            // if item is actually an equippable weapon, then show it in the players hand
            if (Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item) != null)
            {
                Weapon w = Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item);
                if (w.type == Weapon.WeaponType.Polearm)
                {
                    // load the sprite from file, disable the others
                    Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(w.spritePath) as Sprite;
                    One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
                    PV.RPC("RPC_updateWeaponSprite", RpcTarget.AllBuffered, Weapon.WeaponType.Polearm, w.spritePath);
                }
                if (w.type == Weapon.WeaponType.Sword)
                {
                    // load the sprite from file, disable the others
                    One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(w.spritePath) as Sprite;
                    Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
                    PV.RPC("RPC_updateWeaponSprite", RpcTarget.AllBuffered, Weapon.WeaponType.Sword, w.spritePath);
                }
            }
        }
    }

    // an animation trigger run during the two handed attack animation
    public void TwoHandedAttack()
    {
        DamageArea(Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item).range, true, Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item).damage);
    }
    public void OneHandedAttack()
    {
        DamageArea(Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.data.equippedWeaponIndex].item).range, true, Item.convertToWeapon(PlayerData.data.playerInv.inv[ PlayerData.data.equippedWeaponIndex].item).damage);
    }

    /// <summary>
    /// used to damage enemies within the players location
    /// </summary>
    /// <param name="range"> the range of the damage</param>
    /// <param name="isDirectional"> if true, will onyl damage enemies infront of the player (not behind)</param>
    /// <param name="damage"> the amount of damage to do to the enemies</param>
    public void DamageArea(float range, bool isDirectional, int damage) 
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(this.transform.position, range);  // get every collider within interactionDistance
        for(int i = 0; i < nearbyColliders.Length;i++)
        {
            if (isDirectional)
            {
                if(aac.mirrorAnim) // search right of player
                {
                    if(nearbyColliders[i].gameObject.transform.position.x > this.transform.position.x)
                    {
                        nearbyColliders[i].gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
                    }
                }
                else // search left of player
                {
                    if (nearbyColliders[i].gameObject.transform.position.x < this.transform.position.x)
                    {
                        nearbyColliders[i].gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            else
            {
                nearbyColliders[i].gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
        
    }

    
    [PunRPC]
    void RPC_AddPlayerName(string playername)
    {
        name = playername;
    }
    [PunRPC]
    void RPC_AddPlayerHair(int hairID)
    {
        Object[] sprites = Resources.LoadAll<Sprite>(PlayerData.hairStylePaths[hairID]);
        hair.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[0];
        hairBehind.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[1];
    }
    [PunRPC]
    void RPC_updateWeaponSprite(Weapon.WeaponType type, string spritePath)
    {
        if (type == Weapon.WeaponType.Polearm)
        {
            // load the sprite from file, disable the others
            Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath) as Sprite;
            One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (type == Weapon.WeaponType.Sword)
        {
            // load the sprite from file, disable the others
            One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath) as Sprite;
            Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
        }
        
    }
}
