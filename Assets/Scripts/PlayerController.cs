using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerController : IDisposable
{
    private readonly SwipeDetector _swipeDetector;
    private readonly MazeSpawner _mazeSpawner;
    private readonly GameConfig _gameConfig;
    private readonly ReactiveCommand _onReachEnd;

    private Cell[,] _maze;
    private GameObject _player;
    private Vector2Int _currentCell;
    private bool _isMoving;

    public PlayerController(SwipeDetector swipeDetector, MazeSpawner mazeSpawner,
        GameConfig gameConfig, ReactiveCommand onReachEnd)
    {
        _swipeDetector = swipeDetector;
        _mazeSpawner = mazeSpawner;
        _gameConfig = gameConfig;
        _onReachEnd = onReachEnd;

        _swipeDetector.OnSwipe += OnSwipe;
    }

    public void Set(Cell[,] maze)
    {
        _maze = maze;
        _currentCell = _gameConfig.StartPoint;

        if (_player != null) GameObject.Destroy(_player);

        var playerPosition = _mazeSpawner.GetInCellCoordinates(_currentCell);
        _player = Object.Instantiate(_gameConfig.PlayerPrefab, playerPosition, Quaternion.identity);
    }

    public void Dispose()
    {
        Object.Destroy(_player);
        _swipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeDetector.SwipeDirection direction)
    {
        if (_isMoving) return;

        var lenX = _maze.GetLength(0);
        var lenY = _maze.GetLength(1);

        var nextX = _currentCell.x;
        var nextY = _currentCell.y;
        var steps = 0;
        var isExit = false;

        switch (direction)
        {
            case SwipeDetector.SwipeDirection.Left:
                for (var x = _currentCell.x; x > 0; x--)
                {
                    if (!_maze[x, nextY].wallLeft)
                    {
                        nextX = x - 1;
                        steps++;

                        if (IsEnd(ref isExit, nextX, nextY)) break;
                        if (nextY < lenY - 1 && !_maze[nextX, nextY + 1].wallBottom) break;
                        if (!_maze[nextX, nextY].wallBottom) break;
                    }
                    else break;
                }

                break;

            case SwipeDetector.SwipeDirection.Right:
                for (var x = _currentCell.x; x < _maze.GetLength(0); x++)
                {
                    if (!_maze[x + 1, nextY].wallLeft)
                    {
                        nextX = x + 1;
                        steps++;

                        if (IsEnd(ref isExit, nextX, nextY)) break;
                        if (nextY < lenY - 1 && !_maze[nextX, nextY + 1].wallBottom) break;
                        if (!_maze[nextX, nextY].wallBottom) break;
                    }
                    else break;
                }

                break;

            case SwipeDetector.SwipeDirection.Up:
                for (var y = _currentCell.y; y < lenX - 1; y++)
                {
                    if (!_maze[nextX, y + 1].wallBottom)
                    {
                        nextY = y + 1;
                        steps++;

                        if (IsEnd(ref isExit, nextX, nextY)) break;
                        if (nextX < lenX - 1 && !_maze[nextX + 1, nextY].wallLeft) break;
                        if (!_maze[nextX, nextY].wallLeft) break;
                    }
                    else break;
                }

                break;
            case SwipeDetector.SwipeDirection.Down:
                for (var y = _currentCell.y; y > 0; y--)
                {
                    if (!_maze[nextX, y].wallBottom)
                    {
                        nextY = y - 1;
                        steps++;

                        if (IsEnd(ref isExit, nextX, nextY)) break;
                        if (nextX < lenX - 1 && !_maze[nextX + 1, nextY].wallLeft) break;
                        if (!_maze[nextX, nextY].wallLeft) break;
                    }
                    else break;
                }

                break;
        }

        if (steps == 0) return;

        _isMoving = true;

        var toPos = _mazeSpawner.GetInCellCoordinates(nextX, nextY);

        // Debug.Log($"------------");
        // Debug.Log($"curXY[{_currentCell}], coord from { _mazeSpawner.GetInCellCoordinates(_currentCell)};");
        // Debug.Log($"nextXY[{nextX};{nextY}], coord to {toPos};");

        _player.transform.DOMove(toPos, steps * _gameConfig.BallSpeed).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                _isMoving = false;
                _currentCell.x = nextX;
                _currentCell.y = nextY;
                if (isExit) _onReachEnd?.Execute();
            });
    }

    private bool IsEnd(ref bool exit, int x, int y)
    {
        if (_gameConfig.EndPoint.x == x && _gameConfig.EndPoint.y == y)
        {
            exit = true;
            return true;
        }

        exit = false;
        return false;
    }
}