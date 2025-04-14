using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharSO")]
public class CharSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite; public Sprite Sprite { get { return _sprite; } }
    [SerializeField] private string _name; public string Name { get { return _name; } }
    [SerializeField] private int _hp; public int Hp { get { return _hp; } }
    [SerializeField] private int _damage; public int Damage { get { return _damage; } }
    [SerializeField] private int _stamina; public int Stamina { get { return _stamina; } }
    [SerializeField] private bool _isDevelopment; public bool IsDevelopment { get { return _isDevelopment; } }
    [SerializeField] private GameObject _playerObject; public GameObject PlayerObject { get { return _playerObject; } }
}
