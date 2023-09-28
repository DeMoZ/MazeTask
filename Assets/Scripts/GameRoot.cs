using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class GameRoot : IDisposable
{
    public struct Ctx
    {
        public CanvasGroup blocker;
        public GameConfig gameConfig;
    }
 
    private readonly Ctx _ctx;
    private CompositeDisposable _disposables;

    public GameRoot(Ctx ctx)
    {
        _disposables = new CompositeDisposable();
        _ctx = ctx;
        
        var maze = new MazeGenerator(_ctx.gameConfig.FieldSize, _ctx.gameConfig.StartPoint, _ctx.gameConfig.EndPoint).AddTo(_disposables);
        maze.Generate();
        
        var mazeSpawner = new MazeSpawner(_ctx.gameConfig.CellPrefab).AddTo(_disposables);
        mazeSpawner.Spawn(maze.Maze);
        
        Fade();
    }

    private void Fade()
    {
        _ctx.blocker.DOFade(0, 0.3f);
    }


    public void Dispose()
    {
        _disposables.Dispose();
    }
}