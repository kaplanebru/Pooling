using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public Distances distances;
    public float spawnDelay = 1;
    public float tweenValue = 2;
    
    protected abstract Vector3 SpawnPos();
    protected abstract void Initialize();
    public abstract IEnumerator WinRoutine();
    protected virtual void CheckState(Player.InputState state){}
    protected virtual void Disable(){}
    protected virtual void Stop() {}
    
    

    private void Start()
    {
        Player.OnStateChange += CheckState;
        Player.OnFail += Stop;
        Finish.OnFinish += Win;
        Initialize();
        transform.position = SpawnPos().x * Vector3.right;
    }
    
    void Win()
    {
        StartCoroutine(nameof(WinRoutine));
    }
    
    private void OnDisable()
    {
        Disable();
        Player.OnFail -= Stop;
        Finish.OnFinish -= Win;
        Player.OnStateChange -= CheckState;
    }
}
