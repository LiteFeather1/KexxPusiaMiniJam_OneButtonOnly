public class UpgradeDashCooldown : UpgradeBase
{
    protected override void Upgrade()
    {
        PlayerShip.Instance.UpgradeDashCoolDown();
    }
}
