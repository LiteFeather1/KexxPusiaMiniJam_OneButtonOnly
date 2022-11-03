public class UpgradeMaxSpeed : UpgradeBase
{
    protected override void Upgrade()
    {
        PlayerShip.Instance.UpgradeMaxSpeed();
    }
}
