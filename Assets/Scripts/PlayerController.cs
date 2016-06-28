using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public GameObject explosion;
	public AudioSource thrustSound;
	public ParticleSystem thrustEffect;
	public float angularSpeed;
	public float thrustForce;
	public float fireRate;
	public float maxSpeed;
	public float invincibilityDuration;

	private GameController gameController;
	private Rigidbody2D rb;
	private float nextFire = -1;
	private const float BLINK_INTERVAL = 0.25f;


	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(temporaryInvincibility());
	}

	private IEnumerator temporaryInvincibility()
	{
		Collider2D cld = GetComponent<Collider2D>();
		cld.enabled = false;

		Renderer rend = GetComponent<Renderer>();
		Color original = rend.material.color;
		float blinkDuration = (invincibilityDuration * BLINK_INTERVAL) / 2;

		for (float i = 0; i < invincibilityDuration; i += BLINK_INTERVAL * 2)
		{
			yield return StartCoroutine(fade(rend, Color.clear, blinkDuration));
			yield return StartCoroutine(fade(rend, original, blinkDuration));
		}

		cld.enabled = true;
	}

	private IEnumerator fade(Renderer rend, Color target, float duration)
	{
		float begin = Time.time;
		float ratio;

		while ((ratio = Time.time - begin) <= duration)
		{
			rend.material.color = Color.Lerp(rend.material.color, target, ratio / duration);
			yield return null;
		}
	}

	// Called just before each fixed physics frame
	void FixedUpdate()
	{
		rb.angularVelocity = Input.GetAxis("Horizontal") * angularSpeed;

		if (Input.GetButton("Thrust"))
		{
			rb.AddForce(transform.up * thrustForce);

			rb.velocity = new Vector2(
				Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
				Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));

			thrustEffect.Emit(5);

			if (!thrustSound.isPlaying)
			{
				thrustSound.Play();
			}
		}
		else
		{
			thrustSound.Stop();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;

			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case "AlienShot":
				Destroy(other.gameObject);
				goto case "Alien";
			case "Alien":
			case "Asteroid":
				Instantiate(explosion, transform.position, transform.rotation);
				Destroy(gameObject);
				gameController.loseLife();
				break;
		}
	}
}