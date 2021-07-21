using UnityEngine;

public class TakeFromStorage : IState
{
    private readonly Citizen _citizen;

    private string _mostResource;

    public TakeFromStorage(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
    }
    public void Tick() 
    { 
      if (_citizen.StorageTarget != null)
      {
        _citizen.TakeFromStorage();
      }
    }

    public void OnExit() 
    {
      _citizen.takeFromStorage = false;
    }
}