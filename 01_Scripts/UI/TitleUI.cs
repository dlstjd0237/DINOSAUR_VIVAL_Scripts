using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Hellmade.Sound;
public class TitleUI : MonoBehaviour
{
    private UIDocument _doc;
    private Label _iconLabel, _aGameByLabel, _presentsLabel;
    private VisualElement _contain;
    private bool _isSkip;

    [SerializeField]
    private AudioClip musicClip;
    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        var _root = _doc.rootVisualElement;
        _iconLabel = _root.Q<Label>("icon-label");
        _aGameByLabel = _root.Q<Label>("agameby-label");
        _presentsLabel = _root.Q<Label>("presents-label");
        _contain = _root.Q<VisualElement>("contain-box");
        StartCoroutine("TitleStart");
    }

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.anyKeyDown && _isSkip == false)
        {
            Skip();
        }
    }

    private void Skip()
    {
        StopCoroutine("TitleStart");
        _isSkip = true;
        _contain.AddToClassList("on");
        _contain.pickingMode = PickingMode.Ignore;
        _doc.enabled = false;
        EazySoundManager.PlayMusic(musicClip, 1);

    }

    private IEnumerator TitleStart()
    {
        yield return new WaitForSeconds(0.5f);
        _iconLabel.AddToClassList("on");
        yield return new WaitForSeconds(2f);
        _iconLabel.AddToClassList("off");
        yield return new WaitForSeconds(2f);
        _presentsLabel.AddToClassList("on");
        yield return new WaitForSeconds(3.5f);
        _presentsLabel.AddToClassList("off");
        yield return new WaitForSeconds(2f);
        _aGameByLabel.AddToClassList("on");
        yield return new WaitForSeconds(3.5f);
        _aGameByLabel.AddToClassList("off");
        yield return new WaitForSeconds(2);
        _isSkip = true;
        _contain.AddToClassList("on");
        _contain.pickingMode = PickingMode.Ignore;
    }
}
