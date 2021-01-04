using UnityEngine;

public class Fireball : MonoBehaviour
{
    private SphereCollider _collider;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private GameObject sphere;
    private EnemyUnit unit;

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 9);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) ;
        {
            var colls = Physics.OverlapSphere(other.gameObject.transform.position, 3f);

            foreach (var coll in colls)
                if (coll.GetComponent<EnemyUnit>() != null)
                {
                    coll.GetComponent<EnemyUnit>().TakeDamage(1, 50f);
                    Debug.Log("HIT");
                }

            Destroy(gameObject);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}