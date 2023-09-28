using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    private static GameContext _instance;

    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private CanvasGroup blocker;
    [SerializeField] private SwipeDetector swipeDetector;
    [SerializeField] private Transform mazeParent;
    
    private CompositeDisposable _disposables;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }

        _disposables = new CompositeDisposable();
        //DontDestroyOnLoad(gameObject);

        Debug.Log($"[EntryRoot][time] Loading scene start.. {Time.realtimeSinceStartup}");
    }

    private async void Start()
    {
        await CreateAppSettings();
        CreateRootEntity();
    }

    private async Task CreateAppSettings()
    {
        Application.targetFrameRate = 60;
        await Task.Delay(0);
    }

    private async void CreateRootEntity()
    {
        await Task.Delay(0);

        var rootEntity = new GameRoot(new GameRoot.Ctx
        {
            gameConfig = gameConfig,
            blocker = blocker,
            swipeDetector = swipeDetector,
            mazeParent = mazeParent,
        }).AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}