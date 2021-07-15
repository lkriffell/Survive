using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    private Grid grid;

    void Awake()
    {
      grid = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            PlaceObjectNear(hitInfo.point);
        }
      }
    }

    private void PlaceObjectNear(Vector3 clickPoint)
    {
      var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
      Instantiate(objectToPlace, finalPosition, Quaternion.identity);
    }
}
