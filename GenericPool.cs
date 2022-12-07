using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public abstract class GenericPool<T> : MonoBehaviour where T : Component
{
    public static GenericPool<T> Instance;
    public T prefab;
    private Queue<T> _pool = new Queue<T>();
    public int population = 10;
    public Transform poolParent;
    private void Awake() => Instance = this;
    private void OnEnable() => AddToPool(population);

    public T Get()
    {
        T element = _pool.Dequeue();
        element.gameObject.SetActive(true);
        _pool.Enqueue(element);
        return element; 
    }
    
    public void Release(T item)
    {
        if(_pool.Contains(item))
            _pool.Enqueue(item);
    }

    public Vector3 totalDistance;
    public void GetAll(float interval, float offset, bool random)
    {
        
        for (int i = 0; i < population; i++)
        {
            float multiplier = random ? Random.Range(1, 1.6f) : 1;
            T item= Get();
            var dist = Vector3.forward * (i+offset) * interval * multiplier;
            item.transform.localPosition += dist;
            if (i == population - 1)
                totalDistance = item.transform.localPosition + Vector3.forward * interval;
        }
    }
    
    
    void AddToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T element = Instantiate(prefab, poolParent);
            element.gameObject.SetActive(false);
            _pool.Enqueue(element);
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            T item= Get();
            item.gameObject.SetActive(false);
        }
    }
    
    
}
