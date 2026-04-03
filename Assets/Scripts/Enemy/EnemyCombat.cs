using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    }

    public void Attack()
    {
        Debug.Log("Attacking Player Now!");
    }
}
