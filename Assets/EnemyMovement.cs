using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float speed = 4f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
        transform.LookAt(player);
        rb.MovePosition(pos);

    }
}
