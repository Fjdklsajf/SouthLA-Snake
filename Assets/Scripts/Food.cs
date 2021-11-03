using UnityEngine;

public class Food : MonoBehaviour
{
	public BoxCollider2D gridArea;

	private void Start() {
		SpawnRandom();
	}

	private void SpawnRandom() {
		Bounds boundaries = gridArea.bounds;

		float x = Random.Range(boundaries.min.x, boundaries.max.x);
		float y = Random.Range(boundaries.min.y, boundaries.max.y);

		this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			SpawnRandom();
		}
	}
}
