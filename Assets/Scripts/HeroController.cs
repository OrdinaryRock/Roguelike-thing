using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    enum CharacterState
    {
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4,
        Idle = 5
    }

    [SerializeField]
    private float movementSpeed = 3.0f;
    private Vector2 movementDirection = new Vector2();

    Animator animator;
    string animationParameter = "AnimationState";

    Rigidbody2D rigidBody;

    [SerializeField]
    private Rigidbody2D projectilePrefab;
    private Rigidbody2D projectileInstance;
    private float projectileSpeed = 8.0f;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateState();
        WeaponFire();
    }

    // Fixed update is called at a fixed interval
    private void FixedUpdate()
    {
        MoveCharacter();
    }



    private void UpdateState()
    {
        void SetAnimInt(CharacterState argument)
        {
            animator.SetInteger(animationParameter, (int) argument);
        }

        if(movementDirection.x > 0) SetAnimInt(CharacterState.Right);
        else if(movementDirection.x < 0) SetAnimInt(CharacterState.Left);
        else if(movementDirection.y > 0) SetAnimInt(CharacterState.Up);
        else if(movementDirection.y < 0) SetAnimInt(CharacterState.Down);
        else SetAnimInt(CharacterState.Idle);
    }

    private void WeaponFire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            if(movementDirection.magnitude == 0) projectileInstance.velocity = Vector2.down * projectileSpeed;
            else projectileInstance.velocity = movementDirection.normalized * projectileSpeed;
        }
    }

    private void MoveCharacter()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
        movementDirection.Normalize();

        rigidBody.velocity = movementDirection * movementSpeed;
    }
}
