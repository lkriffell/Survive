using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Laborer : MonoBehaviour
{
    public Rigidbody rb;
    private StateMachine _stateMachine;

    private int maxCarry = 20;
    public Dictionary<String, int> inventory = new Dictionary<String, int>();
    public float lastSearch;
    public GatherableResource Target;

    void Start () {
      var navMeshAgent = GetComponent<NavMeshAgent>();

      _stateMachine = new StateMachine();

      var Wander = new Wander(this, navMeshAgent);
      var GoToMarkedResource = new GoToMarkedResource(this, navMeshAgent);
      var FindMarkedResource = new FindMarkedResource(this);
      var TakeFromResource = new TakeFromResource(this);

      At(GoToMarkedResource, FindMarkedResource, MyResourcePickedUp());
      At(GoToMarkedResource, TakeFromResource, NextToTarget());
      At(FindMarkedResource, GoToMarkedResource, HasTarget());
      At(TakeFromResource, FindMarkedResource, InventoryNotFullAndMyTargetIsGone());
      // At(GoToStockpile, TakeFromResource, InventoryFull());
      At(FindMarkedResource, Wander, NoTargetFound());

      _stateMachine.SetState(FindMarkedResource);

      void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

      Func<bool> HasTarget() => () => Target != null;
      Func<bool> NextToTarget() => () => Target != null && Vector3.Distance(transform.position, Target.transform.position) < 2f;
      Func<bool> MyResourcePickedUp() => () => Target != null && Target.pickedUp;
      Func<bool> InventoryNotFullAndMyTargetIsGone() => () => Target == null && !InventoryFull();
      Func<bool> NoTargetFound() => () => Target == null && lastSearch <= 30f;
    }

    void FixedUpdate () 
    {
      lastSearch += Time.deltaTime;
      _stateMachine.Tick();
    }

    public void TakeFromTarget()
    {
      if (!InventoryFull())
      {
        if (Target.Take())
        {
          if (inventory.ContainsKey(Target.resourceType)) inventory[Target.resourceType]++;
          else inventory.Add(Target.resourceType, 1);
        }
        else Target = null;
      }
    }

    public int CarryingNow()
    {
      int carrying = 0;
      foreach(int amount in inventory.Values)
      {
        carrying += amount;
      }
      return carrying;
    }
    public bool InventoryFull()
    {
      if (CarryingNow() >= maxCarry) return true;
      else return false;
    }
}
