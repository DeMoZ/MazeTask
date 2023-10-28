using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // scriptables
        Container.Bind<GameConfig>().FromInstance(Resources.Load<GameConfig>("GameConfig"));
        
        // prefabs
        Container.Bind<SwipeDetector>().FromComponentInNewPrefabResource("SwipeDetector").AsSingle().NonLazy();
        
        Container.Bind<GameRoot>().AsSingle().NonLazy();

        Container.Bind<GameService>().AsSingle();
        Container.Bind<MazeGenerator>().AsSingle();
        Container.Bind<MazeSpawner>().AsSingle();
        Container.Bind<PlayerController>().AsSingle();
    }
}
