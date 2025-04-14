using UnityEngine;

public class SoundEffectObject : MonoBehaviour
{

    private enum SoundType { MUSIC, EFFECT }
    [Header("SFX의 경우 EFFECT로")] [SerializeField] private SoundType _soundType;

    private AudioSource _audio;
    private void OnEnable()
    {
        _audio.Play();
        if (_soundType == SoundType.EFFECT)
            Invoke("ObjectSet", _audio.clip.length);
        else if (_soundType == SoundType.MUSIC)
            _audio.loop = true;
    }
    private void ObjectSet()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PoolManagerq.ReturnToPool(this.gameObject);
    }

}
