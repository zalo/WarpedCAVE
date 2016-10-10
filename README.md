WarpedCAVE
=====================

An experiment extending Paul Bourke's mirrored sphere projection technique to 3D in Unity, for the purpose of building low-cost single-projector, single computer immersive CAVEs on arbitrary surfaces.  After some initial pre-computation, this technique allows one to render perspective corrected 3D imagery on arbitrary surfaces using a very cheap vertex shader and an on-axis rendering camera (enabling a variety of graphical effects that traditional CAVEs are not normally capable of).

Includes a guided calibration routine for assembling the projection geometry (via probing the bottom tip of a Vive controller to various surfaces to ascertain their shape).   The automated routine calculates the projector's projection matrix, three flat walls, and the position + radius of the mirrored sphere reflector (though each step can be easily extended to arbitrary geometry).


How the pre-distortion works:
1) For each vertex in the distortion plane, trace a ray from the projector to the projection surface (simulating the reflection off of the mirrored sphere in between).
2) Record the hit position, and bake it into the vertex color of the distortion plane mesh.
3) In the distortion mesh, reproject that world position back into the viewport space of the eye's render texture camera.
4) Set the UV of that vertex in the distortion plane to be equal to 3)'s viewport space coordinate.


This example scene is set up such that the "eye camera" is looking through the ring on the right vive controller.

Here are Before/After comparison screenshots:

<img src="https://github.com/zalo/WarpedCAVE/raw/master/Images/Before.png" width="300"> <img src="https://github.com/zalo/WarpedCAVE/raw/master/Images/After.png" width="300">

Video (shows 3D effect):

[![3D Warped CAVE Demo](http://img.youtube.com/vi/IcZejDUULB8/0.jpg)](http://www.youtube.com/watch?v=IcZejDUULB8)

(note the wavy distortions in the grid pattern introduced by the budget spherical mirror; primary surface reflectors would not have this issue)
