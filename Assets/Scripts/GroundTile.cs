using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject BarrierPrefab;
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

        // Nếu tile ở gần vị trí bắt đầu, dịch lên trước để tránh va
        Vector3 pos = spawnPoint.position;
        if (transform.position.z < 30f)  // tile đầu tiên (z nhỏ)
        {
            pos.z += 20f; // đẩy vật cản ra xa hơn 20 đơn vị
        }

        Instantiate(BarrierPrefab, pos, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        float laneDistance = Player.instance != null ? Player.instance.LaneDistance : 1.3f;
        float laneOffset = Player.instance != null ? Player.instance.LaneOffset : 0f;
        float[] lanePositions = { -laneDistance + laneOffset, 0f + laneOffset, laneDistance + laneOffset };

        int randomLane = Random.Range(0, 3);
        float targetX = lanePositions[randomLane];

        Collider groundCollider = GetComponent<Collider>();
        float randomZ = Random.Range(groundCollider.bounds.min.z, groundCollider.bounds.max.z);

        // Nếu tile ở gần người chơi, đẩy coin ra xa thêm
        if (transform.position.z < 30f)
        {
            randomZ += 10f;
        }

        // ⚡ Thêm random độ cao (Y)
        // coinY = 1f (thấp - dễ ăn), 1.8f hoặc 2.2f (cao - phải nhảy mới ăn được)
        float[] possibleHeights = { 0.8f, 2.2f };
        float randomY = possibleHeights[Random.Range(0, possibleHeights.Length)];

        Vector3 coinPosition = new Vector3(targetX, randomY, randomZ);

        // Vector3 coinPosition = new Vector3(targetX, 1f, randomZ);

        GameObject temp = Instantiate(coinPrefab, transform);
        temp.transform.position = coinPosition;
    }
}
