using System.Threading.Tasks;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private CanvasGroup blocker;
    [SerializeField] private Transform mazeParent;
    
    public CanvasGroup Blocker => blocker;
    public Transform MazeParent => mazeParent;

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