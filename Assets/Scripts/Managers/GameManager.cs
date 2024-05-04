using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    public UnityEvent GamePausedBroadcaster;
    public UnityEvent GameUnPausedBroadcaster;
    public UnityEvent GameEndBroadcaster;

    float _timeScaleCache;

    new void Awake()
    {
        base.Awake();
        GamePausedBroadcaster = new UnityEvent();
        GameUnPausedBroadcaster = new UnityEvent();
        GameEndBroadcaster = new UnityEvent();
    }

    void Start()
    {
        EnemiesManager.Instance.NoEnemiesLeftBroadcaster.AddListener(() => GameEndBroadcaster.Invoke());
        Player.Instance.IsActive = true;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }
#endif

        if (Input.GetKeyDown(KeyCode.Escape) && Player.Instance.IsAlive)
        {
            PauseUnpauseGame();
        }
    }

    public void PauseUnpauseGame()
    {
        if (Time.timeScale != 0)
        {
            _timeScaleCache = Time.timeScale;
            Time.timeScale = 0;
            GamePausedBroadcaster.Invoke();
        }
        else
        {
            Time.timeScale = _timeScaleCache;
            GameUnPausedBroadcaster.Invoke();
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
