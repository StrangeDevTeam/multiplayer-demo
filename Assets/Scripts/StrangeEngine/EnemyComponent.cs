// Copyright(c) 2020 arcturus125 & StrangeDevTeam
// Free to use and modify as you please, Not to be published, distributed, licenced or sold without permission from StrangeDevTeam
// Requests for the above to be made here: https://www.reddit.com/r/StrangeDev/

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    
    public EnemySpawner parentSpawner;
    public Enemy enemyReference;
    public int health;
    public PhotonView PV;

    GameObject player; // TODO :  make work with multiple clients: target closest player

    public void Use()
    {

    }
    public void Damage(int damage)
    {
        // display damage in screen
        DamageIndicatorManager.singleton.SpawnDamageSplashNumber(this.transform, damage);

        //tell the master client to kill the enemy
        PV.RPC("RPC_DamageEnemy", RpcTarget.MasterClient, damage);

    }

    public void OnNearby()
    {
        UIController.ShowInteractionTooltip();
    }

    public void NoLongerNearby()
    {
        UIController.HideInteractionTooltip();
    }

    void Kill()
    {
        //tell the master client to kill the enemy
        PV.RPC("RPC_KillEnemy", RpcTarget.MasterClient);
    }

    
    private void Update()
    {
        Animations();

        if (PhotonNetwork.IsMasterClient)
        {
            Movement();
        }
    }
    private void Start() // Move to SPAWN() when spaning engine done
    {
        PV = this.GetComponent<PhotonView>();
        SR = GetComponent<SpriteRenderer>();
        health = enemyReference.health;
        player = Avatar.singleton.gameObject;
    }




    //  *********************************************** Sync to master ***************************************


    [PunRPC]
    void RPC_KillEnemy()
    {

        // remove the enemy from the spawner to make room for another to spawn
        try
        {
            if (parentSpawner.enemies_list.Contains(this))
                parentSpawner.enemies_list.Remove(this);
        }catch
        {
            // destroy the enemy gameobject
            Destroy(this.gameObject);
            PlayerInteraction2D.previousColliders.Remove(this.gameObject.GetComponent<Collider2D>());
        }
        // destroy the enemy gameobject
        Destroy(this.gameObject);
        PlayerInteraction2D.previousColliders.Remove(this.gameObject.GetComponent<Collider2D>());

        PV.RPC("RPC_DestroyEnemy", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    void RPC_DamageEnemy(int damage)
    {
        health -= damage;
        bool isDed = enemyReference.CheckforKill(this);
        if (isDed)
        {
            Kill();
        }
    }

    [PunRPC]
    void RPC_DestroyEnemy()
    {
        // destroy the enemy gameobject
        Destroy(this.gameObject);
        PlayerInteraction2D.previousColliders.Remove(this.gameObject.GetComponent<Collider2D>());
    }

    //  *********************************************** AI ***************************************


    public enum enemyAgressionState { passive, neutral, agressive }; //An enum holding the current state of the player.

    private void Movement()
    {

    }

    //  *********************************************** animations ***************************************

    SpriteRenderer SR;
    public enum enemyState { idle, moving, jumping, hit, die }; //An enum holding the current state of the player.
    public enemyState currentEnemyState = enemyState.idle; //The default state is set to idle, though this will change as soon as the player is loaded in.
    int spriteIndex = 0;

    public float animation_frame_rate = 0.25f;
    float time = 0;
    void Animations()
    {
        time += Time.deltaTime;
        if(time>animation_frame_rate)
        {
            time = 0;
            nextFrame();
        }
    }
    void nextFrame()
    {
        if(currentEnemyState == enemyState.idle)
        {
            SR.sprite = enemyReference.IdleAnimation[spriteIndex];
            spriteIndex++;
            if(spriteIndex >= enemyReference.IdleAnimation.Length)
            {
                spriteIndex = 0;
            }
        }
        else if (currentEnemyState == enemyState.moving)
        {
            SR.sprite = enemyReference.MovingAnimation[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= enemyReference.MovingAnimation.Length)
            {
                spriteIndex = 0;
            }
        }
        else if (currentEnemyState == enemyState.jumping)
        {
            SR.sprite = enemyReference.JumpAnimation[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= enemyReference.JumpAnimation.Length)
            {
                spriteIndex = 0;
            }
        }
        else if (currentEnemyState == enemyState.hit)
        {
            SR.sprite = enemyReference.HitAnimation[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= enemyReference.HitAnimation.Length)
            {
                spriteIndex = 0;
            }
        }
        else if (currentEnemyState == enemyState.die)
        {
            SR.sprite = enemyReference.DeadAnimation[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= enemyReference.DeadAnimation.Length)
            {
                spriteIndex = 0;
            }
        }
    }
}
