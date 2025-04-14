using UnityEngine;

public class Fixed : MonoBehaviour
{
    [SerializeField] private int _setWidth = 1920;
    [SerializeField] private int _setHeight = 1080;
    void Start()
    {
        Screen.SetResolution(_setWidth, _setHeight, true);
    }
}
