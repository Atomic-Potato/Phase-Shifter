using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIManager : Singleton<MatchUIManager>
{
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _restartMenu;
    [SerializeField] GameObject _pauseMenu;

    void Start()
    {
        _gameUI.SetActive(true);
        _restartMenu.SetActive(false);
        _pauseMenu.SetActive(false);

        Player.Instance.DeathBroadcaster.AddListener(ShowRestartScreen);
        Player.Instance.HitpointsUpdateBroadcaster.AddListener(UpdateHitpoints);
    }

    public void UpdateHitpoints()
    {

    }

    public void ShowRestartScreen()
    {
        _gameUI.SetActive(false);
        _restartMenu.SetActive(true);
    }
}
