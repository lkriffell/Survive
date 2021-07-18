using UnityEngine;
using UnityEngine.AI;

public class GoToStorage : IState
{
    private readonly Laborer _laborer;
    private readonly NavMeshAgent _navMeshAgent;

    public float TimeStuck;
    private Vector3 _lastPosition;

    public GoToStorage(Laborer laborer, NavMeshAgent navMeshAgent) 
    {
      _laborer = laborer;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      TimeStuck = 0f;
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_laborer.StorageTarget.transform.position);
  }

    public void Tick() 
    { 
      // if (Vector3.Distance(_laborer.StorageTarget.transform.position, _laborer.transform.position) < 3f) _navMeshAgent.SetDestination(_laborer.transform.position);
      if (Vector3.Distance(_laborer.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _laborer.transform.position;
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}