using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
	public Texture2D fadeoutTexture;
	public float fadeSpeed = 0.8f;

	private const int DRAW_DEPTH = int.MinValue;
	private float alpha = 1f;
	private int fadeDir = -1;

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = DRAW_DEPTH;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeoutTexture);
	}

	public float beginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelWasLoaded()
	{
		beginFade(-1);
	}
}