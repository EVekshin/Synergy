// ExplosiveBarrel.cs
public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 5f;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > explosionForce)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(100f);
            }
            Explode();
        }
    }

    private void Explode()
    {
        // Создание эффекта взрыва
        // Уничтожение бочки
        Destroy(gameObject);
    }
}