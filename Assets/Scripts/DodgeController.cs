using UnityEngine;

public class DodgeController : MonoBehaviour
{
	public float dodgeForce;

	private Rigidbody2D parentBody;

	void Start()
	{
		parentBody = transform.parent.GetComponent<Rigidbody2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Asteroid"))
		{
			float angle = (Mathf.Atan2(
				              other.transform.position.y - transform.position.y,
				              other.transform.position.x - transform.position.x) - Mathf.PI / 2) * Mathf.Rad2Deg + 90;

			parentBody.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * dodgeForce;
		}
	}
}