using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Hellmade.Sound;
public class TitleButton : MonoBehaviour
{
    private Color _cr;
    [SerializeField] private TextMeshProUGUI _toych;
    [SerializeField] private string _nextSceneName;
    [SerializeField] private Button _btn;

    private void Awake()
    {
        StartCoroutine(TextFadeOut());
    }

    private IEnumerator TextFadeOut()
    {
        _cr = _toych.color;
        while (_toych.color.a <= 1)
        {
            _cr.a += Time.deltaTime / 2f;
            _toych.color = _cr;
            yield return null;
        }
        StartCoroutine(TextFadeIn());
    }
    private IEnumerator TextFadeIn()
    {
        _cr = _toych.color;
        while (_toych.color.a >= 0)
        {
            _cr.a -= Time.deltaTime / 2f;
            _toych.color = _cr;
            yield return null;
        }
        StartCoroutine(TextFadeOut());
    }

    public void TitleButtonClike()
    {
        _btn.enabled = false;
            EazySoundManager.StopAllMusic(1f);
        SceneControlManager.FadeOut(() =>
        {
            SceneManager.LoadScene(_nextSceneName);
        });
    }
}
