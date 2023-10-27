using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class GameRoot : IDisposable
{
    private CompositeDisposable _disposables;
    private MazeGenerator _maze;
    private PlayerController _playerController;
    private MazeSpawner _mazeSpawner;
    private readonly GameContext _gc;


    public GameRoot(GameService gs, GameContext gc, MazeGenerator maze, MazeSpawner mazeSpawner, PlayerController playerController)
    {
        Debug.Log($"{this} created");
        _gc = gc;
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

        // _ctx.Blocker.DOFade(0, 0.3f);
    }

    private void OnReachEnd()
    {
        Restart();
    }

    private void Restart()
    {
        _gc.Blocker.DOFade(1, 0.3f).OnComplete(() => { CreateObjects(); });
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}