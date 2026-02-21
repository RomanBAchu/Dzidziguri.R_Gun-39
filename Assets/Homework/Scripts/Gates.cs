using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField] private int _score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _score++;
            Debug.Log($"Текущий счёт: {_score}");
            Destroy(other.gameObject);
        }
    }
}
