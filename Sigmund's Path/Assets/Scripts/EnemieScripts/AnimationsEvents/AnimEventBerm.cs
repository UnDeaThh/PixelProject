using UnityEngine;

public class AnimEventBerm : MonoBehaviour
{
    public BermonchAI berm;

    void SetAnimationEvent()
    {
        berm.bermBuild = true;
    }

    void DestroyBerm()
    {
        berm.callDead = true;
        berm.InstantiateSoul(berm.enemyType);
    }
}
