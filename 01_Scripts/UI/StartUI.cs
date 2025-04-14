using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Hellmade.Sound;
public class StartUI : MonoBehaviour
{
    [SerializeField] private AudioSource _clickAudio;
    [SerializeField] private AudioSource _hoverAudio;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _playSceneName;
    [SerializeField] public List<CharSO> _charSOList;
    [SerializeField] private VisualTreeAsset _charContain;
    private UIDocument _doc;
    private VisualElement _mainUI;
    private VisualElement _optionUI;
    private VisualElement _visualUI;
    private VisualElement _sceneUI;
    private VisualElement _labelContainUI;
    private VisualElement _maincontainUI;
    private Label _headerLabel, _infoLabel;

    private List<Button> _buttons = new();
    private int currentChar = 0;





    private void Start()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;


        _doc = GetComponent<UIDocument>();
        VisualElement _root = _doc.rootVisualElement;
        _mainUI = _root.Q<VisualElement>("leftbarcontain-box");
        _optionUI = _root.Q<VisualElement>("optioncontain-box");
        _sceneUI = _root.Q<VisualElement>("scenecontain-box");
        _visualUI = _root.Q<VisualElement>("visualcontain-box");
        _labelContainUI = _root.Q<VisualElement>("labelcontain-label");
        _maincontainUI = _root.Q<VisualElement>("maincontain-box");
        _mainUI.Q<VisualElement>("iconcontain-box").style.backgroundImage = _icon.texture;


        LabelInit();
        MainButtonInit();
        OptionButtonInit();
        CharacterButtonInit();
        SceneUIInit();
        OptionSliderInit();
        ButtonHoverSet();
        ButtonClickSet();
        
    }

    private void LabelInit()
    {
        VisualElement _root = _doc.rootVisualElement;
        _headerLabel = _root.Q<Label>("statone-label");
        _infoLabel = _root.Q<Label>("stattwo-label");
        StartCoroutine(StateSet());
    }

    private IEnumerator StateSet()
    {
        _labelContainUI.AddToClassList("on");
        yield return new WaitForSeconds(0.2f);
        _headerLabel.text = _charSOList[currentChar].Name;
        _infoLabel.text = $"체력 : {_charSOList[currentChar].Hp}\n데미지 : {_charSOList[currentChar].Damage}\n스테미너 : {_charSOList[currentChar].Stamina}";
        _labelContainUI.RemoveFromClassList("on");

    }

    private void SceneUIInit()
    {
        _visualUI.style.width = (_charSOList.Count + 1) * 800;
        _visualUI.style.left = 0;
        for (int i = 0; i < _charSOList.Count; ++i)
        {
            var contain = _charContain.Instantiate().Q<VisualElement>();
            contain.Q<VisualElement>("chariconcontain-box").style.backgroundImage = _charSOList[i].Sprite.texture;
            _visualUI.Add(contain);
        }
    }

    private void CharacterButtonInit()
    {
        var LeftButton = _sceneUI.Q<Button>("Leftarrow-button");
        LeftButton.RegisterCallback<ClickEvent>(evt => LeftArrow());
        _buttons.Add(LeftButton);

        var RightButton = _sceneUI.Q<Button>("Rightarrow-button");
        RightButton.RegisterCallback<ClickEvent>(evt => RightArrow());
        _buttons.Add(RightButton);

    }

    private void OptionSliderInit()
    {
        var masterSlider = _optionUI.Q<Slider>("masterslider-slider");
        var musicSlider = _optionUI.Q<Slider>("musicslider-slider");
        var sfxslider = _optionUI.Q<Slider>("sfxslider-slider");


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

    private void OptionButtonInit()
    {
        var _optionExitButton = _optionUI.Q<Button>("exitbutton-button");
        _optionExitButton.RegisterCallback<ClickEvent>(evt => OptionClose());
        _buttons.Add(_optionExitButton);
    }

    private void MainButtonInit()
    {
        var _playButton = _mainUI.Q<Button>("playbutton-button");
        _playButton.RegisterCallback<ClickEvent>(evt => playChake());
        _buttons.Add(_playButton);

        var _optionButton = _mainUI.Q<Button>("option-button");
        _optionButton.RegisterCallback<ClickEvent>(evt => OptionOpen());
        _buttons.Add(_optionButton);

        var exitButton = _mainUI.Q<Button>("exit-button");
        exitButton.RegisterCallback<ClickEvent>(evt => { _maincontainUI.AddToClassList("on"); _visualUI.AddToClassList("on"); SceneControlManager.FadeOut(() => Application.Quit()); _mainUI.AddToClassList("on"); });
        _buttons.Add(exitButton);

    }

    private void ButtonHoverSet()
    {
        for (int i = 0; i < _buttons.Count; ++i)
        {
            _buttons[i].RegisterCallback<MouseEnterEvent>(evt => 
            {
                EazySoundManager.PlayUISound(_hoverAudio.clip,1);
            });
        }
    }
    private void ButtonClickSet()
    {
        for (int i = 0; i < _buttons.Count; ++i)
        {
            _buttons[i].RegisterCallback<ClickEvent>(evt => 
            {
                EazySoundManager.PlayUISound(_clickAudio.clip,1);

            });
        }
    }

    private void OptionOpen()
    {
        _mainUI.AddToClassList("on");
        _optionUI.RemoveFromClassList("on");
    }
    private void OptionClose()
    {
        _mainUI.RemoveFromClassList("on");
        _optionUI.AddToClassList("on");
    }   

    private void LeftArrow()
    {
        if (currentChar == 0)
        {
            return;
        }
        else
        {
            --currentChar;
            _visualUI.style.left = currentChar * -800;
            StartCoroutine(StateSet());
        }
    }

    private void RightArrow()
    {
        if (currentChar + 1 >= _charSOList.Count)
        {
            return;
        }
        else
        {
            ++currentChar;
            _visualUI.style.left = currentChar * -800;
            StartCoroutine(StateSet());
        }
    }

    private void playChake()
    {
        if (_charSOList[currentChar].IsDevelopment == true)
            return;
        _maincontainUI.AddToClassList("on");
        _visualUI.AddToClassList("on");
        _mainUI.AddToClassList("on");
        SceneControlManager.Instance.ChoiceCharSO = _charSOList[currentChar];
        SceneControlManager.FadeOut(() => { SceneManagement.LoadScene(_playSceneName); });

        SaveModel.Instance.CheackChar(currentChar);
    }
}
