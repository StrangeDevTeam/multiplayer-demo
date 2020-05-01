using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{

    private PhotonView PV; // the photon view
    private bool oldMirrorAnim = false;

    public bool IsWalkingNormal = false;
    public bool mirrorAnim = false;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            anim.SetBool("IsWalkingNormal", IsWalkingNormal);
            if (mirrorAnim != oldMirrorAnim)
            {
                this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
                oldMirrorAnim = mirrorAnim;
            }
        }
    }
}
