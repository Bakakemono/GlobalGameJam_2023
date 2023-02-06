using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerTreeBehavior : MonoBehaviour
{
    [SerializeField] SpriteShapeController[] roots;

    [Header("Tree Parameters")]
    int nmbRootsAvailable = 4;

    [Header("Root Parameters")]
    [SerializeField] float maxLengthPerRoot = 40.0f;
    [SerializeField] float propagationSpeed = 10.0f;
    [SerializeField] float segmentMaxLength = 3.0f;

    Vector2 currentDirection = Vector2.zero;

    Vector2 input = Vector2.zero;
    int framerate;

    private void Start() {
        framerate = (int)(1.0f / Time.fixedDeltaTime);
    }

    private void Update() {
        CaptureInpute();
    }

    private void FixedUpdate() {
        
    }
    
    void CaptureInpute() {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
            );
    }
}
