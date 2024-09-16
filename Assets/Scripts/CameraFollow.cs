using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject go_followObject;
    [SerializeField]
    private Vector2 v2_cameraOffset;
    [SerializeField]
    private Vector2 v2_threshold;
    [SerializeField]
    Rect rct_aspect;
    [SerializeField]
    float f_speed = 3f;

    private Rigidbody2D rb_player;

    void Start()
    {
        rb_player = go_followObject.GetComponent<Rigidbody2D>();
        v2_threshold = calculateThreshold();
    }

    private void FixedUpdate()
    {
        Vector2 follow = go_followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 v3_newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= v2_threshold.x)
        {
            v3_newPosition.x = follow.x;
        }

        if (Mathf.Abs(yDifference) >= v2_threshold.y)
        {
            v3_newPosition.y = follow.y;
        }
        
        float moveSpeed = rb_player.velocity.magnitude > f_speed ? rb_player.velocity.magnitude : f_speed;
        transform.position = Vector3.MoveTowards(transform.position, v3_newPosition, moveSpeed * Time.deltaTime);

    }

    private Vector3 calculateThreshold()
    {
        //Here we get the rectangle measurements of the camera that we can use to calculate the distances needed.
        rct_aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * rct_aspect.width / rct_aspect.height, Camera.main.orthographicSize);
        t.x -= v2_cameraOffset.x;
        t.y -= v2_cameraOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}