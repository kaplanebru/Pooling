using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Humanoid : MonoBehaviour
{
    private Collider col;
    [ReadOnly]public Animator animator;
    private string[] winAnimations = {"Win1", "Win2", "Win3"};

    private void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        col = GetComponent<Collider>();
    }

    public void EnableCollider(bool enable)
    {
        col.enabled = enable;
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void PlayWin()
    {
        animator.CrossFade(winAnimations[Random.Range(0, winAnimations.Length)], 0.1f);
    }


}
