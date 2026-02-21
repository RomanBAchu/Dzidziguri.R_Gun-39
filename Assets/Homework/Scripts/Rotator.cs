using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationSpeed = new Vector3(0f, 10f, 0f);
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("На объекте отсутствует Rigidbody!");
            return;
        }
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        yield return new WaitForFixedUpdate(); // синхронизация с физическим циклом

        while (true)
        {
            // Вращение через Rigidbody.MoveRotation
            Quaternion rotation = Quaternion.Euler(_rotationSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(_rb.rotation * rotation);
            yield return new WaitForFixedUpdate();
        }
    }
}
