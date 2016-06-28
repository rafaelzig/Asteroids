using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
	void Start()
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}
}
