using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _speed;
    [SerializeField] private int _hp;
    [SerializeField] private int _damage;
    [SerializeField] private int _scoreToAdd;

    private Vector3 _direction;
    public int HP { get => _hp; }
    public Vector3 WhereToMove { get => _direction; set => _direction = value; }

    private void Update()
    {
        Moviment();
        ReachedDestination();
    }

    private void Moviment()
    {
        transform.position = Vector2.MoveTowards(transform.position, WhereToMove, _speed * Time.deltaTime);
    }

    private void ReachedDestination()
    {
        if (transform.position == WhereToMove)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if(damage > 0)
        {
            _hp -= damage;
            SoundManager.Instance.PlayDamages();
            GameManager.Instance.AddToScore(_scoreToAdd);
            if (_hp <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        SoundManager.Instance.PlayEnemyKill();
        GameManager.Instance.AddToScore(_scoreToAdd);
        Destroy(gameObject);
    }

    public void SetHPSpeedDamage(int hp, float speed, int damage)
    {
        _hp = hp;
        _speed = speed;
        _damage = damage;
    }

    IEnumerator Squeeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Squeeze(.8f, .8f, .25f));
        if(collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            if (_damage > 0)
                damageable.TakeDamage(_damage);
        }
    }
}
