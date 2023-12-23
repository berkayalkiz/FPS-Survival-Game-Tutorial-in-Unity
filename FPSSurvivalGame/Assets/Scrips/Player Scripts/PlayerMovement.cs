using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private CharacterController character_Controller;

    private Vector3 move_Direction;

    public float speed = 5f;
    private float gravity = 20f;

    public float jump_Force = 10f;
    private float vertical_Velocity;


    private void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        character_Controller.Move(move_Direction);

    }


}
