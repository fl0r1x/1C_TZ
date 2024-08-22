using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpawnInterval = 0.2f;  // Интервал между выстрелами
    public float maxYPosition = 0f;  // Максимальная позиция по оси Y, выше которой игрок не может подняться

    private Vector2 screenBounds;
    private float bulletTimer;

    void Start()
    {
        // Получаем границы экрана
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        }
        else
        {
            Debug.LogError("Main camera is not assigned.");
        }
    }

    void Update()
    {
        // Движение игрока
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX, moveY).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime);

        // Ограничение передвижения игрока по экрану
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x + 0.5f, screenBounds.x - 0.5f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y + 0.5f, maxYPosition); // Ограничение по Y
        transform.position = clampedPosition;

        // Спавн пуль через определенные интервалы времени
        bulletTimer += Time.deltaTime;
        if (bulletTimer >= bulletSpawnInterval)
        {
            Shoot();
            bulletTimer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Bullet prefab or spawn point not assigned.");
        }
    }
}

