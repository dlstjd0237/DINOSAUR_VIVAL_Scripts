using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;
using Invector.vCharacterController;
using Hellmade.Sound;
public class PauseUI : MonoBehaviour
{
    [SerializeField] private AudioSource _hoverAudio;
    [SerializeField] private AudioSource _clickAudio;
    [SerializeField] private string _exitSceneName;
    [SerializeField] private string _happyEndingSceneName;
    [SerializeField] private string _badEndingSceneName;

    private UIDocument _doc;
    private VisualElement _pauseBox;
    private VisualElement _optionBox;
    private bool _pauseOn;
    private bool _optionOn;

    private List<Button> _buttonList = new();
    private GameScene _player;
    [SerializeField]
    private AudioClip musicClip;

    private void Awake()
    {
        EazySoundManager.PlayMusic(musicClip, .6f, true, false);
    }
    private IEnumerator Start()
    {
        yield return YieldCache.WaitUntil(() => ((GameScene)SceneManagement.Instance.CurrentScene).Player != null);
        EazySoundManager.PlayMusic(musicClip, .6f);
        _doc = GetComponent<UIDocument>();
        var _root = _doc.rootVisualElement;
        _pauseBox = _root.Q<VisualElement>("menuecontain-box");
        _optionBox = _root.Q<VisualElement>("optioncontain-box");

        _player = (GameScene)SceneManagement.Instance.CurrentScene;

        ButtonSet();
        OptionSliderInit();
        ButtonSoundSet();
    }
    private void OptionSliderInit()
    {
        var masterSlider = _optionBox.Q<Slider>("masterslider-slider");
        var musicSlider = _optionBox.Q<Slider>("musicslider-slider");
        var sfxslider = _optionBox.Q<Slider>("sfxslider-slider");


        if (SoundManager.Instance.MasterVolumeChake() == false)
            masterSlider.value = 0.5f;
        else
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            SoundManager.Instance.VolumeSetMaster(masterSlider.value);
        }

        if (SoundManager.Instance.MusicVolumeChake() == false)
            musicSlider.value = 0.5f;
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SoundManager.Instance.VolumeSetMusic(musicSlider.value);
        }

        if (SoundManager.Instance.SFXVoluemChake() == false)
        {
            sfxslider.value = 0.5f;
        }
        else
        {
            sfxslider.value = PlayerPrefs.GetFloat("SFXVoluem");
            SoundManager.Instance.VolumeSetSFX(sfxslider.value);
        }

        masterSlider.RegisterCallback<ChangeEvent<float>>(evt => SoundManager.Instance.VolumeSetMaster(masterSlider.value));
        musicSlider.RegisterCallback<ChangeEvent<float>>(evt => SoundManager.Instance.VolumeSetMusic(musicSlider.value));
        sfxslider.RegisterCallback<ChangeEvent<float>>(evt => SoundManager.Instance.VolumeSetSFX(sfxslider.value));
    }


    private void ButtonSet()
    {
        var _optionButton = _pauseBox.Q<Button>("option-button");
        _optionButton.RegisterCallback<ClickEvent>(evt => { ClosePause(); OpenOption(); _optionOn = true; });
        _buttonList.Add(_optionButton);

        var _continueButton = _pauseBox.Q<Button>("continue-button");
        _continueButton.RegisterCallback<ClickEvent>(evt => { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; ClosePause(); _pauseOn = false; Time.timeScale = 1; });
        _buttonList.Add(_continueButton);

        var _exitButton = _pauseBox.Q<Button>("exit-button");
        _exitButton.RegisterCallback<ClickEvent>(evt => { ClosePause(); SceneControlManager.FadeOut(() => SceneManager.LoadScene(_exitSceneName)); Time.timeScale = 1; });
        _buttonList.Add(_exitButton);

        var _optionExit = _optionBox.Q<Button>("exitbutton-button");
        _optionExit.RegisterCallback<ClickEvent>(evt => { CloseOption(); OpenPause(); _optionOn = false; });
        _buttonList.Add(_optionExit);

        _pauseBox.Q<Button>("leveup-button").RegisterCallback<ClickEvent>(evt => _player.Player.GetComponent<vThirdPersonController>().LevelUP());

        _pauseBox.Q<Button>("ending-button").RegisterCallback<ClickEvent>(evt => SceneControlManager.FadeOut(() => SceneManager.LoadScene(_happyEndingSceneName)));

        _pauseBox.Q<Button>("endingtwo-button").RegisterCallback<ClickEvent>(evt => SceneControlManager.FadeOut(() => SceneManager.LoadScene(_badEndingSceneName)));
    }

    private void ButtonSoundSet()
    {
        for (int i = 0; i < _buttonList.Count; ++i)
        {
            _buttonList[i].RegisterCallback<MouseEnterEvent>(ect =>
            {
                EazySoundManager.PlayUISound(_hoverAudio.clip);
            });
        }
        for (int i = 0; i < _buttonList.Count; ++i)
        {
            _buttonList[i].RegisterCallback<ClickEvent>(ect =>
            {
                EazySoundManager.PlayUISound(_clickAudio.clip);
            });
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (_optionOn == true && _pauseOn == true)//�ɼ�â�� ��������
            {
                _optionOn = false;
                CloseOption();
                OpenPause();
            }
            else if (_optionOn == false && _pauseOn == true)//�ɼ��� ���������� ����� ��������
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _pauseOn = false;
                ClosePause();
                Time.timeScale = 1;
            }
            else if (_optionOn == false && _pauseOn == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                _pauseOn = true;
                OpenPause();
                Time.timeScale = 0;
            }

        }
    }

    private void OpenPause()
    {
        EazySoundManager.PauseAllMusic();
        _pauseBox.RemoveFromClassList("on");
    }
    private void ClosePause()
    {
        EazySoundManager.ResumeAllMusic();

        _pauseBox.AddToClassList("on");
    }
    private void OpenOption()
    {
        EazySoundManager.PauseAllMusic();

        _optionBox.RemoveFromClassList("on");
    }
    private void CloseOption()
    {
        EazySoundManager.ResumeAllMusic();

        _optionBox.AddToClassList("on");
    }
}
