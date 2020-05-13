using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private PhotonView PV;
    public TextMesh text; // the playerName text that appears below the players avatar
    public string name;

    public GameObject Two_Handed_weapon;
    public GameObject One_Handed_weapon;





    AvatarAnimationController aac;
    // Start is called before the first frame update
    void Start()
    {
        aac = GetComponent<AvatarAnimationController>();
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            text.text = PlayerData.data.Name;
            name = PlayerData.data.Name;
            //set my name - and send it to other players
            PV.RPC("RPC_AddPlayerName", RpcTarget.AllBuffered, name);
        }
        else
        {
            text.text = name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = name; // TODO: find a way to not run this every frame

        // if Two Hand Attack animation is playing, reset the bool
        if (aac.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_Attack"))
            aac.TwoHandedAttack = false;
        // if One hand attack animatin is playing, reset the bool
        else if (aac.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_OneHand_Attack"))
            aac.OneHandedAttack = false;
        // if no attack animation is playing, detect inputs for attacks
        else
        {
            if (Input.GetKey(KeyBinds.attackKey))
            {
                // double check a weapon is equipped
                if (Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.equippedWeaponIndex].item) != null)
                {
                    Weapon w = Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.equippedWeaponIndex].item);
                    if (w.type == Weapon.WeaponType.Polearm)
                        aac.TwoHandedAttack = true;
                    else if (w.type == Weapon.WeaponType.Sword)
                        aac.OneHandedAttack = true;
                }
            }
        }
        UpdateEquipedWeaponSprite(); // TODO: find a way to not run this every frame
    }

    // updates the sprite of the weapon in the avatar's hand
    public void UpdateEquipedWeaponSprite()
    {
        if (PlayerData.equippedWeaponIndex != -1)
        {
            // if item is actually an equippable weapon, then show it in the players hand
            if (Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.equippedWeaponIndex].item) != null)
            {
                Weapon w = Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.equippedWeaponIndex].item);
                if (w.type == Weapon.WeaponType.Polearm)
                {
                    Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(w.spritePath) as Sprite;
                    One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
                }
                if (w.type == Weapon.WeaponType.Sword)
                {
                    One_Handed_weapon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(w.spritePath) as Sprite;
                    Two_Handed_weapon.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
    }

    // an animation trigger run during the two handed attack animation
    public void TwoHandedAttack()
    {
        Damage(1, true, Item.convertToWeapon(PlayerData.data.playerInv.inv[PlayerData.equippedWeaponIndex].item).damage); // TODO: get weapon damage  and range
    }
    public void OneHandedAttack()
    {
        Damage(0.5f, true, Item.convertToWeapon(PlayerData.data.playerInv.inv[ PlayerData.equippedWeaponIndex].item).damage); // TODO: get weapon damage  and range
    }

    /// <summary>
    /// used to damage enemies within the players location
    /// </summary>
    /// <param name="range"> the range of the damage</param>
    /// <param name="isDirectional"> if true, will onyl damage enemies infront of the player (not behind)</param>
    /// <param name="damage"> the amount of damage to do to the enemies</param>
    public void Damage(float range, bool isDirectional, int damage) // TODO: add this to the AIP documentation
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
    
}
