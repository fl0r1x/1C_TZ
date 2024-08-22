using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public GameObject destructionEffect;
    private EnemySpawner spawner; // Ссылка на спавнер
    private float speed; // Скорость врага

    void Start()
    {
        // Если необходимо инициализировать что-то
    }

    void Update()
    {
        // Движение врага вниз по экрану
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Проверка, если враг пересек финишную линию
        if (transform.position.y <= spawner.finishLine.position.y)
        {
            spawner.OnEnemyCrossedFinishLine(transform.position);
            Destroy(gameObject); // Удаление врага
        }
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
            }
            spawner.OnEnemyDestroyed(transform.position); // Уведомляем спавнер
            Destroy(gameObject); // Удаление врага
        }
    }

    // Метод для установки спавнера
    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    // Метод для установки скорости врага
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Метод для получения скорости врага (если необходимо)
    public float GetSpeed()
    {
        return speed;
    }
}
