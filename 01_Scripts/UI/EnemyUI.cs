using UnityEngine;
using TMPro;
using System.Collections;
public class EnemyUI : MonoBehaviour
{
    private TextMeshPro _lvTxt;
    private Transform _playerTrm;
    private DetecPlayer _detecPlayer;
    private void Awake()
    {
        _lvTxt = GetComponent<TextMeshPro>();
        _playerTrm = Camera.main.transform;
        _detecPlayer = transform.parent.GetComponentInParent<DetecPlayer>();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (_detecPlayer.isMeat)
            _lvTxt.SetText($"<color=red>Lv{_detecPlayer.Age}</color>");
        else
            _lvTxt.SetText($"<color=green>Lv{_detecPlayer.Age}</color>");

    }

    private void FixedUpdate()
    {
        transform.LookAt(_playerTrm);
    }






}
