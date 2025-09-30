using UnityEngine;

public class AddColliderToMap : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null &&
                child.gameObject.GetComponent<MeshCollider>() == null)
            {
                child.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}
