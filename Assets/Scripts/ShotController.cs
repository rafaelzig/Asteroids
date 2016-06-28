using UnityEngine;

public class ShotController : MonoBehaviour
{
	public float shotVelocity;
	public float timeToLive;

	private ParticleSystem shotEffect;

	void Start()
	{
		shotEffect = GetComponent<ParticleSystem>();
		GetComponent<Rigidbody2D>().velocity = transform.up * shotVelocity;
		Destroy(gameObject, timeToLive);
	}

	void Update()
	{
		shotEffect.Emit(5);
	}
}