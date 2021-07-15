using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    public string targetTag;
    public GameObject target;
    public Transform targetTransform;
    public Target targetScript;
    private Animator animator;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsTarget;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float speed;
    public float damage;
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool targetInSightRange, targetInAttackRange;

    private AudioSource audio;
    public AudioClip[] audioClipArray;

    public Gun gun;

    // Start is called before the first frame update
    void Start()
    {     
      gun = GetComponentInChildren<Gun>();
      audio = GetComponent<AudioSource>();
      animator = GetComponent<Animator>();
      agent = GetComponent<NavMeshAgent>();
      Patrolling();
    }

    void FixedUpdate()
    { 
      // targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsTarget);
      FindNewTarget();
      targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsTarget);
      if (target == null || targetScript.dead) 
      {
        Patrolling();
        FindNewTarget();
      }
      if (targetInAttackRange) AttackTarget();
      if (!targetInAttackRange) ChaseTarget();
    }

    private void ResetShooting()
    {
      gun.shooting = false;
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
        Invoke(nameof(ResetWalkPointSet), 10f);
      } 
      Invoke(nameof(FindNewTarget), 5f);

      Vector3 distanceToWalkPoint = transform.position - walkPoint;

      if (distanceToWalkPoint.magnitude < 1f)
        walkPointSet = false;
    }

    private void ChaseTarget()
    {
      animator.SetBool("Attacking", false);
      animator.SetBool("Walking", true);
      agent.SetDestination(targetTransform.position);
      // FindNewTarget();
    }

    private void AttackTarget()
    { 
      FindNewTarget();
      agent.SetDestination(transform.position);
      if (!alreadyAttacked)
      {
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", true);
        Invoke(nameof(ResetAnimator), 1f);
        transform.LookAt(targetTransform, Vector3.up);
        if (gun != null)
        {
          gun.shooting = true;
          if (gun.allowButtonHold) Invoke(nameof(ResetShooting), 2f);
          else Invoke(nameof(ResetShooting), 0.1f);
        } else
        {
          HitDetection();
          audio.clip = audioClipArray[Random.Range(0, audioClipArray.Length - 1)];
          audio.PlayOneShot(audio.clip);
        }
        
      } 
    }

    private void HitDetection()
    {
      Vector3 noAngle = transform.forward;
      Quaternion spreadAngle = Quaternion.AngleAxis(-15, new Vector3(1, 1, 0));
      Vector3 newVector = spreadAngle * noAngle;
      var ray = new Ray(transform.position, newVector);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, attackRange))
      {
        if (hit.transform.gameObject.GetInstanceID() == target.GetInstanceID())
        {
          targetScript.TakeDamage(damage);
        }
      }
    }

    private void FindNewTarget()
    {
      GetClosestEnemy(GameObject.FindGameObjectsWithTag(targetTag));
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

    private void ResetWalkPointSet()
    {
      walkPointSet = false;
    }

    private void GetClosestEnemy(GameObject[] enemies)
    {
      GameObject closestEnemy = null;
      float minDist = Mathf.Infinity;
      Vector3 currentPos = transform.position;
      if (enemies.Length <= 0) Patrolling();
      foreach (GameObject enemy in enemies)
      {
        float dist = Vector3.Distance(enemy.GetComponent<Transform>().root.position, currentPos);
        if (dist < minDist)
        {
          if (enemy.name != "Target" && enemy.name != transform.name)
          {
            closestEnemy = enemy;
            minDist = dist;
          }
        }
      }
      target = closestEnemy;
      targetTransform = target.GetComponent<Transform>();
      targetScript = target.GetComponent<Target>();
    }
}
