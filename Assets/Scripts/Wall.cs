using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	//public Sprite dmgSprite;                    //Alternate sprite to display after Wall has been attacked by player.
	//public int hp = 4;                          //hit points for the wall.

	private SpriteRenderer spriteRenderer;      //Store a component reference to the attached SpriteRenderer.

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	//DamageWall is called when the player attacks a wall.
	//public void DamageWall(int loss)
	//{
	//	SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
	//	spriteRenderer.sprite = dmgSprite;
	//	hp -= loss;
	//	if (hp <= 0)
	//		gameObject.SetActive(false);
	//}
}
