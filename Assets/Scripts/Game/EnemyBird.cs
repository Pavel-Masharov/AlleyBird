using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : Enemy
{
    private float speed; // Скорость движения
    private Vector2 direction; // Направление движения

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
            Turn(); // Разворот
        }
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
}
