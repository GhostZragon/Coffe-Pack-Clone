using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public RawImage img; // RawImage cần di chuyển
    [SerializeField] private float speed = 0.2f; // Tốc độ di chuyển
    [SerializeField] private Vector2 direction; // Hướng di chuyển
    private float timer; // Bộ đếm thời gian

    void Start()
    {
        ChangeDirection(); // Chọn hướng ban đầu
    }

    void Update()
    {
        // Di chuyển ảnh theo hướng hiện tại
        img.uvRect = new Rect(img.uvRect.position + direction * (speed * Time.deltaTime), img.uvRect.size);

        // Đếm thời gian và đổi hướng nếu cần
        
    }

    void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}