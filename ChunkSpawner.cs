using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;


public class ChunkSpawner : Spawner
{
    private int counter = 0;
    public int loopCount = 2;
    private float constantTweenValue;
    private float offset;
    public Chunk finalChunk;
    protected override void Initialize()
    {
        offset = GameManager.Instance.laneSize;
        loopCount *= ChunkPool.Instance.population;
        constantTweenValue = tweenValue;
        ChunkPool.Instance.GetAll(offset, 0, false);
        Player.OnStateChange += CheckState;
        StartCoroutine(nameof(SpawnChunkRoutine));
    }
    

    protected override Vector3 SpawnPos() => Vector3.zero;


    IEnumerator SpawnChunkRoutine()
    {
        for (int i = 0; i < ChunkPool.Instance.population; i++)
        {
            var chunk = ChunkPool.Instance.Get();
            StartCoroutine(nameof(MoveChunkRoutine), chunk);
        }
        yield break;
        
    }
    

    IEnumerator MoveChunkRoutine(Chunk newChunk)
    {
        counter++;
        if (counter == loopCount)
            newChunk = FinalChunk(newChunk);
        while (newChunk.transform.localPosition.z >= - offset +0.01f)
        {
            newChunk.transform.localPosition = new Vector3(0, 0, Mathf.MoveTowards(newChunk.transform.localPosition.z, - offset, tweenValue));
            yield return new WaitForFixedUpdate();
        }
        newChunk.transform.localPosition = new Vector3(0, 0, - offset);
        RestoreChunk(newChunk);
        
    }


    void RestoreChunk(Chunk newChunk)
    {
        if (counter >= loopCount)
        {
            newChunk.gameObject.SetActive(false);
        }
            
        newChunk.transform.localPosition += offset * ChunkPool.Instance.population * Vector3.forward;
        ChunkPool.Instance.Release(newChunk);
        StartCoroutine("MoveChunkRoutine", newChunk);
    }
    

    Chunk FinalChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
        return Instantiate(finalChunk, chunk.transform.position, Quaternion.identity,transform);
    }

    protected override void CheckState(Player.InputState state)
    {
        switch (state)
        {
            case Player.InputState.Hold:
                tweenValue *= 1.5f;
                break;
            case Player.InputState.Release:
                tweenValue = constantTweenValue;
                break;
        }
    }
    
    protected override void Stop()
    {
        StopAllCoroutines();
    }

    public override IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Stop();
    }
    
 

    



}
