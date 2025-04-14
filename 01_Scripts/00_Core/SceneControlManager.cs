using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneControlManager : MonoSingleton<SceneControlManager>
{
    private Image _image = null;
    private Color _cr;
    private float _fadeCool = 2; public float FadeCool { get => _fadeCool; set => _fadeCool = value; }
    public CharSO ChoiceCharSO;
    private void Start()
    {
        _fadeIn(null);
    }

    /// <summary>
    /// 까메지는거
    /// </summary>
    /// <param name="action"></param>
    public static void FadeOut(Action action) =>
        Instance._fadeOut(action);
    /// <summary>
    /// 투명해지는거
    /// </summary>
    /// <param name="action"></param>
    public static void FadeIn(Action action) =>
     Instance._fadeIn(action);


    /// <summary>
    /// 1=>0
    /// </summary>
    /// <param name="action"></param>
    /// 

    private void _fadeIn(Action action)
    {
        Time.timeScale = 1;
        _image.raycastTarget = false;
        StartCoroutine(fadeInCo(action));
    }
    private IEnumerator fadeInCo(Action action)
    {
        _cr = _image.color;
        while (_image.color.a >= 0)
        {
            _cr.a -= Time.deltaTime / _fadeCool;
            _image.color = _cr;
            yield return null;
        }
        action?.Invoke();
    }

    /// <summary>
    /// 0=>1
    /// </summary>
    /// <param name="action"></param>
    private void _fadeOut(Action action)
    {
        _image.raycastTarget = true;
        StopAllCoroutines();
        StartCoroutine(fadeOutCo(action));
    }
    private IEnumerator fadeOutCo(Action action)
    {
        _cr = _image.color;
        while (_image.color.a <= 1)
        {
            _cr.a += Time.deltaTime / _fadeCool;
            _image.color = _cr;
            yield return null;
        }
        action?.Invoke();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _image = transform.Find("Canvas/FadeImage").GetComponent<Image>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        _fadeIn(null);

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
