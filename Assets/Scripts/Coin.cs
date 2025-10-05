using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Coin collected by Player!");
            if (GameManager.instance != null)
            {
                Debug.Log("Calling GameManager.AddScore(1)");
                GameManager.instance.AddScore(1);
            }
            else
            {
                Debug.LogError("GameManager.instance is NULL!");
            }
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 2);
        }
    }
}
