using UnityEngine;

public class GiveToStorage : IState
{
    private readonly Citizen _citizen;

    private string _mostResource;

    public GiveToStorage(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
      _citizen.SetMostResource();
    }
    public void Tick() 
    { 
      if (_citizen.StorageTarget != null)
      {
        _citizen.GiveToStorage(_citizen._resourceToDeliver);
      }
    }

    public void OnExit() 
    {
      _citizen.giveToStorage = false;
      _citizen.SetMostResource();
    }
}