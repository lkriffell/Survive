using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    private Mesh objectToPlaceMesh;
    public Material objectPreviewMat;
    private Grid grid;

    void Awake()
    {
      grid = FindObjectOfType<Grid>();
      objectToPlaceMesh = objectToPlace.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
      RaycastHit hitInfo;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hitInfo)) Graphics.DrawMesh(objectToPlaceMesh, hitInfo.point, Quaternion.identity, objectPreviewMat, 0);

      if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitInfo))
      {
        PlaceObjectNear(hitInfo.point);
      }
    }

    private void PlaceObjectNear(Vector3 clickPoint)
    {
      var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
      finalPosition.y += 1.8f;
      Instantiate(objectToPlace, finalPosition, Quaternion.identity);
    }
}
