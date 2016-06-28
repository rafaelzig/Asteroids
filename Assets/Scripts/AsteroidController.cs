using UnityEngine;

public class AsteroidController : MonoBehaviour
{
	public GameObject explosion;
	public GameObject nextAsteroid;
	public Vector2 velocity;
	public Vector2 torque;
	public int maxSpawn;
	public int score;

	private GameController gameController;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = Random.Range(velocity.x, velocity.y) * Random.insideUnitCircle;
		rb.AddTorque(Random.Range(-1f, 1f) * Random.Range(torque.x, torque.y));
	}

	private void spawnChild()
	{
		if (nextAsteroid != null)
		{
			Color myPalette = GetComponent<SpriteRenderer>().color;
			int spawnCount = Random.Range(2, maxSpawn + 1);

			for (int i = 0; i < spawnCount; ++i)
			{
				GameObject child = Instantiate(nextAsteroid, transform.position, transform.rotation) as GameObject;
				child.GetComponent<SpriteRenderer>().color = myPalette;
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "PlayerShot":
				gameController.addScore(score);
				goto case "AlienShot";
			case "AlienShot":
				Destroy(other.gameObject);
				goto case "Player";
			case "Player":
			case "Alien":
				Instantiate(explosion, transform.position, transform.rotation);
				spawnChild();
				Destroy(gameObject);
				break;
		}
	}
}