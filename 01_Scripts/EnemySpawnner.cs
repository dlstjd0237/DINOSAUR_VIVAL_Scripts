using System.Collections;
using System.Collections.Generic;
using FIMSpace.Generating;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EnemySpawnner))]
public class EnemySpawnnerEditor : Editor
{
    private const string INFO =
        "Enemy Tag List : �������� ��ȯ�� Enemy\nPoolManager�� ����� Tag�� �ۼ�\nSpawn Pos : ���� 20 �ȿ� �������� ����\nSpawn Amount : ������ ��";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif


public class EnemySpawnner : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPointList;
    [SerializeField] private int _grassSpawnAmount;
    [SerializeField] private int _meetSpawnAmount;
    [SerializeField] private List<PoolingListSO> _grassList;
    [SerializeField] private List<PoolingListSO> _meetList;
    [SerializeField] private int _enemySpawnMaxAmount = 300;
    private List<Transform> enemies = new List<Transform>();
    private Transform player;

    private void Awake()
    {
        CreatePoolManager();
    }


    private void CreatePoolManager()
    {
        PoolManager.Instance = new PoolManager(transform);
        for (int i = 0; i < _grassList.Count; i++)
        {
            for (int j = 0; j < _grassList[i].pairs.Count; j++)
            {
                PoolManager.Instance.CreatePool(_grassList[i].pairs[j].prefab, _grassList[i].pairs[j].Count);
            }
        }

        for (int i = 0; i < _meetList.Count; i++)
        {
            for (int j = 0; j < _meetList[i].pairs.Count; j++)
            {
                PoolManager.Instance.CreatePool(_meetList[i].pairs[j].prefab, _meetList[i].pairs[j].Count);
            }
        }
    }

    private IEnumerator Start()
    {
        yield return YieldCache.WaitUntil(() => ((GameScene)SceneManagement.Instance.CurrentScene).Player != null);
        player = ((GameScene)SceneManagement.Instance.CurrentScene).Player.transform;
        InvokeRepeating("SpawnEnemey", 0, 180);
    }

    private void SpawnEnemey()
    {
        if (enemies.Count > _enemySpawnMaxAmount)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].gameObject.activeSelf) enemies.RemoveAt(i);
                if (Vector3.Distance(enemies[i].position, player.position) >= 100)
                {
                    PoolManager.Instance.Push(enemies[i].GetComponent<PoolableMono>());
                    enemies.RemoveAt(i);
                }
            }
        }

        if (_grassList != null && _meetList != null)
            for (int i = 0; i < _enemySpawnMaxAmount - enemies.Count; i++)
            {
                string name;
                if (Random.Range(0, 100) <= 30)
                {
                    var a = _meetList[Random.Range(0, _meetList.Count)];


                    name = a.pairs[Random.Range(0, a.pairs.Count)].prefab.name;


                    Spawn(name); //����
                }
                else
                {
                    var a = _grassList[Random.Range(0, _grassList.Count)];
                    name = a.pairs[Random.Range(0, a.pairs.Count)].prefab.name;
                    Spawn(name); //�ʽ�
                }
            }
    }

    private void Spawn(string EnemyTag)
    {
        var qwer = SpawnPointList;
        var DefultTrm = SpawnPointList;
        int SpawnTrmIndex = 0;
        DefultTrm.Shuffle();

        Vector3 _spawnPos = Random.insideUnitSphere * 50 + player.position;
        NavMeshAgent agent;


        var a = PoolManager.Instance.Pop(EnemyTag);
        if (a.TryGetComponent(out agent))
        {
            agent.enabled = false;
            StartCoroutine(EnableAgnet(agent));
        }
        else
        {
            agent = a.GetComponentInChildren<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                StartCoroutine(EnableAgnet(agent));
            }
        }

        Vector3 pos = RandomNavSphere(_spawnPos, 20, NavMesh.AllAreas);
        a.transform.position = pos;
        enemies.Add(a.transform);
    }

    IEnumerator EnableAgnet(NavMeshAgent agent)
    {
        yield return YieldCache.WaitForSeconds(.5f);
        agent.enabled = true;
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int mask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        if (NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, Mathf.Infinity, mask))
        {
            return navHit.position;
        }

        return origin;
    }
}