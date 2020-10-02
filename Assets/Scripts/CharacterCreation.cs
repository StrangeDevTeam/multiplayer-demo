using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    public Image Hair_GO;
    public static int hairIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

        Object[] sprites = Resources.LoadAll<Sprite>(PlayerData.hairStylePaths[0]);
        Hair_GO.sprite = (Sprite)sprites[0];
    }

    public void nexthair()
    {
        hairchangeindex(1);
    }
    public void prevhair()
    {
        hairchangeindex(-1);
    }
    private void hairchangeindex(int changeInIndex)
    {
        hairIndex += changeInIndex;
        if(hairIndex >= PlayerData.hairStylePaths.Count)
        {
            hairIndex = 0;
        }
        else if(hairIndex < 0)
        {
            hairIndex = PlayerData.hairStylePaths.Count - 1;
        }
        Object[] sprites = Resources.LoadAll<Sprite>(PlayerData.hairStylePaths[hairIndex]);
        Hair_GO.sprite = (Sprite)sprites[0];

    }
}
