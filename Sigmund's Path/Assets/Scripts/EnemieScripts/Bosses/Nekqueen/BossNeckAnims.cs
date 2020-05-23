using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckAnims : MonoBehaviour
{
    [SerializeField] BossNeck neckBrain;

    void StopDobleAttack()
    {
        neckBrain.DoDobleAttack = false;
    }

    void StopSalpicadura()
    {
        neckBrain.DoRangeAttack = false;
    }

    void ExecuteAttack()
    {
        neckBrain.CloseAttack();
    }

    void ThrowWater()
    {
        neckBrain.ThrowWater();
    }
}
