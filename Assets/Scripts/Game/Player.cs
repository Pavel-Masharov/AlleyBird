using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private float forceJump; //Сила прыжка
    private Rigidbody2D rb;

    private float speed; // Скорость движения
    private Vector2 direction; // Направление движения

    private readonly int jumpsMax = 2; // Максимальное количество прыжков за один отрыв от платформы
    private int jumps; // Поле для прыжков

    private int playerObj; // поместим номер слоя пользователя
    private int platformObj; // поместим номер слоя платформы

    private GameObject platformNext; // Для новой платформы
    private GameObject platformOld; // Для старой платформы

    public delegate void ColisionAction(); // делегат для обновления платформ
    public static event ColisionAction ActionEvent; // евент для обновления платформ

    public delegate void Death(); // делегат для проигрыша
    public static event Death DeathEvent; // евент для проигрыша

    public delegate void ColectedMonet(); // делегат для cбора монет
    public static event ColectedMonet ColectedMonetEvent; // евент для cбора монет

    public delegate void NewStep(); // делегат для повышения уровня
    public static event NewStep NewStepEvent; // евент для повышения уровня

    private Animator anim; // Аниматор
    static readonly int deathStateHash = Animator.StringToHash("death"); // хеш тригера аниматора

    private AudioSource playerAudio; // аудио
    [SerializeField] private AudioClip crashSound; // поле для аудио при ударе

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        forceJump = 22f;
        direction = Vector2.right;
        speed = 2f;
        playerObj = LayerMask.NameToLayer("Player");
        platformObj = LayerMask.NameToLayer("Ground");
    }

    private void FixedUpdate()
    {
        IgnorPlatforms(); // Игнорируем платформу сверху
    }

    void Update()
    {
        if (!GameManager.Instance.gameOver)
        {
            if (Input.GetMouseButtonDown(0) && jumps > 0)
            {
                Jump();
            }
            Move(direction); // Движение
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Turn(); // Разворот
        }

        if (collision.gameObject.GetComponent<Platform>())
        {
            StepUp(collision.gameObject);
            Invoke(nameof(AddJumps), 0.2f);
        }

        if (collision.gameObject.GetComponent<Enemy>())
        {
            DeathEvent();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            anim.SetTrigger(deathStateHash);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Monet>())
        {
            ColectedMonetEvent();
            Destroy(collision.gameObject);
        }
    }

    public void Jump() // Прыжок
    {
        rb.AddForce(Vector2.up * forceJump);
        jumps--;
    }

    private void Move(Vector2 direction) // Движение
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void Turn() // Разворот
    {
        direction.x *= -1;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void AddJumps() // Добавить прыжки
    {
        jumps = jumpsMax;
    }

    private void IgnorPlatforms() // Игнорируем платформу сверху
    {
        if (rb.velocity.y > 0)
        {
            Physics2D.IgnoreLayerCollision(playerObj, platformObj, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerObj, platformObj, false);
        }
    }

    private void StepUp(GameObject platform)
    {
        platformNext = platform;
        if (platformNext != platformOld)
        {
            jumps = 0;
            NewStepEvent();

            if (GameManager.Instance.level > 6)
            {
                ActionEvent();
            }
        }
        platformOld = platformNext;
    }
}
