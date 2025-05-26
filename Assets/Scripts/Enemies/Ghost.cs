// using UnityEngine;
//
// public class Ghost : BaseMovingEnemy
// {
//     public void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
//         {
//             collision.gameObject.GetComponent<Damagable>().Damage(1);
//             // animator.SetTrigger("Vanish");
//             Destroy(gameObject, 0.6f);
//         }
//     }
// }
