using UnityEngine;

public class TakeFromStorage : IState
{
    private readonly Laborer _laborer;

    private string _mostResource;

    public TakeFromStorage(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
    }
    public void Tick() 
    { 
      if (_laborer.StorageTarget != null)
      {
        _laborer.TakeFromStorage();
      }
    }

    public void OnExit() 
    {
      _laborer.takeFromStorage = false;
    }
}