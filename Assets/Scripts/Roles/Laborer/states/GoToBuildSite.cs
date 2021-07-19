using UnityEngine;
using UnityEngine.AI;

public class GoToBuildSite : IState
{
    private readonly Laborer _laborer;
    private readonly NavMeshAgent _navMeshAgent;

    public float TimeStuck;
    private Vector3 _lastPosition;

    public GoToBuildSite(Laborer laborer, NavMeshAgent navMeshAgent) 
    {
      _laborer = laborer;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_laborer.BuildSiteTarget.transform.position);
  }

    public void Tick() 
    { 
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}