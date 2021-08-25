using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

// Game Manager requires other manager components
public class GameManager : MonoBehaviour
{
	[HideInInspector]
	public Transform playerTransform;
	
	// C# property to retrieve currently active instance of object, if any
	public static GameManager Instance
	{
		get
		{
			if (!instance) 
				instance = new GameObject ("GameManager").AddComponent<GameManager>(); 
			return instance;
		}
	}
		
	// C# property to retrieve save/load manager
	public static LoadSaveManager StateManager
	{
		get
		{
			if(!statemanager) 
				statemanager = instance.GetComponent<LoadSaveManager>();

			return statemanager;
			// return null;
		}
	}

	

	// Internal reference to single active instance of object - for singleton behaviour
	private static GameManager instance = null;

	// Internal reference to Saveload Game Manager
	static LoadSaveManager statemanager = null;

	// Should load from save game state on level load, or just restart level from defaults
	private static bool bShouldLoad = false;

	public bool shouldPlay;

	// Called before Start on object creation
	void Awake ()
	{
		//Check if there is an existing instance of this object
		if((instance) && (instance.GetInstanceID() != GetInstanceID()))
			Destroy(gameObject); //Delete duplicate
		else
		{
			instance = this; //Make this object the only instance
			DontDestroyOnLoad (gameObject); //Set as do not destroy
		}
	}
		
	// Use this for initialization
	void Start ()
	{
		// Check if the Level should be loaded
		if(bShouldLoad)
		{
			// Load the file to read from	
			StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
		
			// Reset load flag
			bShouldLoad = false; 
		}
		else
        {
			// currentLives = startingLives;
        }
		shouldPlay = true;
		// playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

    private void Update()
    {
	
    }



    // Restart Game
    public void RestartGame()
	{
		// Load first level
		SceneManager.LoadScene ("Level 1");
	}

	// Exit Game
	public void ExitGame()
	{
		Application.Quit();
	}

	// Save Game
	public void SaveGame()
	{
		// Print the path where the XML is save
		// Debug.Log(Application.persistentDataPath);

		playerTransform.GetComponent<Character>().SaveGamePrepare();

		// Call save game functionality
		StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
	}

	// Load Game
	public void LoadGame()
	{
		// Set load on restart
		bShouldLoad = true;

		//Call load game functionality
		StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");

		playerTransform.GetComponent<Character>().LoadComplete();
		// Restart Level
		//RestartGame();
	}


	public void InitLevel()
    {
		Time.timeScale = 1;
		playerTransform.GetComponent<Character>().currentLives = playerTransform.GetComponent<Character>().initialLives;
		
		shouldPlay = true;
		// playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
	}

	

}