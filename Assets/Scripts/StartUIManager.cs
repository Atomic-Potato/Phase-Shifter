using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] GameObject _startMenu;
    [SerializeField] GameObject _tutorialItems;
    [SerializeField] List<GameObject> _tutorialPanels;

    int _tutorialIndex = 0;

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
        _tutorialItems.SetActive(true);
        ShowNextPanel();
    }

    public void ShowNextPanel()
    {
        if (_tutorialIndex > _tutorialPanels.Count - 1)
        {
            SceneManager.LoadScene("Level01");
            return;
        }
        if (_tutorialIndex > 0)
            _tutorialPanels[_tutorialIndex-1].SetActive(false);
        _tutorialPanels[_tutorialIndex].SetActive(true);
        _tutorialIndex++;
    }
}
