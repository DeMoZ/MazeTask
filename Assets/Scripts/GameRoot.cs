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

    public GameRoot(Ctx ctx)
    {
        _disposables = new CompositeDisposable();
        _ctx = ctx;

        var maze = new MazeGenerator(_ctx.gameConfig.FieldSize, _ctx.gameConfig.StartPoint, _ctx.gameConfig.EndPoint)
            .AddTo(_disposables);
        maze.Generate();

        var mazeSpawner = new MazeSpawner(_ctx.gameConfig.CellPrefab, _ctx.mazeParent).AddTo(_disposables);
        mazeSpawner.Spawn(maze.Maze);

        var playerPosition = mazeSpawner.GetInCellCoordinates(_ctx.gameConfig.StartPoint);
        var player = Object.Instantiate(_ctx.gameConfig.PlayerPrefab, playerPosition,
            quaternion.identity);

        var moveController = new MoveController(player, _ctx.swipeDetector, maze.Maze, mazeSpawner,
            _ctx.gameConfig.StartPoint, _ctx.gameConfig.BallSpeed);

        Fade(false);
    }

    private void Fade(bool showBlocker)
    {
        var endValue = showBlocker ? 1 : 0;
        _ctx.blocker.DOFade(endValue, 0.3f);
    }


    public void Dispose()
    {
        _disposables.Dispose();
    }
}