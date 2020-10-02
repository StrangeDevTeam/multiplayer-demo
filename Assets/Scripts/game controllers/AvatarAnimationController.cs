using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using TMPro;
using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{

    private PhotonView PV; // the photon view
    private bool oldMirrorAnim = false;

    public bool IsWalkingNormal = false;
    public bool isClimbing = false;
    public float isClimbingSpeed = 1;
    public bool mirrorAnim = false;
    public bool TwoHandedAttack = false;
    public bool OneHandedAttack = false;
    public bool showJumpAnimation = false;
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
            anim.SetBool("IsClimbing", isClimbing);
            anim.SetBool("TwoHandedAttack", TwoHandedAttack);
            anim.SetBool("OneHandedAttack", OneHandedAttack);
            anim.SetFloat("IsClimbingSpeed", isClimbingSpeed);
            anim.SetBool("isNotOnGround", showJumpAnimation);
            if (mirrorAnim != oldMirrorAnim)
            {
                this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
                this.GetComponentInChildren<TextMesh>().gameObject.transform.localScale = new Vector2(
                    -this.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.x,
                    this.GetComponentInChildren<TextMesh>().gameObject.transform.localScale.y);
                oldMirrorAnim = mirrorAnim;
            }
        }
    }
}
