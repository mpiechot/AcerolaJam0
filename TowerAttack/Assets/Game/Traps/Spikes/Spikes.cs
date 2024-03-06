using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Minion minion = other.GetComponent<Minion>();
        if (minion != null)
        {
            minion.TakeDamage(damage);
        }
    }
}
