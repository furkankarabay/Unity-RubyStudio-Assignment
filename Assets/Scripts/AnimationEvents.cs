using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject parentGO;

    public void AttackToTarget()
    {
        parentGO.SendMessage("AttackToTarget");
    }
}
