using UnityEngine;
using Valve.VR;
using System.Collections;

public class RoomCalibrator : MonoBehaviour {
  public Transform Lens;
  public Transform Sphere;
  public Transform Wall;
  public Transform RightWall;
  public Transform LeftWall;
  public Transform TopEdge;
  public Transform BottomLeftCorner;
  public Transform BottomRightCorner;

  public TextMesh CornerState;
  public Transform cornerTransform;
  int curCorner = 0;
  int curPoint = 0;
  Vector3[] points = new Vector3[2];
  public bool recalculateEveryFrame = true;

  void Start() {
    CornerState.text = "1) Start by touching the center of the projector lens.";
  }

  // Update is called once per frame
  void Update() {
    int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.First);
    if (deviceIndex != -1 && SteamVR_Controller.Input(deviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
      SteamVR_Controller.Input(deviceIndex).TriggerHapticPulse(10000);
      setCorner();
    }
    if (recalculateEveryFrame) {
      Sphere.GetComponent<SphereDistort>().Start();
    }
  }

  void setCorner() {
    if (curCorner == 0) {
      Lens.position = cornerTransform.position;
      Debug.Log("Lens Set!");
      CornerState.text = "2) Now touch the top edge of the projection plane.";
      curCorner++;
    } else if (curCorner == 1) {
      TopEdge.position = cornerTransform.position;
      Debug.Log("Top edge Set!");
      CornerState.text = "2) Now touch the bottom left corner of the projection plane.";
      curCorner++;
    } else if (curCorner == 2) {
      BottomLeftCorner.position = cornerTransform.position;
      Debug.Log("Bottom left corner Set!");
      CornerState.text = "3) Now touch the bottom right corner of the projection plane.";
      curCorner++;
    } else if (curCorner == 3) {
      BottomRightCorner.position = cornerTransform.position;
      Debug.Log("Bottom right corner Set!");
      CornerState.text = "3) Now touch the left side of the mirror sphere.";
      curCorner++;
    } else if (curCorner == 4) {
      Sphere.position = cornerTransform.position;
      Debug.Log("Left Sphere Set!");
      CornerState.text = "3) Now touch the right side of the mirror sphere.";
      curCorner++;
    } else if (curCorner == 5) {
      Sphere.localScale = Vector3.one * Vector3.Distance(Sphere.position, cornerTransform.position);
      Sphere.GetComponent<SphereDistort>().SphereRadiusSqrd = Mathf.Pow(Sphere.localScale.x/2f,2f);
      Sphere.position = (Sphere.position+cornerTransform.position)/2f;
      Debug.Log("Sphere Set!");
      CornerState.text = "4) Now touch the primary wall in two places left and right.";
      curCorner++;
    } else if (curCorner == 6) {
      points[curPoint] = cornerTransform.position;
      curPoint++;
      if (curPoint == points.Length) {
        curPoint = 0;
        Wall.position = (points[0] + points[1]) / 2f;
        Wall.rotation = Quaternion.LookRotation(Vector3.up, Vector3.Cross(Vector3.up, (points[1] - points[0]).normalized));
        Debug.Log("Primary Wall Set!");
        CornerState.text = "5) Now touch the right wall the same way.";
        curCorner++;
      }
    } else if (curCorner == 7) {
      points[curPoint] = cornerTransform.position;
      curPoint++;
      if (curPoint == points.Length) {
        curPoint = 0;
        RightWall.position = (points[0] + points[1]) / 2f;
        RightWall.rotation = Quaternion.LookRotation(Vector3.up, Vector3.Cross(Vector3.up, (points[1] - points[0]).normalized));
        Debug.Log("Right Wall Set!");
        CornerState.text = "6) Now touch the left wall the same way.";
        curCorner++;
      }
    } else if (curCorner == 8) {
      points[curPoint] = cornerTransform.position;
      curPoint++;
      if (curPoint == points.Length) {
        curPoint = 0;
        LeftWall.position = (points[0] + points[1]) / 2f;
        LeftWall.rotation = Quaternion.LookRotation(Vector3.up, Vector3.Cross(Vector3.up, (points[1] - points[0]).normalized));
        Debug.Log("Left Wall Set!");
        CornerState.text = "1) Start by touching the center of the projector lens.";
        curCorner++;
      }
    }
    Sphere.GetComponent<SphereDistort>().Start();
  }
}