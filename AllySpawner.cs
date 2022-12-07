using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AllySpawner : Spawner
{
    public float interval = 5;
    public float poolDelay = 0.5f;
    protected override void Initialize()
    {
        AllyPool.Instance.GetAll(interval, 1, true);
        Player.OnPassingAlly += SpawnAlly;
    }

    protected override void Disable()
    {
        Player.OnPassingAlly -= SpawnAlly;
    }

    protected override Vector3 SpawnPos() => distances.AllySpawnPos();
    public void SpawnAlly()
    {
        StartCoroutine(nameof(SpawnAllyRoutine));
    }

    IEnumerator SpawnAllyRoutine()
    {
        yield return new WaitForSeconds(poolDelay);
        var newAlly = AllyPool.Instance.Get();
        newAlly.transform.localPosition += distances.AllyDestination();
        newAlly.EnableCollider(true);
    }
    
    protected override void Stop()
    {
        StopAllCoroutines();
        transform.DOMoveZ(distances.AllyFinalDestination(transform.position).z, tweenValue).SetEase(Ease.Linear).OnComplete(()=>Destroy(gameObject)); //transform.position.z + 80
    }

    public override IEnumerator WinRoutine()
    {
        Stop();
        yield break;
    }
    
}
