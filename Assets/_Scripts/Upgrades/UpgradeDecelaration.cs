public class UpgradeDecelaration : UpgradeBase
{
    protected override void Upgrade()
    {
        PlayerShip.Instance.UpgradeDashDecelaration();
    }
}
