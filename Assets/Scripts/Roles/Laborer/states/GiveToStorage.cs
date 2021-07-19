using UnityEngine;

public class GiveToStorage : IState
{
    private readonly Laborer _laborer;

    private string _mostResource;

    public GiveToStorage(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
      _laborer.SetMostResource();
    }
    public void Tick() 
    { 
      if (_laborer.StorageTarget != null)
      {
        _laborer.GiveToStorage(_laborer._resourceToDeliver);
      }
    }

    public void OnExit() 
    {
      _laborer.giveToStorage = false;
      _laborer.SetMostResource();
    }
}