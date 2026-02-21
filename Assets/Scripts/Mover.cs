using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 _start = Vector3.zero;
    [SerializeField] private Vector3 _end = new Vector3(5f, 0f, 0f);
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _delay = 1f;

    private Rigidbody _rb;
    private Vector3 _globalStart;
    private Vector3 _globalEnd;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("На объекте отсутствует Rigidbody!");
            return;
        }

        UpdateGlobalPositions();
        StartCoroutine(MoveCycle());
    }

    private void UpdateGlobalPositions()
    {
        _globalStart = transform.position + _start;
        _globalEnd = transform.position + _end;
    }

    private IEnumerator MoveCycle()
    {
        while (true)
        {
            yield return MoveToPoint(_globalEnd);
            yield return new WaitForSeconds(_delay);
            yield return MoveToPoint(_globalStart);
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        Vector3 startPosition = _rb.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / _speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            // Равномерное движение без ускорений
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            _rb.MovePosition(newPosition);
            yield return null;
        }

        // Гарантируем точную конечную позицию
        _rb.MovePosition(targetPosition);
    }

    private void OnDrawGizmos()
    {
        UpdateGlobalPositions();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_globalStart, 0.3f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_globalEnd, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_globalStart, _globalEnd);

        
    }
}
