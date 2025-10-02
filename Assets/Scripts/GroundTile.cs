using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject BarrierPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject coinPrefab;
    void Start()
    {
        SpawnBarrier();
        SpawnCoin();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GroundSpawner.instance.SpawnTile();
            Destroy(gameObject, 2);
        }
    }

    void SpawnBarrier()
    {
        int randomIndex = Random.Range(0, 3);
        Transform spawnPoint = transform.GetChild(randomIndex).transform;
        Instantiate(BarrierPrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        GameObject temp = Instantiate(coinPrefab, transform);
        temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );
        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }
        point.y = 1;
        return point;
    }
}
