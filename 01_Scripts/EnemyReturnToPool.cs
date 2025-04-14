using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReturnToPool : PoolableMono
{
    [SerializeField] private string _name;
    public override void Init()
    {

    }

    private void OnDisable()
    {
        //PoolManager.Instance.Push(this);
    }
}
