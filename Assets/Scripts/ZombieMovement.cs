using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float moveSpeed = 5.2f;

    void Update()
    {
        // Movimiento constante.
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
