using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIManager : Singleton<MatchUIManager>
{
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _restartMenu;
    [SerializeField] GameObject _pauseMenu;

    [Space]
    [SerializeField] Transform _healthIconsParent;
    [SerializeField] GameObject _healthIconPrefab;

    void Start()
    {
        _gameUI.SetActive(true);
        _restartMenu.SetActive(false);
        _pauseMenu.SetActive(false);

        Player.Instance.DeathBroadcaster.AddListener(ShowRestartScreen);
        Player.Instance.HitpointsUpdateBroadcaster.AddListener(UpdateHitpoints);
        GameManager.Instance.GamePausedBroadcaster.AddListener(HideShowPauseMenu);
        GameManager.Instance.GameUnPausedBroadcaster.AddListener(HideShowPauseMenu);
        UpdateHitpoints();
    }

    public void HideShowPauseMenu()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        _gameUI.SetActive(!_gameUI.activeSelf);
    }

    public void UpdateHitpoints()
    {

        foreach(Transform child in _healthIconsParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < Player.Instance.HitPoints; i++)
        {
            Instantiate(_healthIconPrefab, _healthIconsParent);
        }
    }

    public void ShowRestartScreen()
    {
        _gameUI.SetActive(false);
        _restartMenu.SetActive(true);
    }
}
