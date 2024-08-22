using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f; // Скорость пули
    public float maxYPosition = 10f; // Позиция по оси Y, после которой пуля уничтожается
    public int damage = 1; // Урон, который наносит пуля

    void Start()
    {
        // Устанавливаем скорость пули
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * bulletSpeed;
        }
    }

    void Update()
    {
        // Удаление пули, если она выходит за пределы экрана
        if (transform.position.y > maxYPosition)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка на столкновение с врагом
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Передаем урон врагу
                Destroy(gameObject); // Удаляем пулю после попадания
            }
        }
    }
}
