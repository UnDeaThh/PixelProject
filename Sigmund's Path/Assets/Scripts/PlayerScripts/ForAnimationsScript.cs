using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnimationsScript : MonoBehaviour
{
    private PlayerController plController;
    private PlayerAttack plAttack;
    private PlayerParry plParry;

    private void Awake()
    {
        plController = GetComponentInParent<PlayerController>();
        plAttack = GetComponentInParent<PlayerAttack>();
        plParry = GetComponentInParent<PlayerParry>();
    }


    public void StopAttacking()
    {
        plAttack.isAttacking = false;
    }

    public void StopWallJump()
    {
        plController.isWallJump = false;
    }
}
