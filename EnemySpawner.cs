using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner
{
    private float constantSpawnDelay;
    public float spawnMultiplier = 10;
    protected override void Initialize()
    {
        constantSpawnDelay = spawnDelay;
        SetSpawnerRot();
        StartCoroutine(nameof(SpawnRoutine));
    }
    
    
    protected override Vector3 SpawnPos() => distances.EnemySpawnPos();
    
    float GetRandomDelay() => Random.Range(spawnDelay, spawnDelay + 1);

    private void Spawn<T>(T item) where T : Component
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.forward * SpawnPos().z;
        Move(item);
    }

    private void Move<T>(T item) where T: Component
    {
        item.transform.DOLocalMoveZ(distances.EnemyDestination().z, tweenValue).SetEase(Ease.Linear).
            OnComplete(() =>GenericPool<T>.Instance.Release(item));
    }

    public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn(EnemyPool.Instance.Get());
            yield return new WaitForSeconds(GetRandomDelay()); 
        }
    }

    void SetSpawnerRot()
    {
        Vector3 direction = (distances.EnemyDestination() - SpawnPos()).normalized * -1;
        Quaternion rotation = Quaternion.LookRotation(direction); 
        transform.rotation = rotation;  
    }


    protected override void CheckState(Player.InputState state)
    {
        switch (state)
        {
            case Player.InputState.Hold:
                spawnDelay /= spawnMultiplier;
                break;
            case Player.InputState.Release:
                spawnDelay = constantSpawnDelay;
                break;
        }
    }

    public override IEnumerator WinRoutine()
    {
        //yield return new WaitForSeconds(0.1f);
        StopAllCoroutines();
        transform.DOKill();
        EnemyPool.Instance.DisableAll();
        yield break;
    }
}
