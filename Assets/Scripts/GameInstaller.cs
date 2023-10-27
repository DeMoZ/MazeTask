using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameRoot>().AsSingle().NonLazy();

        Container.Bind<GameService>().AsSingle();//.NonLazy()*/;
        Container.Bind<MazeGenerator>().AsSingle();//.NonLazy()*/;
        Container.Bind<MazeSpawner>().AsSingle();//.NonLazy()*/;
        Container.Bind<PlayerController>().AsSingle();//.NonLazy()*/;
    }
}
