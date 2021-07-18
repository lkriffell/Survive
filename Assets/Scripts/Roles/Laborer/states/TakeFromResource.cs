using UnityEngine;

public class TakeFromResource : IState
{
    private readonly Laborer _laborer;

    public TakeFromResource(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {

    }
    public void Tick() 
    { 
      if (_laborer.ResourceTarget != null)
      {
        _laborer.TakeFromResource();
        _laborer.SetMostResource();
      }
    }

    public void OnExit() 
    {
      _laborer.SetMostResource();
      _laborer.ResourceTarget = null;
    }
}