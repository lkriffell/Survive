using UnityEngine;
using UnityEngine.AI;

public class GoToMarkedResource : IState
{
    private readonly Laborer _laborer;
    private readonly NavMeshAgent _navMeshAgent;

    public float TimeStuck;
    private Vector3 _lastPosition;

    public GoToMarkedResource(Laborer laborer, NavMeshAgent navMeshAgent) 
    {
      TimeStuck = 0f;
      _laborer = laborer;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_laborer.ResourceTarget.transform.position);
    }

    public void Tick() 
    { 
      // if (Vector3.Distance(_laborer.ResourceTarget.transform.position, _laborer.transform.position) < 2f) _navMeshAgent.SetDestination(_laborer.transform.position);
      if (Vector3.Distance(_laborer.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _laborer.transform.position;
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}