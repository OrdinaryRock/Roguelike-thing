using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag.Equals("Projectile") || tag.Equals("EnemyProjectile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
