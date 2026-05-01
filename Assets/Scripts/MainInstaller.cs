using Zenject;
using UnityEngine;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneController>().AsSingle();
    }
}
