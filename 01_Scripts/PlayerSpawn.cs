using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform _spawnTrm;
    [SerializeField] private CharSO _defultSO;
    private void Start()
    {
        if (SceneControlManager.Instance.ChoiceCharSO != null)
            Instantiate(SceneControlManager.Instance.ChoiceCharSO.PlayerObject, _spawnTrm.position, Quaternion.identity);
        else
            Instantiate(_defultSO.PlayerObject, _spawnTrm.position, Quaternion.identity);
    }
}
