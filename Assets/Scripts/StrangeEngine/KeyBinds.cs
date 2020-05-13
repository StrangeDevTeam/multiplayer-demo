using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public static KeyCode interactKey = KeyCode.F; //Key used to interact with objects/ NPCs.
    public static KeyCode movementLeftKey = KeyCode.A;
    public static KeyCode movementRightKey = KeyCode.D;
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode crouchKey = KeyCode.S;
    public static KeyCode upKey = KeyCode.W; //Used for climbable objects to move upwards.

    public static KeyCode attackKey = KeyCode.E; // the default attack in the game

    public static KeyCode inventoryKey = KeyCode.I;
}
