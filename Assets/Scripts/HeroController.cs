using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    public static HeroController Instance;
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
    private float projectileSpeed = 8.0f;

    [SerializeField]
    private int maxLifePoints = 10;
    private int lifePoints;
    [SerializeField]
    private Image healthBarFill;
    [SerializeField]
    private GameObject gameOverText;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip healSound;

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        lifePoints = maxLifePoints;
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
            Rigidbody2D projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            if(movementDirection.magnitude == 0) projectileInstance.velocity = Vector2.down * projectileSpeed;
            else projectileInstance.velocity = movementDirection.normalized * projectileSpeed;
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
        }
    }

    private void MoveCharacter()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
        movementDirection.Normalize();

        rigidBody.velocity = movementDirection * movementSpeed;
    }

    private void TakeDamage(int damagePoints)
    {
        AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position);
        lifePoints -= damagePoints;
        healthBarFill.fillAmount = (float) lifePoints / maxLifePoints;
        if(lifePoints <= 0)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
            gameOverText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void Heal(int healingPoints)
    {
        lifePoints += healingPoints;
        if(lifePoints > maxLifePoints) lifePoints = maxLifePoints;
        AudioSource.PlayClipAtPoint(healSound, Camera.main.transform.position);
        healthBarFill.fillAmount = (float) lifePoints / maxLifePoints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag.Equals("EnemyProjectile"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        if(tag.Equals("Enemy"))
        {
            TakeDamage(2);
            Destroy(collision.gameObject);
        }
        if(collision.CompareTag("Health"))
        {
            Heal(collision.GetComponent<Healthpak>().healingAmount);
            Destroy(collision.gameObject);
        }
    }
}
