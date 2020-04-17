using UnityEngine;

public class AnimEventBerm : MonoBehaviour
{
    public BermonchAI berm;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void BuildBerm()
    {
        berm.bermBuild = true;
        anim.SetBool("bermBuild", true);
    }

    void DestroyBerm()
    {
        berm.callDead = true;
        berm.InstantiateSoul(berm.enemyType);
    }

    void BermDisolving()
    {
        berm.IsDisolving = true;
    }

    void CloseAttack()
    {
        berm.CloseAttack1 = true;
    }

    void FinishCloseAttack()
    {
        berm.CloseAttackAnimation = false;
    }

    void RangeAttack()
    {
        berm.RangeAttack1 = true;
    }
}
