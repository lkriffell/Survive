using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Builder : Citizen
{
    private StateMachine _stateMachine;
    
    public Citizen Citizen;

    void Awake () {
      var navMeshAgent = Citizen.GetComponent<NavMeshAgent>();

      _stateMachine = new StateMachine();

      var Wander = new Wander(Citizen, navMeshAgent);

      // Storage 
      var FindStorage = new FindStorage(Citizen);
      var GoToStorage = new GoToStorage(Citizen, navMeshAgent);
      var GiveToStorage = new GiveToStorage(Citizen);

      // BuildSite
      var FindBuildSite = new FindBuildSite(Citizen);
      var FindStorageToTakeFrom = new FindStorageToTakeFrom(Citizen);
      var TakeFromStorage = new TakeFromStorage(Citizen);
      var GoToBuildSite = new GoToBuildSite(Citizen, navMeshAgent);
      var GiveToBuildSite = new GiveToBuildSite(Citizen);

      // Wander transitions
      
      // Storage

      _stateMachine.SetState(FindBuildSite);

      void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
    }

    void FixedUpdate () 
    {
      _stateMachine.Tick();
    }
}
