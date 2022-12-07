using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : Humanoid
{
    public static Action OnPassingAlly;
    public static Action<InputState> OnStateChange;
    public static Action OnFail;
    public static Action<bool, bool> OnEnableRagdoll;
    
    public InputState state = InputState.Release;
    public float ease = 0.3f;
    
    public Transform chunkParent;
    private bool moving = false;

    public enum InputState
    {
        Release, Hold
    }
    
    void Start()
    {
        transform.position += Vector3.right;
        StartCoroutine(nameof(InputRoutine));
        StartCoroutine(nameof(StateRoutine));
    }
    
    IEnumerator StateRoutine()
    {
        while (true)
        {
            switch (state)
            {
                case InputState.Hold:
                    StopCoroutine("MoveRoutine");
                    StartCoroutine("MoveRoutine");
                    Left();
                    yield return new WaitUntil(() => state == InputState.Release);
                    break;
                
                case InputState.Release:
                    Right();
                    yield return new WaitUntil(() => state == InputState.Hold);
                    break;
            }
            OnStateChange?.Invoke(state);

            yield return null;
        }
    }

    private Vector3 destination;
    void Move()
    {
        Vector3 oldTransform = transform.position;
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, destination.x, ease), 0, transform.position.z + destination.z*ease);
        var dir = (transform.position - oldTransform).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(dir), 0.5f);
    }

    
    IEnumerator MoveRoutine()
    {
        moving = false;
        while (true)
        {
            moving = true;
            if (transform.position.x >= 0.99f && state == InputState.Release)
            {
                moving = false;
                transform.position = new Vector3(1, 0, transform.position.z);
                yield break;
            }
            chunkParent.position = new Vector3(0,0,transform.position.z);
            Move();
            yield return new WaitForFixedUpdate();
        }
    }
    
    void Right()
    {
        destination = new Vector3(1, 0, 1f);
    }

    void Left()
    {
        destination = new Vector3(-1, 0, 1);
    }

    void CheckAlly()
    {
        Ray ray = new Ray(transform.position + Vector3.up*0.5f, transform.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 5,LayerMask.GetMask("Ally")))
        {
            hit.collider.enabled = false;
            OnPassingAlly.Invoke();
        }
    }
    
    
    IEnumerator InputRoutine()
    {
        while (true)
        {
            CheckInput();
            if(moving)
                CheckAlly();
            yield return null;
        }
    }

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
            state = InputState.Hold;
        
        else if (Input.GetMouseButtonUp(0))
            state = InputState.Release;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        EnableCollider(false);
        DisableAnimator();
        transform.DOKill();
        OnEnableRagdoll?.Invoke(false, true);
        OnFail?.Invoke();
        StopAllCoroutines();
        GameManager.Instance.ChangeGameStateUI(GameManager.GameState.Fail);
    }

    public void WinReaction()
    {
        StopAllCoroutines();
        PlayWin();
    }
    
}
