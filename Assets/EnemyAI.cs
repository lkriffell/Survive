using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float speed;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {   
      playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
      playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
      if (!playerInSightRange && !playerInAttackRange) Patrolling();
      if (playerInSightRange && !playerInAttackRange) ChasePlayer();
      if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, - transform.up, 2f, whatIsGround)) 
            walkPointSet = true;
    }

    private void Patrolling()
    {
      if (!walkPointSet) SearchWalkPoint();
      if (walkPointSet)
      {
        agent.SetDestination(walkPoint);
        animator.SetBool("Attacking", false);
        animator.SetBool("Walking", true);  
      } 
        

      Vector3 distanceToWalkPoint = transform.position - walkPoint;

      if (distanceToWalkPoint.magnitude < 1f)
        walkPointSet = false;
    }

    private void ChasePlayer()
    {
      animator.SetBool("Attacking", false);
      animator.SetBool("Walking", true);
      agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    { 
      agent.SetDestination(transform.position);
      if (!alreadyAttacked)
      {
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", true);
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        Invoke(nameof(ResetAnimator), 1f);
      } 
    }

    private void ResetAnimator()
    {
      animator.SetBool("Walking", false);
      animator.SetBool("Attacking", false);
    }

    private void ResetAttack()
    {
      alreadyAttacked = false;
    }
}
