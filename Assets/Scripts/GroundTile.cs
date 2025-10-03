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
        // Chỉ spawn coin vào 3 lane cố định (trái - giữa - phải)
        // Lấy giá trị từ Player để đảm bảo đồng bộ
        float laneDistance = Player.instance != null ? Player.instance.LaneDistance : 1.3f;
        float laneOffset = Player.instance != null ? Player.instance.LaneOffset : 0f;
        float[] lanePositions = { -laneDistance + laneOffset, 0f + laneOffset, laneDistance + laneOffset }; // 3 lane: trái, giữa, phải

        // Chọn ngẫu nhiên 1 trong 3 lane
        int randomLane = Random.Range(0, 3);
        float targetX = lanePositions[randomLane];

        // Lấy vị trí Z ngẫu nhiên trong ground tile
        Collider groundCollider = GetComponent<Collider>();
        float randomZ = Random.Range(groundCollider.bounds.min.z, groundCollider.bounds.max.z);

        // Tạo vị trí spawn cho coin
        Vector3 coinPosition = new Vector3(targetX, 1f, randomZ);

        GameObject temp = Instantiate(coinPrefab, transform);
        temp.transform.position = coinPosition;
    }

}
