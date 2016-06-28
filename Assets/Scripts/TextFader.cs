using UnityEngine;
using System.Collections;

public class TextFader : MonoBehaviour
{
	public float duration;

	public void fadeEffect(bool isCompleteEffect = true)
	{
		StartCoroutine(fadeInOut(isCompleteEffect));
	}

	private IEnumerator fadeInOut(bool isCompleteEffect)
	{
		yield return StartCoroutine(fade(Color.white));

		if (isCompleteEffect)
		{
			yield return StartCoroutine(fade(Color.clear));	
		}
	}

	private IEnumerator fade(Color targetColor)
	{
		GUIText text = GetComponent<GUIText>();
		float begin = Time.time;

		while (text.color != targetColor)
		{
			text.color = Color.Lerp(text.color, targetColor, (Time.time - begin) / duration);
			yield return null;
		}
	}
}