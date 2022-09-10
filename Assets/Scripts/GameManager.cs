using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	menu, inGame, gameOver
}

public class GameManager : MonoBehaviour
{
	public static GameManager sharedInstance;				//Variable que referencia al propio GameManager
	public GameState currentGameState = GameState.menu; 	//Variable para saber en que estado del juego nos encontramos,
	public Canvas menuCanvas, gameCanvas, gameOverCanvas;	//al inicio queremos que se encuentre en el menu principal.
	public int collectedObjects = 0;

	private void Awake()
	{
		sharedInstance = this;							//Se hace referencia a la propia instancia.
	}

	private void Start()
	{
		BackToMenu();									//Se empieza en el modo de juego "Pausa"
	}

	private void Update()				
	{
		if (Input.GetButtonDown("Start") && currentGameState != GameState.inGame)
		{
			StartGame();								//Solo empieza el juego si pulsamos Start y no estamos actualmente jugando, lo cual provocaria un reinicio de partida.
		}
		if (Input.GetButtonDown("Pause"))
		{
			BackToMenu();								//Si presionas el boton "Pausa" se activa el modo Pausa.
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ExitGame();
		}
	}

	public void StartGame()
	{
		SetGameState(GameState.inGame);					//Se establece el Estado de Juego: Jugando.
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
		cameraFollow.ResetCameraPosition();

		if(PlayerController.sharedInstance.transform.position.x>10)
		{
			LevelGenerator.sharedInstance.RemoveAllTheBlocks();
			LevelGenerator.sharedInstance.GenerateInitialBlocks();
		}

		PlayerController.sharedInstance.StartGame();	//La instancia compartida del player controller se establece a Empezar juego, por lo que ya podemos caminar, saltar, morir, etc...
		this.collectedObjects = 0;
	}

	public void GameOver()
	{
		SetGameState(GameState.gameOver);				//Se establece el Estado de juego: Fin de la partida.
	}

	public void BackToMenu()
	{
		SetGameState(GameState.menu);					//Se establece el Estado de juego: Pausa.
	}

	public void ExitGame()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	private void SetGameState(GameState newGameState)
	{
		if (newGameState == GameState.menu)				//preparar escena de Unity para mostrar el menu
		{
			menuCanvas.enabled = true;
			gameCanvas.enabled = false;
			gameOverCanvas.enabled = false;
		}
		else if (newGameState == GameState.inGame)		//preparar escena de Unity para jugar
		{
			menuCanvas.enabled = false;
			gameCanvas.enabled = true;
			gameOverCanvas.enabled = false;
		}
		else if (newGameState == GameState.gameOver)	//preparar escena de Unity para el gameover
		{
			menuCanvas.enabled = false;
			gameCanvas.enabled = false;
			gameOverCanvas.enabled = true;
		}

		this.currentGameState = newGameState;
	}

	public void CollectObject(int objectValue)
	{
		this.collectedObjects += objectValue;
		Debug.Log("Has recogido: " + this.collectedObjects + " objetos.");
	}

}