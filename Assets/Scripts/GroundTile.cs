using UnityEngine;

public class GroundTile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GroundSpawner.instance.SpawnTile();
            Destroy(gameObject, 2);
        }
    }
}
