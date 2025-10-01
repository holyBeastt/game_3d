using UnityEngine;

// #if UNITY_EDITOR
// [ExecuteInEditMode]
// #endif
public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner instance;
    private Vector3 nextSpawnPoint;
    public GameObject groundTile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (Application.isPlaying)   // chỉ chạy khi bấm Play
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnTile();
            }
        }
    }

    [ContextMenu("Spawn Tiles In Editor")]
    public void SpawnTilesInEditor()
    {
        // Xóa các tile cũ (nếu có)
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        nextSpawnPoint = transform.position;
        for (int i = 0; i < 10; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity, transform);
        //nextSpawnPoint = temp.transform.GetChild(1).transform.position;
        nextSpawnPoint += new Vector3(0, 0, 15.96f);

    }
}