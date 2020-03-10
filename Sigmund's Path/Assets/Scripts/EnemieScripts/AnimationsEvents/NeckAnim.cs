using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAnim : MonoBehaviour
{
    [SerializeField] NeckAI neckBrain;
    void FirstAttack()
    {
        neckBrain.firstAttack = true;
    }
}
