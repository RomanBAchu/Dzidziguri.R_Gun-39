using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BattleController>().AsSingle();
        Container.Bind<PlayerController>().AsSingle();
        Container.Bind<Battlefield>().AsSingle();
        Container.Bind<InputHandler>().AsSingle();
    }
}
