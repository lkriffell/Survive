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
      if (_laborer.Target != null)
      {
        _laborer.TakeFromTarget();
      }
    }

    public void OnExit() 
    {
    }
}