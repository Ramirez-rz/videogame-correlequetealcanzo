using UnityEngine;

public class SimuladorMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}