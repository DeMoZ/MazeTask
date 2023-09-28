using System;
using DG.Tweening;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameRoot : IDisposable
{
    public struct Ctx
    {
        public CanvasGroup blocker;
        public GameConfig gameConfig;
        public SwipeDetector swipeDetector;
        public Transform mazeParent;
    }

    private readonly Ctx _ctx;
    private CompositeDisposable _disposables;
    private MazeGenerator _maze;
    private PlayerController _playerController;
    private MazeSpawner _mazeSpawner;

    public GameRoot(Ctx ctx)
    {
        _disposables = new CompositeDisposable();
        _ctx = ctx;

        var onReachEnd = new ReactiveCommand().AddTo(_disposables);
        _maze = new MazeGenerator(_ctx.gameConfig.FieldSize, _ctx.gameConfig.StartPoint, _ctx.gameConfig.EndPoint)
            .AddTo(_disposables);
        _mazeSpawner = new MazeSpawner(_ctx.gameConfig.CellPrefab, _ctx.mazeParent).AddTo(_disposables);
        _playerController = new PlayerController(_ctx.swipeDetector, _mazeSpawner, _ctx.gameConfig,
            onReachEnd).AddTo(_disposables);

        onReachEnd.Subscribe(_ => OnReachEnd()).AddTo(_disposables);
        CreateObjects();
    }

    private void CreateObjects()
    {
        _maze.Generate();
        _mazeSpawner.Spawn(_maze.Maze);
        _playerController.Set(_maze.Maze);
        
        _ctx.blocker.DOFade(0, 0.3f);
    }

    private void OnReachEnd()
    {
        // some win logic
        Restart();
    }
    
    private void Restart()
    {
        _ctx.blocker.DOFade(1, 0.3f).OnComplete(() =>
        {
            CreateObjects();
        });
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}