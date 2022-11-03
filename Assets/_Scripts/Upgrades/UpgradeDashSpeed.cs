public class UpgradeDashSpeed : UpgradeBase
{

    protected override void Upgrade()
    {
        PlayerShip.Instance.UpgradeDashSpeed();
    }
}
