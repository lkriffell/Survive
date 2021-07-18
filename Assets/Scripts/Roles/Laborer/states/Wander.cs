using UnityEngine;
using UnityEngine.AI;

public class Wander : IState
{
    private readonly Laborer _laborer;
    private readonly NavMeshAgent _navMeshAgent;

    private Vector3 walkPoint;
    public float TimeStuck;
    private Vector3 _lastPosition;

    private float wanderDistance = 20f;

    public Wander(Laborer laborer, NavMeshAgent navMeshAgent) 
    {
      _laborer = laborer;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    { 
      TimeStuck = 0f;
      _navMeshAgent.enabled = true;
      walkPoint = _laborer.transform.position;
    }
    public void Tick() 
    { 
      if (Vector3.Distance(_laborer.transform.position, walkPoint) < 1f)
      {
        SearchWalkPoint();
        _navMeshAgent.SetDestination(walkPoint);
      }

      if (Vector3.Distance(_laborer.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _laborer.transform.position;
    }

    public void OnExit() 
    { 
      _navMeshAgent.enabled = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-wanderDistance, wanderDistance);
        float randomX = Random.Range(-wanderDistance, wanderDistance);

        walkPoint = new Vector3(_laborer.transform.position.x + randomX, _laborer.transform.position.y, _laborer.transform.position.z + randomZ);          
    }
}