using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void RollDice()
    {
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        rb.AddForce(transform.up * 600);
        rb.AddTorque(dirX, dirY, dirZ);
        GameController.instance.Rolled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DiceBoundary")
        {
            var speed = rb.velocity.magnitude;
            var direction = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 4f);
        }
    }
}
