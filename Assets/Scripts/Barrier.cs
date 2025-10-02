using UnityEngine;

public class Barrier : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Die();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
