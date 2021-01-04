using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    public Unit target;
    public float damage;
    public float damagePercent;
    public GameObject VFX;
    private readonly float _speed = 0.05f;

    private float destroyTimer;

    private void Update()
    {
        if (target != null)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, target.transform.position + Vector3.up, _speed);
            destroyTimer = 0f;
        }
        else
        {
            if (destroyTimer >= 0.3f)
            {
                Destroy(gameObject);
            }
            else
            {
                destroyTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            target.TakeDamage( damage,damagePercent);
            Instantiate(VFX, target.transform);
            Destroy(gameObject);
        }
    }
}