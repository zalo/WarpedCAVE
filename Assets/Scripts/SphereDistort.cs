using UnityEngine;

public class SphereDistort : MonoBehaviour {
  public Camera EyeCam;
  public Camera ProjectionPerspective;
  public GameObject Sphere;
  public GameObject ViewPlane;
  public Plane[] planes;

  public float SphereRadiusSqrd = 0.4365f;

  public void Start() {
    ProjectionPerspective.GetComponent<ProjectorMatrix>().LateUpdate();
    Vector3[] vertices = ViewPlane.GetComponent<MeshFilter>().sharedMesh.vertices;
    Color[] colors = new Color[vertices.Length];

    for (int i = 0; i < vertices.Length; i++) {
      Vector3 vertpos = ViewPlane.transform.TransformPoint(vertices[i]);
      Vector3 rayDirection = (vertpos - ProjectionPerspective.transform.position).normalized;
      float t = 0;
      if ((t = intersectLineSphere(ProjectionPerspective.transform.position, rayDirection)) > 0f) {
        Vector3 hitPoint = ProjectionPerspective.transform.position + (rayDirection * t);
        RaycastHit hit;
        Vector3 reflectedDirection = Vector3.Reflect(rayDirection, (transform.position - hitPoint).normalized);

        if (Physics.Raycast(hitPoint, reflectedDirection, out hit)) {
          colors[i] = new Color(hit.point.x, hit.point.y, hit.point.z);
        } else {
          colors[i] = Color.black;
        }
      } else {
        colors[i] = Color.black;
      }
    }
    ViewPlane.GetComponent<MeshFilter>().sharedMesh.colors = colors;
  }

  void LateUpdate() {
    Shader.SetGlobalMatrix("_eyeProjection", EyeCam.projectionMatrix * EyeCam.worldToCameraMatrix);
  }

  float intersectLineSphere(Vector3 Origin, Vector3 Direction) {
    Vector3 L = transform.position - Origin;
    Vector3 offsetFromSphereCenterToRay = Vector3.Project(L, Direction) - L;
    return (offsetFromSphereCenterToRay.sqrMagnitude <= SphereRadiusSqrd) ? Vector3.Dot(L, Direction) - Mathf.Sqrt(SphereRadiusSqrd - offsetFromSphereCenterToRay.sqrMagnitude) : -1f;
  }
}

/*
 * Entire Screen is 1 quanta of light
say width = 10ft and height = 5 ft
light density is 0.02 quanta per sqfoot.
so the light density is just 1 quanta / whatever the screensize would be at that distance
cool, so now I have the light density at the surface of the mirror sphere

sphere surface area increases as 4*pi*(dist from surface)^2
so the light density is now:
(light density at sphere surface) * (surface area of sphere at surface/surface area of sphere at radius from surface)
since the 4pi's cancel in the ratio, the light density is now:
(light density at sphere surface) * (sphereRadius^2/distFromSphereCenter^2)

and we have the light density for each ray as a fraction of the total light emitted
*/