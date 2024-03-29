using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBehavior : MonoBehaviour
{
    [SerializeField] GameObject rootCorePreabs;
    Vector2 direction = Vector2.up;
    Vector2 input = Vector2.zero;

    Vector2 startingPos = Vector2.zero;

    float waitingTime = 0.2f;
    float currentWaitTime = 0.0f;

    private void Start() {
        currentWaitTime = waitingTime;
    }

    private void Update() {
        input =
            new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
                );
    }

    private void FixedUpdate() {
        currentWaitTime -= Time.fixedDeltaTime;
        if(currentWaitTime > 0) {
            return;
        }
        currentWaitTime = waitingTime;

        if(input.x > 0) {
            direction = Vector2.right;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90.0f));
        }
        else if(input.x < 0) {
            direction = Vector2.left;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
        }
        else if(input.y > 0) {
            direction = Vector2.up;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0.0f));
        }
        else if(input.y < 0) {
            direction = Vector2.down;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180.0f));
        }

        GameObject.Instantiate(rootCorePreabs, transform.position, Quaternion.identity);
        transform.position += (Vector3)direction;
    }
}
