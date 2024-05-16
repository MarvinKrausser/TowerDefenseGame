using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraEditorMovement : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    public Camera camera;

    public TMP_Dropdown saveList;

    public TMP_InputField inputField;
    
    // Update is called once per frame
    void Update()
    {
        if (!inputField.isFocused)
        {
            Vector3 movement = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                movement.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement.y -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement.x += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement.x -= 1;
            }

            movement.Normalize();
            transform.Translate(movement * (camera.orthographicSize * 1.25f * Time.deltaTime));

            if (!saveList.IsExpanded)
            {
                camera.orthographicSize =
                    Mathf.Clamp((camera.orthographicSize + Input.mouseScrollDelta.y * Time.deltaTime * speed * -1), 1,
                        100);
            }
        }
    }
}