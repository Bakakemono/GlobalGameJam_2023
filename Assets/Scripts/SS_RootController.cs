using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SS_RootController : MonoBehaviour
{
    [SerializeField] SpriteShapeController spriteShapeController;

    Vector2 direction = Vector2.right;
    Vector2 input = Vector2.zero;

    int rootSplinePointNmb;

    float totalTimer = 2.0f;
    float currentTime = 0.0f;

    float speed = 0.02f;

    bool[] directionChecker;

    private void Start() {
        currentTime = totalTimer;
        rootSplinePointNmb = spriteShapeController.spline.GetPointCount();

        spriteShapeController.spline.InsertPointAt(
            rootSplinePointNmb - 1,
            (spriteShapeController.spline.GetPosition(rootSplinePointNmb - 1) + spriteShapeController.spline.GetPosition(rootSplinePointNmb - 2)) / 2.0f
            );
        rootSplinePointNmb++;

        directionChecker = new bool[4];
        for(int i = 0; i < directionChecker.Length; i++) {
            directionChecker[i] = false;
        }
        directionChecker[0] = true;

        for(int i = 0; i < spriteShapeController.spline.GetPointCount(); i++) {
            spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
    }

    private void Update() {
        input =
            new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
                );
    }

    private void FixedUpdate() {
        //Vector2 pos1 = spriteShapeController.spline.GetPosition(0);
        //Vector2 pos2 = spriteShapeController.spline.GetPosition(1);

        //spriteShapeController.spline.InsertPointAt(1, Vector2.up * 3.0f + (pos1 + pos2) / 2.0f);

        if(input.x > 0) {
            direction = Vector2.right;
            if(!directionChecker[0]) {
                for(int i = 0; i < directionChecker.Length; i++) {
                    directionChecker[i] = false;
                }
                directionChecker[0] = true;

                AddNewPointToSpline();
            }
        }
        else if(input.x < 0) {
            direction = Vector2.left;

            if(!directionChecker[1]) {
                for(int i = 0; i < directionChecker.Length; i++) {
                    directionChecker[i] = false;
                }
                directionChecker[1] = true;

                AddNewPointToSpline();
            }
        }
        else if(input.y > 0) {
            direction = Vector2.up;

            if(!directionChecker[2]) {
                for(int i = 0; i < directionChecker.Length; i++) {
                    directionChecker[i] = false;
                }
                directionChecker[2] = true;

                AddNewPointToSpline();
            }
        }
        else if(input.y < 0) {
            direction = Vector2.down;

            if(!directionChecker[3]) {
                for(int i = 0; i < directionChecker.Length; i++) {
                    directionChecker[i] = false;
                }
                directionChecker[3] = true;

                AddNewPointToSpline();
            }
        }

        spriteShapeController.spline.SetPosition(
            rootSplinePointNmb - 1,
            spriteShapeController.spline.GetPosition(rootSplinePointNmb - 1) + (Vector3)direction * speed);

        currentTime -= Time.fixedDeltaTime;
        if(currentTime > 0) {
            return;
        }
        currentTime = totalTimer;

        AddNewPointToSpline();
    }

    void AddNewPointToSpline() {
        //spriteShapeController.spline.SetTangentMode(rootSplinePointNmb - 1, ShapeTangentMode.Broken);

        spriteShapeController.spline.InsertPointAt(
            rootSplinePointNmb - 2,
            spriteShapeController.spline.GetPosition(rootSplinePointNmb - 2) +
            (spriteShapeController.spline.GetPosition(rootSplinePointNmb - 1) - spriteShapeController.spline.GetPosition(rootSplinePointNmb - 2)) * 0.95f
            );
        rootSplinePointNmb++;
        //spriteShapeController.spline.SetTangentMode(rootSplinePointNmb - 1, ShapeTangentMode.Broken);
    }
}