using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float speed = 4f;

    public Transform enemy;

    public Animator animator;

    private bool attacking = false;
    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.SetBool("Walking", true);
    }

    void Update()
    {   
        if ((player.transform.position-enemy.transform.position).sqrMagnitude<3*3) 
        {
          if (attacking) 
          {
            attacking = false;
            animator.SetBool("Attacking", false);
          } else
          {
            animator.SetBool("Walking", false);
            animator.SetBool("Attacking", true);
            attacking = true;
          }
        } else
        {
          animator.SetBool("Walking", true);
          animator.SetBool("Attacking", false);
          attacking = false;
          float target;
          if (player.position.x < 0)
          {
            target = player.position.x + 3;
          } else
          {
            target = player.position.x - 3;
          }
          Vector3 targetPostition = new Vector3( target, enemy.transform.position.y, player.position.z );
          enemy.transform.LookAt( targetPostition );
          nav.SetDestination(player.position);
        }
    }
}
