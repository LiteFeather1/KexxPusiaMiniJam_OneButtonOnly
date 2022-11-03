using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct UpgradeSpot
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _description;

    public void SetUp(string levels, Sprite image, string description)
    {
        _title.text = levels ;
        _image.sprite = image;
        _description.text = description;
    }

    public void Disable()
    {
        GameObject spot = _title.transform.parent.gameObject;
        spot.SetActive(false);
    }
}
