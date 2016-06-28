using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
	public GUIText infoGUI;
	public AudioSource gameStartSound;

	private Color originalColor;
	private bool isGameStarting = false;

	void Start()
	{
		originalColor = infoGUI.color;
		InvokeRepeating("blinkText", 0f, 0.5f);
	}

	private void blinkText()
	{
		infoGUI.color = (infoGUI.color.Equals(Color.clear)) ? originalColor : Color.clear;
	}

	void Update()
	{
		if (!isGameStarting)
		{
			if (Input.GetButtonDown("Submit"))
			{
				isGameStarting = true;
				gameStartSound.Play();
				StartCoroutine(changeScene());
			}
			else if (Input.GetButtonDown("Cancel"))
			{
				Application.Quit();
			}
		}
	}

	private IEnumerator changeScene()
	{
		float fadeTime = GetComponent<ScreenFader>().beginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}