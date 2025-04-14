using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // �ν����Ϳ� ����ȭ
public struct PoolingPair
{
    public PoolableMono prefab;
    public int Count;
}

[CreateAssetMenu (menuName = "SO/PoolList")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> pairs;
}
