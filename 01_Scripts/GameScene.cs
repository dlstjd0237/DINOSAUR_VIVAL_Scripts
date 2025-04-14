using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using Invector.vCharacterController;
using Parkjung2016;
using UnityEngine;

public class GameScene : SceneBase
{
    private vThirdPersonController player;


    public vThirdPersonController Player => player;

    
    [SerializeField] private DamageNumber damageNumberPrefab;

    protected override void Awake()
    {

        VolumeManager.Instance = new VolumeManager();
        TimeManager.Instance = new TimeManager();
        base.Awake();

        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        yield return YieldCache.WaitUntil(() => FindObjectOfType<vThirdPersonController>());
        player = FindObjectOfType<vThirdPersonController>();
    }

    public void SpawnDamageNumber(float damage, Vector3 pos)
    {
        damageNumberPrefab.Spawn(pos, damage);
    }
}