using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private SwipeDetector _swipeDetector;
    
    public override void InstallBindings()
    {
        Container.Bind<GameContext>().FromInstance(_gameContext);
        Container.Bind<SwipeDetector>().FromInstance(_swipeDetector);
        //Container.BindInstance(gameConfig).AsSingle().NonLazy();
        //Container.BindInstance(gameContext);
        //Container.Bind<GameRoot>().FromInstance(new GameRoot()).AsSingle().NonLazy();
    }
}