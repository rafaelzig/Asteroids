using UnityEngine;

public class Teleporter : MonoBehaviour
{
	private Bounds boundary;
	private Bounds bounds;

	void Start()
	{
		boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoxCollider2D>().bounds;
		bounds = GetComponent<Collider2D>().bounds;
	}

	void FixedUpdate()
	{
		Vector2 newPosition = new Vector2(
			                      Mathf.Repeat(transform.position.x + boundary.extents.x + bounds.extents.x, boundary.size.x + bounds.size.x) - (boundary.extents.x + bounds.extents.x),
			                      Mathf.Repeat(transform.position.y + boundary.extents.y + bounds.extents.y, boundary.size.y + bounds.size.y) - (boundary.extents.y + bounds.extents.y)
		                      );

		transform.position = newPosition;
	}
}