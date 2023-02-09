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
    int[] rootsSegmentNmbs;

    [Header("Tree Parameters")]
    int maxRootNmb = 5;
    int availableRootNmb = 4;
    int currentRootIndex = 0;

    [Header("Root Parameters")]
    [SerializeField] float maxLengthPerRoot = 40.0f;
    [SerializeField] float propagationSpeed = 10.0f;
    [SerializeField] float segmentMaxLength = 3.0f;
    [SerializeField] float segmentMinLength = 1.0f;
    [SerializeField] float segmentCurrentLength = 0;
    [SerializeField] float rootCurrentLength = 0;

    Vector2 currentDir = Vector2.right;
    Vector2 newDir;
    float maxAngle = 45.0f;

    Vector2 input = Vector2.zero;
    readonly Vector3 up = Vector3.back;
    int framerate;

    private void Start() {
        framerate = (int)(1.0f / Time.fixedDeltaTime);

        rootsSegmentNmbs = new int[maxRootNmb];
        for(int i = 0; i < roots.Length; i++) {
            rootsSegmentNmbs[i] = roots[i].spline.GetPointCount();
        }
        AddNewPointToSpline(currentRootIndex);
    }

    private void Update() {
        CaptureInpute();
    }

    private void FixedUpdate() {
        DirCheckerAndCorrector();

        roots[currentRootIndex].spline.SetPosition(
            rootsSegmentNmbs[currentRootIndex] - 1,
            roots[currentRootIndex].spline.GetPosition(rootsSegmentNmbs[currentRootIndex] - 1) + (Vector3)currentDir * (propagationSpeed * Time.fixedDeltaTime)
            );

        for(int i = 0; i < rootsSegmentNmbs[currentRootIndex]; i++) {
            roots[currentRootIndex].spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

        rootCurrentLength += (propagationSpeed * Time.fixedDeltaTime);

        segmentCurrentLength += (propagationSpeed * Time.fixedDeltaTime);

        if(newDir != Vector2.zero && newDir != currentDir && segmentCurrentLength > segmentMinLength) {
            AddNewPointToSpline(currentRootIndex);
        }
        else if(segmentCurrentLength >= maxLengthPerRoot) {
            AddNewPointToSpline(currentRootIndex);
        }
    }
    
    void CaptureInpute() {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
            ).normalized;
        newDir = input;
    }

    void DirCheckerAndCorrector() {
        if(newDir == Vector2.zero)
            return;

        if(Vector2.Angle(currentDir, newDir) > maxAngle) {
            Side side = CheckSide(currentDir, newDir);
            if(side == Side.RIGHT) {
                currentDir = (currentDir + new Vector2(-newDir.y, newDir.x)).normalized;
                return;
            }
            else if(side == Side.RIGHT) {
                currentDir = (currentDir + new Vector2(newDir.y, -newDir.x)).normalized;
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

    void AddNewPointToSpline(int rootIndex) {
        segmentCurrentLength = 0;
        roots[rootIndex].spline.InsertPointAt(
            rootsSegmentNmbs[rootIndex] - 1,
            roots[rootIndex].spline.GetPosition(rootsSegmentNmbs[rootIndex] - 2) +
            (roots[rootIndex].spline.GetPosition(rootsSegmentNmbs[rootIndex] - 1) - roots[rootIndex].spline.GetPosition(rootsSegmentNmbs[rootIndex] - 2)) * 0.95f
            );
        rootsSegmentNmbs[rootIndex]++;
        roots[rootIndex].spline.SetTangentMode(rootsSegmentNmbs[rootIndex] - 2, ShapeTangentMode.Continuous);
        //if(rootsSegmentNmbs[rootIndex] > 10) {
        //    roots[rootIndex].spline.SetTangentMode(rootsSegmentNmbs[rootIndex] - 10, ShapeTangentMode.Linear);
        //}
    }
}
