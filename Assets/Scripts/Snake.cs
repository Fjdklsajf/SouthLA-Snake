using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour, ILocation
{
	private Vector2 curDir = Vector2.right;
	private Vector2 newDir = Vector2.right;
	private Vector2 nextDir = Vector2.right;
	private List<GameObject> body = new List<GameObject>();
	private ILocation.Direction direction = ILocation.Direction.right;
	private ILocation.Direction nextDirection = ILocation.Direction.right;
	private ILocation.Position position = ILocation.Position.horizontal;

	public float velocity = 15f;
	public GameObject bodyPrefab;

	// sprites
	public SpriteRenderer spriteRenderer;
	public Sprite headLeft, headRight, headUp, headDown;

	// texts
	public Text scoreText, highScoreText;
	private int score = 1, highScore = 1;

	private void Awake() {
		// Set fixed update rate
		if(velocity <= 0) {
			velocity = 15f;
		}
		Time.fixedDeltaTime = (int)(100 / velocity) / 100f;

		// Ensure object transform position
		float x = Mathf.Round(this.transform.position.x);
		float y = Mathf.Round(this.transform.position.y);
		this.transform.position = new Vector3(x, y, 0);

		// Head right sprite by default
		spriteRenderer.sprite = headRight;
	}

	private void Start() {
		Reset();
	}

	// update every frame
	private void Update() {
		// Get direction from input
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			if (newDir == Vector2.up || newDir == Vector2.down) {
				nextDir = Vector2.left;
				nextDirection = ILocation.Direction.left;
			}

			if (curDir == Vector2.up || curDir == Vector2.down) {
				newDir = Vector2.left;
				direction = ILocation.Direction.left;
			}
		} else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			if (newDir == Vector2.up || newDir == Vector2.down) {
				nextDir = Vector2.right;
				nextDirection = ILocation.Direction.right;
			}

			if (curDir == Vector2.up || curDir == Vector2.down) {
				newDir = Vector2.right;
				direction = ILocation.Direction.right;
			}
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			if (newDir == Vector2.left || newDir == Vector2.right) {
				nextDir = Vector2.up;
				nextDirection = ILocation.Direction.up;
			}

			if (curDir == Vector2.left || curDir == Vector2.right) {
				newDir = Vector2.up;
				direction = ILocation.Direction.up;
			}
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			if (newDir == Vector2.left || newDir == Vector2.right) {
				nextDir = Vector2.down;
				nextDirection = ILocation.Direction.down;
			}

			if (curDir == Vector2.left || curDir == Vector2.right) {
				newDir = Vector2.down;
				direction = ILocation.Direction.down;
			}
		}
	}

	// update at a fixed rate
	private void FixedUpdate() {
		// Update positions
		if(curDir == newDir) { // no change in direction
			if(curDir == Vector2.left || curDir == Vector2.right) {	// horizontal position
				position = ILocation.Position.horizontal;
			} else {												// vertical position
				position = ILocation.Position.vertical;
			}
		} else { // direction changed
			if(curDir == Vector2.right) {
				if(newDir == Vector2.up) {		// right to up -> bottom right
					position = ILocation.Position.botRight;
				} else {						// right to down -> top right
					position = ILocation.Position.topRight;
				}
			} else if(curDir == Vector2.left) {
				if (newDir == Vector2.up) {		// left to up -> bottom left
					position = ILocation.Position.botLeft;
				} else {						// left to down -> top left
					position = ILocation.Position.topLeft;
				}
			} else if (curDir == Vector2.up) {
				if (newDir == Vector2.right) {	// up to right -> top left
					position = ILocation.Position.topLeft;
				} else {						// up to left -> top right
					position = ILocation.Position.topRight;
				}
			} else if (curDir == Vector2.down) {
				if (newDir == Vector2.right) {	// down to right -> bottom left
					position = ILocation.Position.botLeft;
				} else {						// down to left -> bottom right
					position = ILocation.Position.botRight;
				}
			}
		}

		SnakeBody back, front;
		// update body position and sprite
		for(int i = body.Count-1; i > 1; i--) {
			back = body[i].GetComponent<SnakeBody>();
			front = body[i - 1].GetComponent<SnakeBody>();
			// update transform position
			body[i].transform.position = body[i-1].transform.position;
			// update direction and position
			back.position = front.position;
			back.direction = front.direction;
		}
		// update body part that's attached to head
		if(body.Count > 1) {
			front = body[1].GetComponent<SnakeBody>();
			body[1].transform.position = this.transform.position;
			front.position = position;
			front.direction = direction;
		}

		// update head position
		this.transform.position = new Vector3(
			this.transform.position.x + newDir.x,
			this.transform.position.y + newDir.y,
			0
		);

		// update sprites
		UpdateSprite();

		curDir = newDir;
		newDir = nextDir;
		direction = nextDirection;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Food") {
			Grow();
		} else if(collision.tag == "Obstacle") {
			Reset();
		}
	}

	// instatiate body prefab, grow snake
	private void Grow() {
		// new tail
		GameObject tail = Instantiate(bodyPrefab);
		tail.transform.position = body[body.Count - 1].transform.position;
		body.Add(tail);

		// update sprite
		SnakeBody t = tail.GetComponent<SnakeBody>();
		if (body.Count <= 1) {  // if had no tail, update tail
			t.setTail(direction);
			t.position = position;
		} else {				// if had a tail, update tail and oldTail
			SnakeBody oldTail = body[body.Count - 1].GetComponent<SnakeBody>();
			t.setTail(oldTail.direction);
			t.position = oldTail.position;
			oldTail.setBody(oldTail.position);
		}

		// update score
		score++;
		scoreText.text = "Score: " + score.ToString();
		if (highScore < score) {
			highScore = score;
			highScoreText.text = "High Score: " + highScore.ToString();
		}

	}

	private void UpdateSprite() {
		// update head sprite
		if(direction == ILocation.Direction.right) {
			spriteRenderer.sprite = headRight;
		} else if (direction == ILocation.Direction.left) {
			spriteRenderer.sprite = headLeft;
		} else if (direction == ILocation.Direction.up) {
			spriteRenderer.sprite = headUp;
		} else if (direction == ILocation.Direction.down) {
			spriteRenderer.sprite = headDown;
		}

		if(body.Count <= 1) {
			return;
		}

		// update tail sprite
		SnakeBody tail = body[body.Count - 1].GetComponent<SnakeBody>();
		tail.setTail(tail.direction);

		// update body sprites
		SnakeBody b;
		for(int i = 1; i < body.Count - 1; i++) {
			b = body[i].GetComponent<SnakeBody>();
			b.setBody(b.position);
		}
	}

	// reset the game
	private void Reset() {
		// reset score
		score = 1;
		scoreText.text = "Score: " + score.ToString();
		highScoreText.text = "High Score: " + highScore.ToString();

		// destroy game objects
		for (int i = 1; i < body.Count; i++) {
			Destroy(body[i].gameObject);
		}

		// clear list
		body.Clear();

		// add head
		this.transform.position = Vector3.zero;
		body.Add(this.gameObject);
	}
}
