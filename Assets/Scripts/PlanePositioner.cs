using UnityEngine;

public class PlanePositioner : MonoBehaviour {
  public Transform LowerLeft;
  public Transform LowerRight;
  public Transform TopEdge;
  public Transform Plane;
  // Use this for initialization
  void Update () {
    //Up an Down Centric Placer
    Plane.position = (LowerLeft.position + LowerRight.position) / 2f;
    Vector3 upfromside = -Vector3.Cross(LowerLeft.position - LowerRight.position,Vector3.Cross(LowerLeft.position - TopEdge.position, LowerLeft.position - LowerRight.position));
    Plane.rotation = Quaternion.LookRotation(upfromside, -Vector3.Cross(LowerLeft.position - LowerRight.position,LowerLeft.position - TopEdge.position));
    float distance = Vector3.Distance(TopEdge.position, Vector3.Project(TopEdge.position - LowerLeft.position, LowerRight.position - LowerLeft.position)+ LowerLeft.position);
    Plane.localScale = new Vector3(Vector3.Distance(LowerLeft.position, LowerRight.position), 1f, distance);
  }
}
