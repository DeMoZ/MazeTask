using System;
using DG.Tweening;
using UniRx;

public class GameRoot : IDisposable
{
    private CompositeDisposable _disposables;
    private MazeGenerator _maze;
    private PlayerController _playerController;
    private MazeSpawner _mazeSpawner;
    private readonly GameContext _ctx;


    public GameRoot(GameService gs, GameContext ctx, MazeGenerator maze, MazeSpawner mazeSpawner, PlayerController playerController)
    {
        _ctx = ctx;
        _maze = maze;
        _mazeSpawner = mazeSpawner;
        _playerController = playerController;
        _disposables = new CompositeDisposable();

        gs.OnReachEnd.Subscribe(_ => OnReachEnd()).AddTo(_disposables);
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
        _disposables.Dispose();
    }
}