using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    Character character;

    public float mouseSensitive;

    private void Update()
    {
        Vector3 direction = Vector3.zero;

        if(Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z -= 1;
        }

        direction = character.transform.TransformDirection(direction);

        Vector3 rotation = Vector3.zero;
        rotation.x = Input.GetAxis("Mouse X") * mouseSensitive * Time.deltaTime;
        rotation.y = Input.GetAxis("Mouse Y") * mouseSensitive * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            character.Jump();
        }

        character.Move(direction);
        character.Rotate(rotation);
    }
}
