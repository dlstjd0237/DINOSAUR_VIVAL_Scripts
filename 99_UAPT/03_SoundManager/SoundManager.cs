using UnityEngine;
using UnityEngine.Audio;
using Hellmade.Sound;
#region 사운드 에디터 설정
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    const string INFO = "슬라이더 설정 값\n" +
       "--------------------\n" +
       "Slider Min Value : 0.001\n" +
       "Slider Max Value : 1\n\n" +
        "MixerPath : AudioMixer가 들어있는 폴더 주소\n" +
        "예시) Resources/AudioMixer 폴더 안에있는 \n" +
        "      _Mixer 을 가져오고 싶으면\n" +
        "MixerPath 안에다 AudioMixer/_Mixer 을 써주면 된다.";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
        SoundManager soundManager = (SoundManager)target;
        // [CustomEditor(typeof(SoundManager))] 에 써져있는 target을 가져옴
        if (GUILayout.Button("사운드 데이터 삭제"))
        {
            soundManager.DeleteSoundData();
        }
    }


}
#endif
#endregion
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private string _mixerPath;
    private AudioMixer _mixer;
    public AudioMixer Mixer
    {
        get
        {
            MixerNullChake();
            return _mixer;
        }
    }
    private void OnEnable()
    {
        MixerNullChake();
    }
    public void DeleteSoundData()
    {
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("SFXVoluem");
        #region 사운드 테스트
#if UNITY_EDITOR

        Debug.Log("사운드 저장 데이터 삭제 됨");
#endif
        #endregion
    }

    public void VolumeSetMaster(float volume)
    {
        Debug.Log(volume);
        MixerNullChake();
        EazySoundManager.GlobalVolume = volume;

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void VolumeSetMusic(float volume)
    {
        MixerNullChake();
        EazySoundManager.GlobalMusicVolume = volume;

      PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void VolumeSetSFX(float volume)
    {
        MixerNullChake();
        EazySoundManager.GlobalUISoundsVolume = volume;
        PlayerPrefs.SetFloat("SFXVoluem", volume);
    }

    /// <summary>
    /// Mixer가 Null인경우 재정의
    /// </summary>
    private void MixerNullChake()
    {
        if (_mixer != null)
            return;
        _mixer = Resources.Load<AudioMixer>(_mixerPath);
    }
    /// <summary>
    /// 프리펩에 저장된 내용이 없다면 false
    /// </summary>
    /// <returns></returns>
    public bool MasterVolumeChake()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 프리펩에 저장된 내용이 없다면 false
    /// </summary>
    /// <returns></returns>
    public bool MusicVolumeChake()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 프리펩에 저장된 내용이 없다면 false
    /// </summary>
    /// <returns></returns>
    public bool SFXVoluemChake()
    {
        if (PlayerPrefs.HasKey("SFXVoluem"))
        {
            return false;
        }
        return true;
    }
}
