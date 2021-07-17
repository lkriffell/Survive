using UnityEngine;
using UnityEngine.AI;

public class GoToMarkedResource : IState
{
    private readonly Laborer _laborer;
    private readonly NavMeshAgent _navMeshAgent;

    public GoToMarkedResource(Laborer laborer, NavMeshAgent navMeshAgent) 
    {
      _laborer = laborer;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_laborer.Target.transform.position);
    }
    public void Tick() 
    { 
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}