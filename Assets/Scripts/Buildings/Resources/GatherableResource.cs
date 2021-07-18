using System;
using UnityEngine;

public class GatherableResource : MonoBehaviour
{
    public int takeable = 10;
    public bool markedForPickup;
    public bool pickedUp;

    public String resourceType;

    void Start () 
    {
      
    }

    void Update ()
    {
      if (pickedUp == true) gameObject.SetActive(false);
    }

    public bool Take()
    {
      if (takeable <= 0) 
      {
        pickedUp = true;
        return false;
      }
      takeable--;
      return true;
    }
}
