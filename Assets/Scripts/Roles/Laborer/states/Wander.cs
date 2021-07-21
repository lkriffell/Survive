using UnityEngine;
using UnityEngine.AI;

public class Wander : IState
{
    private readonly Citizen _citizen;
    private readonly NavMeshAgent _navMeshAgent;

    private Vector3 walkPoint;
    public float TimeStuck;

    private Vector3 _lastPosition;

    private float wanderDistance = 20f;

    public Wander(Citizen citizen, NavMeshAgent navMeshAgent) 
    {
      _citizen = citizen;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    { 
      TimeStuck = 0f;
      _navMeshAgent.enabled = true;
      walkPoint = _citizen.transform.position;
    }
    public void Tick() 
    { 
      if (Vector3.Distance(_citizen.transform.position, walkPoint) < 1f)
      {
        SearchWalkPoint();
        _navMeshAgent.SetDestination(walkPoint);
      }

      if (Vector3.Distance(_citizen.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _citizen.transform.position;
    }

    public void OnExit() 
    { 
      _navMeshAgent.enabled = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-wanderDistance, wanderDistance);
        float randomX = Random.Range(-wanderDistance, wanderDistance);

        walkPoint = new Vector3(_citizen.transform.position.x + randomX, _citizen.transform.position.y, _citizen.transform.position.z + randomZ);          
    }
}