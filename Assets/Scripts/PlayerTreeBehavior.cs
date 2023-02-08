using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerTreeBehavior : MonoBehaviour
{
    enum Side {
        RIGHT = 1,
        LEFT = -1,
        NEITHER = 0
    }

    [SerializeField] SpriteShapeController[] roots;

    [Header("Tree Parameters")]
    int nmbRootsAvailable = 4;

    [Header("Root Parameters")]
    [SerializeField] float maxLengthPerRoot = 40.0f;
    [SerializeField] float propagationSpeed = 10.0f;
    [SerializeField] float segmentMaxLength = 3.0f;

    Vector2 currentDir = Vector2.zero;
    Vector2 newDir;
    float maxAngle = 45.0f;

    Vector2 input = Vector2.zero;
    readonly Vector3 up = Vector3.back;
    int framerate;

    private void Start() {
        framerate = (int)(1.0f / Time.fixedDeltaTime);
    }

    private void Update() {
        CaptureInpute();
    }

    private void FixedUpdate() {
        DirCheckerAndCorrecter();
    }
    
    void CaptureInpute() {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
            ).normalized;
        newDir = input;
    }

    void DirCheckerAndCorrecter() {
        if(newDir == Vector2.zero)
            return;

        if(Vector2.Angle(currentDir, newDir) > maxAngle) {
            Side side = CheckSide(currentDir, newDir);
            if(side == Side.RIGHT) {
                currentDir = new Vector2(-newDir.y, newDir.x);
                return;
            }
            else if(side == Side.RIGHT) {
                currentDir = new Vector2(newDir.y, -newDir.x);
                return;
            }
        }
        if(newDir != currentDir)

        currentDir = newDir;
    }

    // Check if the new direction is to the right or to the left of the current direction
    //Return 1 if the vector is to the right, -1 if to the left or 0 if perfectly align 
    Side CheckSide(Vector2 direction, Vector2 newDirection) {
        Vector3 cross = Vector3.Cross(direction, newDirection);
        float directionCheck = Vector3.Dot(cross, up);

        return directionCheck > 0 ? Side.RIGHT : directionCheck < 0 ? Side.LEFT : Side.NEITHER;
    }

    //void AddNewPointToSpline() {
    //    currentSegmentLength = 0;
    //    spriteShapeController.spline.InsertPointAt(
    //        rootSplinePointNmb - 1,
    //        spriteShapeController.spline.GetPosition(rootSplinePointNmb - 2) +
    //        (spriteShapeController.spline.GetPosition(rootSplinePointNmb - 1) - spriteShapeController.spline.GetPosition(rootSplinePointNmb - 2)) * 0.95f
    //        );
    //    rootSplinePointNmb++;
    //    spriteShapeController.spline.SetTangentMode(rootSplinePointNmb - 2, ShapeTangentMode.Continuous);
    //    if(rootSplinePointNmb > 10) {
    //        spriteShapeController.spline.SetTangentMode(rootSplinePointNmb - 10, ShapeTangentMode.Linear);
    //    }
    //}
}
