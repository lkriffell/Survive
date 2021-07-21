using UnityEngine;
using UnityEngine.AI;

public class GoToBuildSite : IState
{
    private readonly Citizen _citizen;
    private readonly NavMeshAgent _navMeshAgent;

    public float TimeStuck;
    private Vector3 _lastPosition;

    public GoToBuildSite(Citizen citizen, NavMeshAgent navMeshAgent) 
    {
      _citizen = citizen;
      _navMeshAgent = navMeshAgent;
    }
    public void OnEnter() 
    {
      _navMeshAgent.enabled = true;
      _navMeshAgent.SetDestination(_citizen.BuildSiteTarget.transform.position);
  }

    public void Tick() 
    { 
    }

    public void OnExit() 
    {
      _navMeshAgent.enabled = false; 
    }
}