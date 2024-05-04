using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] GameObject _startMenu;

    void Start()
    {
        Player.Instance.IsActive = false;
        CameraManager.Instance.IsActive = false;
    }

    public void StartTuorial()
    {
        _startMenu.SetActive(false);
        Player.Instance.IsActive = true;
        CameraManager.Instance.IsActive = true;
    }
}
