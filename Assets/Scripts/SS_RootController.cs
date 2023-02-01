using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SS_RootController : MonoBehaviour
{
    [SerializeField] SpriteShapeController spriteShapeController;

    float totalTimer = 2.0f;
    float currentTime = 0.0f;

    private void Start() {
        currentTime = totalTimer;
    }

    private void FixedUpdate() {
        currentTime -= Time.fixedDeltaTime;
        if(currentTime > 0) {
            return;
        }
        currentTime = totalTimer;

        Vector2 pos1 = spriteShapeController.spline.GetPosition(0);
        Vector2 pos2 = spriteShapeController.spline.GetPosition(1);

        spriteShapeController.spline.InsertPointAt(1, Vector2.up * 3.0f + (pos1 + pos2) / 2.0f);
    }
}