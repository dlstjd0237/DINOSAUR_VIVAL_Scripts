using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
public class StateBar : MonoBehaviour
{
    private Image _lvGaugeImage, _hpBarImage, _staminaBarImage;
    private TextMeshProUGUI _hpText, _staminaText, _lvText;
    private Renderer _fillMat;
    [SerializeField] private float _maxExp = 1000;
    [SerializeField] private float _currentExp = 0;
    private GameScene currentGameScene;

    private IEnumerator Start()
    {
        yield return YieldCache.WaitUntil(() => SceneManagement.Instance.CurrentScene as GameScene != null);
        currentGameScene = (SceneManagement.Instance.CurrentScene as GameScene);
        currentGameScene.Player.OnChangedCurrentHp += ChangeHp;
        currentGameScene.Player.OnChangedCurrentXP += ChangeExp;
        currentGameScene.Player.OnChangedCurrentStamina += ChangeStamina;

        _lvGaugeImage = transform.Find("LvCircle/LvGaugeBackground/LvGauge1").GetComponent<Image>();
        _hpBarImage = transform.Find("HPBar/HP").GetComponent<Image>();
        _staminaBarImage = transform.Find("StaminaBar/Stamina").GetComponent<Image>();

        _hpText = transform.Find("HPBar/HPText").GetComponent<TextMeshProUGUI>();
        _staminaText = transform.Find("StaminaBar/StaminaText ").GetComponent<TextMeshProUGUI>();

        _lvText = transform.Find("LvCircle/LvGaugeBackground/LvGauge1/LvText").GetComponent<TextMeshProUGUI>();
        _lvText.SetText($"Lv{currentGameScene.Player.CurrentLevel}");
        StateInit();
    }

    private void OnDisable()
    {
        currentGameScene.Player.OnChangedCurrentHp -= ChangeHp;
        currentGameScene.Player.OnChangedCurrentXP -= ChangeExp;
        currentGameScene.Player.OnChangedCurrentStamina -= ChangeStamina;
    }

    private void StateInit()
    {
        ChangeHp(currentGameScene.Player.CurrentHp);
        ChangeExp(currentGameScene.Player.CurrentXP);
        ChangeStamina(currentGameScene.Player.CurrentStamina);
    }

    private void ChangeHp(float currentHp)
    {
        _hpBarImage.DOFillAmount(currentHp / currentGameScene.Player.MaxHP, 0.5f);
        _hpText.SetText($"{(int)currentHp}/{currentGameScene.Player.MaxHP}");
    }

    private void ChangeExp(int currentExp)
    {
        _lvGaugeImage.material.DOFloat((float)currentExp / (float)currentGameScene.Player.NextXP, "_FillLevel", 0.5f);
        _lvText.SetText($"Lv{currentGameScene.Player.CurrentLevel}");
    }

    private void ChangeStamina(float currentStamina)
    {
        _staminaBarImage.DOFillAmount(currentStamina / currentGameScene.Player.MaxStamina, 0.5f);
        _staminaText.SetText($"{(int)currentStamina}/{currentGameScene.Player.MaxStamina}");
    }
}