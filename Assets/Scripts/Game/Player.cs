using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private float forceJump; //���� ������
    private Rigidbody2D rb;

    private float speed; // �������� ��������
    private Vector2 direction; // ����������� ��������

    private readonly int jumpsMax = 2; // ������������ ���������� ������� �� ���� ����� �� ���������
    private int jumps; // ���� ��� �������

    private int playerObj; // �������� ����� ���� ������������
    private int platformObj; // �������� ����� ���� ���������

    private GameObject platformNext; // ��� ����� ���������
    private GameObject platformOld; // ��� ������ ���������

    public delegate void ColisionAction(); // ������� ��� ���������� ��������
    public static event ColisionAction ActionEvent; // ����� ��� ���������� ��������

    public delegate void Death(); // ������� ��� ���������
    public static event Death DeathEvent; // ����� ��� ���������

    public delegate void ColectedMonet(); // ������� ��� c���� �����
    public static event ColectedMonet ColectedMonetEvent; // ����� ��� c���� �����

    public delegate void NewStep(); // ������� ��� ��������� ������
    public static event NewStep NewStepEvent; // ����� ��� ��������� ������

    private Animator anim; // ��������
    static readonly int deathStateHash = Animator.StringToHash("death"); // ��� ������� ���������

    private AudioSource playerAudio; // �����
    [SerializeField] private AudioClip crashSound; // ���� ��� ����� ��� �����

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
        IgnorPlatforms(); // ���������� ��������� ������
    }

    void Update()
    {
        if (!GameManager.Instance.gameOver)
        {
            if (Input.GetMouseButtonDown(0) && jumps > 0)
            {
                Jump();
            }
            Move(direction); // ��������
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Turn(); // ��������
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

    public void Jump() // ������
    {
        rb.AddForce(Vector2.up * forceJump);
        jumps--;
    }

    private void Move(Vector2 direction) // ��������
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void Turn() // ��������
    {
        direction.x *= -1;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void AddJumps() // �������� ������
    {
        jumps = jumpsMax;
    }

    private void IgnorPlatforms() // ���������� ��������� ������
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
