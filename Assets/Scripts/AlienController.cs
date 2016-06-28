using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour
{
	public GameObject shot;
	public GameObject explosion;
	public Vector2 maneuverRate;
	public Vector2 velocity;
	public float fireRate;
	public float initialShotDelay;
	public int score;

	private GameController gameController;
	private Rigidbody2D rb;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		rb = GetComponent<Rigidbody2D>();
		InvokeRepeating("fire", initialShotDelay, fireRate);
		InvokeRepeating("moveRandomly", 0, Random.Range(maneuverRate.x, maneuverRate.y));
//		initialSpeed = Random.Range(velocity.x, velocity.y) * -Mathf.Sign(transform.position.x);
//		initialPosition = transform.position.y;
//		rb.velocity = new Vector2(initialSpeed, 0f);
//		StartCoroutine(evade());
	}

	void moveRandomly()
	{
		rb.velocity = Random.Range(velocity.x, velocity.y) * Random.insideUnitCircle;
	}

	private void fire()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			float angle = (Mathf.Atan2(
				              player.transform.position.y - transform.position.y,
				              player.transform.position.x - transform.position.x) - Mathf.PI / 2) * Mathf.Rad2Deg;

			Instantiate(shot, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
		}
	}

	//	private IEnumerator evade()
	//	{
	//		yield return new WaitForSeconds(Random.Range(initialManeuverDelay.x, initialManeuverDelay.y));
	//
	//		while (true)
	//		{
	//			targetManeuver = Random.Range(1.0f, maneuverDisplacement) * -Mathf.Sign(transform.position.y);
	//			yield return new WaitForSeconds(Random.Range(maneuverRate.x, maneuverRate.y));
	//			targetManeuver = initialPosition;
	//			yield return new WaitForSeconds(Random.Range(maneuverDelay.x, maneuverDelay.y));
	//		}
	//	}
	//	void FixedUpdate()
	//	{
	//		float newManeuver = Mathf.MoveTowards(rb.velocity.y, targetManeuver, Time.deltaTime * smoothing);
	//		rb.velocity = new Vector2(initialSpeed, newManeuver);
	//	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "PlayerShot":
				gameController.addScore(score);
				Destroy(other.gameObject);
				goto case "Player";
			case "Player":
			case "Asteroid":
				Instantiate(explosion, transform.position, transform.rotation);
				Destroy(gameObject);
				break;
		}
	}
}