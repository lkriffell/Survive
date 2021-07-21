using UnityEngine;
using UnityEngine.AI;

public class GoToStorage : IState
{
    private readonly Citizen _citizen;
    private readonly NavMeshAgent _navMeshAgent;

    public float TimeStuck;
    private Vector3 _lastPosition;

    public GoToStorage(Citizen citizen, NavMeshAgent navMeshAgent) 
    {
      _citizen = citizen;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      TimeStuck = 0f;
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_citizen.StorageTarget.transform.position);
  }

    public void Tick() 
    { 
      if (Vector3.Distance(_citizen.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _citizen.transform.position;
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}