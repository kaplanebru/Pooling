using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceData")]
public class Distances: ScriptableObject
{
    public float allyEndPos = 80;
    public float enemyEndPos = 5;
    public float enemyStartPos = 50;
    public float allyStartPos = 2;
    
    
    public Vector3 EnemyDestination()
    {
        return new Vector3(-1, 0, -(GameManager.Instance.player.transform.position.z - enemyEndPos)); 
    }

    public Vector3 EnemySpawnPos()
    {
        return new Vector3(-1, 0,-(enemyStartPos + GameManager.Instance.player.transform.position.z));
    }
    
    public Vector3 AllyDestination()
    {
        return AllyPool.Instance.totalDistance;
    }
    
    public Vector3 AllySpawnPos()
    {
        return new Vector3(1, 0, allyStartPos + GameManager.Instance.player.transform.position.z);
    }
    
    public Vector3 AllyFinalDestination(Vector3 pos)
    {
        return (pos.z + allyEndPos) * Vector3.forward;
    }
}
