using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameContext _gameContext;
    
    public override void InstallBindings()
    { 
        Container.BindInstance(_gameContext);
    }
}