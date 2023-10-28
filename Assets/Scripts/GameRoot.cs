using System;
using DG.Tweening;
using UniRx;

public class GameRoot : IDisposable
{
    private readonly CompositeDisposable _disposables;
    private readonly MazeGenerator _maze;
    private readonly PlayerController _playerController;
    private readonly MazeSpawner _mazeSpawner;
    private readonly GameContext _ctx;
    private readonly GameService _gs;
    
    public GameRoot(GameService gs, GameContext ctx, MazeGenerator maze, MazeSpawner mazeSpawner,
        PlayerController playerController)
    {
        _gs = gs;
        _ctx = ctx;
        _maze = maze;
        _mazeSpawner = mazeSpawner;
        _playerController = playerController;
        _disposables = new CompositeDisposable();

        _gs.OnReachEnd += OnReachEnd;
        CreateObjects();
    }

    private void CreateObjects()
    {
        _maze.Generate();
        _mazeSpawner.Spawn(_maze.Maze);
        _playerController.Set(_maze.Maze);

        _ctx.Blocker.DOFade(0, 0.3f);
    }

    private void OnReachEnd()
    {
        Restart();
    }

    private void Restart()
    {
        _ctx.Blocker.DOFade(1, 0.3f).OnComplete(CreateObjects);
    }

    public void Dispose()
    {
        _gs.OnReachEnd -= OnReachEnd;

        _disposables.Dispose();
    }
}