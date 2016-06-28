using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject playerShip;
	public GameObject asteroid;
	public GameObject alien;
	public Collider2D boundary;
	public AudioSource oneUpSound;
	public AudioSource startGameSound;
	public SpriteRenderer background;

	public GUIText scoreGUI;
	public GUIText highscoreGUI;
	public GUIText gameOverGUI;
	public GUIText wavesGUI;
	public GUIText livesGUI;
	public GUIText restartText;
	public int waveSize;
	public int lives;
	public float respawnDelay;
	public float waveStartDelay;
	public float playerRespawnDelay;
	public int oneUpEvery;
	public int alienEvery;
	public int newPaletteEvery;

	private GameObject playerInstance;
	private int score = 0;
	private	int oneUpsAwarded = 0;
	private int currentWave = 1;
	private float respawnTime = 0f;
	private Color currentPalette;

	void Start()
	{
		currentPalette = background.color;
		updateScoreGUI();
		updateLivesGUI();
		StartCoroutine(spawnWaves());
	}

	void Update()
	{
		if (playerInstance == null)
		{
			if (lives == 0)
			{
				if (Input.GetButtonDown("Restart"))
				{
					startGameSound.Play();
					StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex));
				}
				else if (Input.GetButtonDown("Cancel"))
				{
					StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex - 1));
				}
			}
			else if (Time.time > respawnTime)
			{
				playerInstance = Instantiate(playerShip);
			}
		}
	}

	private IEnumerator spawnWaves()
	{
		while (lives > 0)
		{
			wavesGUI.text = "Wave " + currentWave;
			wavesGUI.GetComponent<TextFader>().fadeEffect();
			yield return new WaitForSeconds(waveStartDelay);

			for (int i = 0; i < waveSize + currentWave - 1; ++i)
			{
				spawnHazard(asteroid);
				yield return new WaitForSeconds(respawnDelay);
			}

			if (currentWave % alienEvery == 0)
			{
				spawnHazard(alien);
			}

			while (GameObject.FindGameObjectWithTag("Asteroid") != null || GameObject.FindGameObjectWithTag("Alien") != null)
			{
				yield return new WaitForSeconds(respawnDelay);
			}

			if (currentWave++ % newPaletteEvery == 0)
			{
				updatePalette();
			}
		}
	}

	private void updatePalette()
	{
		currentPalette = new Color(Random.value, Random.value, Random.value, 1f);
		StartCoroutine(fadeBackground(currentPalette, 1f));
	}

	void spawnHazard(GameObject hazard)
	{
		GameObject asteroidInstance = Instantiate(hazard);
		asteroidInstance.GetComponent<SpriteRenderer>().color = currentPalette;
		moveToRandom(asteroidInstance);
	}

	private void moveToRandom(GameObject hazard)
	{
		Bounds bounds = hazard.GetComponent<Collider2D>().bounds;
		Vector2 newPosition = new Vector2();

		do
		{
			float position = Random.Range(0f, 4f);

			if (position <= 1)
			{
				newPosition.x = (boundary.bounds.size.x + bounds.size.x) * position - (boundary.bounds.extents.x + bounds.extents.x);
				newPosition.y = boundary.bounds.max.y + bounds.extents.y;
			}
			else if (position <= 2)
			{
				newPosition.x = boundary.bounds.max.x + bounds.extents.x;
				newPosition.y = (boundary.bounds.size.y + bounds.size.y) * Mathf.Repeat(position, 1f) - (boundary.bounds.extents.y + bounds.extents.y);
			}
			else if (position <= 3)
			{
				newPosition.x = (boundary.bounds.size.x + bounds.size.x) * Mathf.Repeat(position, 1f) - (boundary.bounds.extents.x + bounds.extents.x);
				newPosition.y = boundary.bounds.min.y - bounds.extents.y;
			}
			else // (position <= 4)
			{
				newPosition.x = boundary.bounds.min.x - bounds.extents.x;
				newPosition.y = (boundary.bounds.size.y + bounds.size.y) * Mathf.Repeat(position, 1f) - (boundary.bounds.extents.y + bounds.extents.y);
			}
		}
		while (isOverlapping(bounds, newPosition));

		hazard.transform.position = newPosition;
	}

	private bool isOverlapping(Bounds bounds, Vector2 newPosition)
	{
		Vector2 one = new Vector2(newPosition.x - bounds.extents.x, newPosition.y - bounds.extents.y);
		Vector2 two = new Vector2(newPosition.x + bounds.extents.x, newPosition.y + bounds.extents.y);
		return Physics2D.OverlapAreaAll(one, two).Length > 1;
	}

	private void updateScoreGUI()
	{
		scoreGUI.text = "Score: " + score;
		highscoreGUI.text = "High: " + PlayerPrefs.GetInt("High Score");
	}

	private void updateLivesGUI()
	{
		livesGUI.text = "Lives: " + lives;
	}

	public void loseLife()
	{
		--lives;
		updateLivesGUI();
		respawnTime = Time.time + playerRespawnDelay;

		if (lives == 0)
		{
			gameOverGUI.GetComponent<TextFader>().fadeEffect(false);
			restartText.GetComponent<TextFader>().fadeEffect(false);

			if (score > PlayerPrefs.GetInt("High Score"))
			{
				highscoreGUI.text = "High: " + score;
				PlayerPrefs.SetInt("High Score", score);
			}
		}
	}

	public void addScore(int newScoreValue)
	{
		score += newScoreValue;
		updateScoreGUI();
		checkForBonus();
	}

	private void checkForBonus()
	{
		if (score >= oneUpEvery * (oneUpsAwarded + 1))
		{
			oneUpSound.Play();
			++oneUpsAwarded;
			oneUpEvery += oneUpEvery / 2;
			++lives;
			updateLivesGUI();
		}
	}

	private IEnumerator fadeBackground(Color target, float duration)
	{
		float begin = Time.time;
		float ratio;

		while ((ratio = Time.time - begin) <= duration)
		{
			background.color = Color.Lerp(background.color, target, ratio / duration);
			yield return null;
		}
	}

	private IEnumerator loadScene(int buildIndex)
	{
		float fadeTime = GetComponent<ScreenFader>().beginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(buildIndex);
	}
}