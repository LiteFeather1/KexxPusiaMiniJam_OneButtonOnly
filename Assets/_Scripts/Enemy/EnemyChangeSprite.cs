using UnityEngine;

public class EnemyChangeSprite : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite[] _sprites;

    private void Start()
    {
        _sr.sprite = _sprites[_enemy.HP - 1];
    }
}
