public class UpgradeRestoreHealth : UpgradeBase
{
    protected override void Upgrade()
    {
        PlayerShip.Instance.RestoreHealth();
    }
}
