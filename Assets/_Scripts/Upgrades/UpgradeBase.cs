using UnityEngine;

public abstract class UpgradeBase : MonoBehaviour
{
    [SerializeField] protected int _maxLevels;
    protected string _levels;
    [SerializeField] protected int _currentLevel;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected string _description;

    public string Title => $"Levels {_currentLevel}/{_maxLevels}";
    public Sprite Icon => _icon;
    public string Description  => _description;

    public virtual void OnlevelUp()
    {
        _currentLevel++;
        Upgrade();
        if(_currentLevel >= _maxLevels)
        {
            int position = UpgradeManager.Instance.Upgrades.IndexOf(this);
            UpgradeManager.Instance.Upgrades.RemoveAt(position);
            Destroy(gameObject);
        }
    }

    protected abstract void Upgrade();
}

