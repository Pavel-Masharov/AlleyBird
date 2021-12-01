using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : Enemy
{
    private float speed; // �������� ��������
    private Vector2 direction; // ����������� ��������

    private void OnEnable()
    {
        direction = Vector2.right;
        speed = 1f;
    }

    private void Update()
    {
        if(!GameManager.Instance.gameOver)
        {
            Move(direction);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Turn(); // ��������
        }
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
}
