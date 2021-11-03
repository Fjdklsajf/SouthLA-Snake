using UnityEngine;

public class SnakeBody : MonoBehaviour, ILocation
{
	public SpriteRenderer spriteRenderer;
	public Sprite horizontal, vertical, topRight, topLeft, botRight, botLeft;
	public Sprite tailUp, tailDown, tailRight, tailLeft;
	public ILocation.Position position;
	public ILocation.Direction direction;

	public void setBody(ILocation.Position pos) {
		if(pos == ILocation.Position.horizontal) {
			spriteRenderer.sprite = horizontal;
		} else if (pos == ILocation.Position.vertical) {
			spriteRenderer.sprite = vertical;
		} else if (pos == ILocation.Position.topRight) {
			spriteRenderer.sprite = topRight;
		} else if (pos == ILocation.Position.topLeft) {
			spriteRenderer.sprite = topLeft;
		} else if (pos == ILocation.Position.botRight) {
			spriteRenderer.sprite = botRight;
		} else if (pos == ILocation.Position.botLeft) {
			spriteRenderer.sprite = botLeft;
		}
		position = pos;
	}

	public void setTail(ILocation.Direction dir) {
		if(dir == ILocation.Direction.right) {
			spriteRenderer.sprite = tailRight;
		} else if (dir == ILocation.Direction.left) {
			spriteRenderer.sprite = tailLeft;
		} else if (dir == ILocation.Direction.up) {
			spriteRenderer.sprite = tailUp;
		} else if (dir == ILocation.Direction.down) {
			spriteRenderer.sprite = tailDown;
		}
		direction = dir;
	}
}
