using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private Rigidbody2D projectilePrefab;
    [SerializeField]
    private float projectileSpeed = 1.0f;
    [SerializeField]
    private int maxLifePoints = 3;
    private int lifePoints;
    [SerializeField]
    private Image healthBarFill;
    enum MovementState
    {
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4
    }

    private GameObject heroObject;
    private Animator animator;
    private Vector2 movementDirection;
    private string animationParameter = "Direction";

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Accelerate), 2, 5.0f);
        InvokeRepeating(nameof(FireProjectile), 1, Random.Range(1.0f,2.0f));

        animator = GetComponent<Animator>();

        lifePoints = maxLifePoints;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Accelerate()
    {
        movementSpeed++;
    }

    private void FireProjectile()
    {
        Rigidbody2D projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Vector2 distanceToPlayer = HeroController.Instance.transform.position - transform.position;
        projectileInstance.velocity = distanceToPlayer.normalized * projectileSpeed;
    }

    private void Movement()
    {
        heroObject = GameObject.FindWithTag("Player");

        if(heroObject != null)
        {
            // Translation
            movementDirection = heroObject.transform.position - transform.position;
            movementDirection.Normalize();
            transform.Translate(movementDirection * movementSpeed * Time.deltaTime);

            // Animation
            void SetAnimInt(MovementState argument)
            {
                animator.SetInteger(animationParameter, (int) argument);
            }

            // This logic flow determines which axis the character should animate on
            // Primarily Horizontal Movement
            if(Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if(movementDirection.x > 0) SetAnimInt(MovementState.Right);
                else if(movementDirection.x < 0) SetAnimInt(MovementState.Left);
            }
            // Primarily Vertical Movement
            else if(Mathf.Abs(movementDirection.y) > Mathf.Abs(movementDirection.x))
            {
                if(movementDirection.y > 0) SetAnimInt(MovementState.Up);
                else if(movementDirection.y < 0) SetAnimInt(MovementState.Down);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);

            lifePoints--;
            healthBarFill.fillAmount = (float) lifePoints / maxLifePoints;
            if(lifePoints <= 0) Destroy(gameObject);
        }
    }
}
