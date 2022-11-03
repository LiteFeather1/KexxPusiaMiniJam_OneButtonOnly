public class UpgradeDamage : UpgradeBase
{
    protected override void Upgrade()
    {
        PlayerShip.Instance.UpgradeDamage();
    }
}
