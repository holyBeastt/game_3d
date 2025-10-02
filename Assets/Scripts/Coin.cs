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
            GameManager.instance.score += 1;
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 2);
        }
    }
}
