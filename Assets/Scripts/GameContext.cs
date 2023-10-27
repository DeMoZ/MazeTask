using System.Threading.Tasks;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    private static GameContext _instance;

    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private CanvasGroup blocker;
    [SerializeField] private Transform mazeParent;
    
    public CanvasGroup Blocker => blocker;
    public Transform MazeParent => mazeParent;
    public GameConfig GameConfig => gameConfig;

    private async void Start()
    {
        await CreateAppSettings();
    }

    private async Task CreateAppSettings()
    {
        Application.targetFrameRate = 60;
        await Task.Delay(0);
    }
}