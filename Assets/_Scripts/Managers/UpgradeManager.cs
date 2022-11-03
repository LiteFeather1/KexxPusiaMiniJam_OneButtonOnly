using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject _upgradesScreen;
    [SerializeField] private Button _rerollButton;
    [SerializeField] private List<UpgradeBase> _upgrades;

    private List<UpgradeBase> _currentPool;
    [SerializeField] private Button[] _buttons;

    public int _spotsToShow;

    [SerializeField] private UpgradeSpot[] _spots;

    private int _currentSelectButton;
    private float _passingTimeForBuy;
    [SerializeField] private Color32 _selectedColor = new(255, 184, 112, 255);
    [SerializeField] private Color32 _pressedColor = new(255, 184, 112, 255);

    [SerializeField] private VolumeManager _volumeManager;

    private static UpgradeManager _instance;

    public static UpgradeManager Instance { get => _instance; }
    public List<UpgradeBase> Upgrades { get => _upgrades; }

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
    }

    private void Start()
    {
         InitCurrentPool();
    }

    private void Update()
    {
        HandleButtons();
        _spotsToShow = SpotsToShow();
    }

    private void InitCurrentPool()
    {
        _currentPool = new List<UpgradeBase>(_upgrades.Count);
        for (int i = 0; i < _upgrades.Count; i++)
        {
            _currentPool.Add(_upgrades[i]);
        }
    }

    private void PickUpgrades()
    {
        for (int i = 0; i < SpotsToShow(); i++)
        {
            int randomNum = Random.Range(0, _currentPool.Count);
            if (_currentPool[randomNum] != null)
                ChangeButtonAction(i, randomNum);
        }
    }

    private void ChangeButtonAction(int button, int upgrade)
    {
        _buttons[button].onClick.RemoveAllListeners();
        _buttons[button].onClick.AddListener(_currentPool[upgrade].OnlevelUp);
        _buttons[button].onClick.AddListener(UnshowUpgradeScreen);
        _buttons[button].onClick.AddListener(InitCurrentPool);

        string title = _currentPool[upgrade].Title;
        Sprite sprite = _currentPool[upgrade].Icon;
        string description = _currentPool[upgrade].Description;
        _spots[button].SetUp(title, sprite, description);

        _currentPool.RemoveAt(upgrade);
    }

    private void HandleButtons()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _passingTimeForBuy += Time.unscaledDeltaTime;

            Color newColor = Color.Lerp(_selectedColor, _pressedColor, _passingTimeForBuy);
            ColorBlock cb = _buttons[_currentSelectButton].colors;
            cb.selectedColor = newColor;
            _buttons[_currentSelectButton].colors = cb;
            if (_passingTimeForBuy >= 1)
            {
                _passingTimeForBuy = 0;
                _buttons[_currentSelectButton].onClick?.Invoke();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if(_passingTimeForBuy <= 0.25f)
                _currentSelectButton++;
            if (_currentSelectButton > SpotsToShow() - 1)
            {
                _currentSelectButton = 0;
            }
            ColorBlock cb = _buttons[_currentSelectButton].colors;
            cb.selectedColor = _selectedColor;
            _buttons[_currentSelectButton].colors = cb;
            _buttons[_currentSelectButton].Select();
            _passingTimeForBuy = 0;
        }
    }

    private int SpotsToShow()
    {
        if (_upgrades.Count > 3)
            return 3;
        else
            return
                _upgrades.Count;
    }

    private void DisableOverSpot()
    {
        if (SpotsToShow() > 3)
            return;
        else if (SpotsToShow() < 3)
        {
            _spots[2].Disable();
        }
        else if (SpotsToShow() < 2)
        {
            _spots[1].Disable();
        }
    }

    public void ButtonRerollUpgrade()
    {
        PickUpgrades();
        _rerollButton.interactable = false;
    }

    public void ShowUpgradeScreen()
    {
        PickUpgrades();
        _buttons[_currentSelectButton].Select();
        _rerollButton.interactable = true;
        _upgradesScreen.SetActive(true);
        DisableOverSpot();
        Time.timeScale = 0;
        _volumeManager.ActivateVignette();
        if (SpotsToShow() == 0)
            UnshowUpgradeScreen();
    }

    public void UnshowUpgradeScreen()
    {
        _upgradesScreen.SetActive(false);
        _currentSelectButton = 0;
        Time.timeScale = 1;
        _volumeManager.DeactivateVignette();
    }
}