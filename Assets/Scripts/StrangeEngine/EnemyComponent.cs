// Copyright(c) 2020 arcturus125 & StrangeDevTeam
// Free to use and modify as you please, Not to be published, distributed, licenced or sold without permission from StrangeDevTeam
// Requests for the above to be made here: https://www.reddit.com/r/StrangeDev/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public Enemy enemyReference;
    public int health;

    public void Use()
    {
    }
    public void Damage(int damage)
    {
        health -= damage;
        bool isDed = enemyReference.CheckforKill(this);
        if (isDed)
        {
            Kill();
        }
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
        Destroy(this.gameObject);
        PlayerInteraction2D.previousColliders.Remove(this.gameObject.GetComponent<Collider2D>());
    }

    
    private void Update()
    {
        Animations();
    }
    private void Start() // Move to SPAWN() when spaning engine done
    {
        SR = GetComponent<SpriteRenderer>();
        health = enemyReference.health;
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
