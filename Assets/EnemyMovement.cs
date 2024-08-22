using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Скорость движения врага вниз
    public float swayAmplitude = 0.5f; // Амплитуда покачивания влево-вправо
    public float swayFrequency = 1f; // Частота покачивания

    private Vector3 startPosition;

    void Start()
    {
        // Запоминаем начальную позицию врага
        startPosition = transform.position;
    }

    void Update()
    {
        // Двигаем врага вниз
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Реализуем покачивание по оси X
        float swayOffset = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        transform.position = new Vector3(startPosition.x + swayOffset, transform.position.y, transform.position.z);

        // Проверяем, если враг достиг финишной линии
        if (transform.position.y <= GameObject.FindGameObjectWithTag("FinishLine").transform.position.y)
        {
            // Если враг пересек линию
            FindObjectOfType<EnemySpawner>().OnEnemyCrossedFinishLine(transform.position);
            Destroy(gameObject); // Удаляем врага
        }
    }
}
