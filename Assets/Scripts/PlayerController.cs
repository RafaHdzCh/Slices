using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	//DECLARACION DE VARIABLES.

	public static PlayerController sharedInstance; 				//Solo debe ser un controlador por personaje, si duplico el Character no deben reaccionar los dos al mismo controlador.
	public float jumpForce = 5f;								//Fuerza de salto establecida a 5
	private Rigidbody2D rigidbody;								//Se crea un componente de cuerpo rigido
	public LayerMask groundLayer; 								//Detecta la capa del suelo.
	public Animator animator;									//Se le asigna una animacion
	public float runningSpeed = 1.5f;							//Velocidad de correr establecida a 1.5
	private Vector3 startPosition;								//Se declaran las coordenadas de inicio.
    private int healthPoints;
    private int manaPoints;
    public const int INITIAL_HEALTH = 130;
    public const int INITIAL_MANA = 15;
    public const int MAX_HEALTH = 180;
    public const int MAX_MANA = 30;
    public const int MIN_HEALTH = 20;
    public const int MIN_MANA = 0;
    public const float MIN_SPEED = 2.5f;
    public const float HEALTH_TIME_DECREASE = 1f;
    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;
    
	//DECLARACION DE METODOS.

	void Awake()
	{
		sharedInstance = this;						//Se hace referencia a la propia instancia compartida.
		rigidbody = GetComponent<Rigidbody2D>();	//Se llama al componente RigidBody.
		startPosition = this.transform.position; 	//Recupero la posicion inicial del personaje.
	}

    public void StartGame()
    {
        animator.SetBool("isAlive", true);			//Al empezar el juego, estamos vivos.
        animator.SetBool("isGrounded", true);		//Al empezar el juego, estamos tocando el suelo.
        this.transform.position = startPosition;	//Asigno la posicion inicial del personaje a la recuperada en el awake cada que reiniciamos la partida.
        this.healthPoints = INITIAL_HEALTH;
        this.manaPoints = INITIAL_MANA;

        StartCoroutine("TirePlayer");
    }

    IEnumerator TirePlayer()
    {
        while(this.healthPoints>MIN_HEALTH)
        {
            this.healthPoints--;
            yield return new WaitForSeconds(HEALTH_TIME_DECREASE);
        }
        yield return null;
    }

    void Update()
    {
    	if(GameManager.sharedInstance.currentGameState == GameState.inGame)   	//Solo podemos saltar si estamos en una partida.
    	{
        if(Input.GetMouseButtonDown(0))	//Si se presiona Espacio o Clic Izquierdo...
        {
        	Jump(false);																//... entonces saltamos
        }
        if(Input.GetMouseButtonDown(1))    //Si se presiona Espacio o Clic Izquierdo...
        {
            Jump(true);                                                             //... entonces saltamos
        }
        animator.SetBool("isGrounded", IsTouchingTheGround());					//Se establece que estamos tocando el suelo
    	}
    }

    void FixedUpdate()
    {
    	//Solo nos movemos si estamos en el modo inGame
    	if(GameManager.sharedInstance.currentGameState == GameState.inGame)
    	{
            float currentSpeed = (runningSpeed-MIN_SPEED) * (float)this.healthPoints / 100.0f;
    		if(rigidbody.velocity.x < currentSpeed)
    		{
    			rigidbody.velocity = new Vector2(currentSpeed, rigidbody.velocity.y);
    		}
    	}		
    }

    void Jump(bool isSuperJump)
    {
    	if(IsTouchingTheGround())												//Si estamos tocando el suelo...
    	{
            if(isSuperJump && this.manaPoints>SUPERJUMP_COST)
            {
                manaPoints -= SUPERJUMP_COST;
                rigidbody.AddForce(Vector2.up * jumpForce * SUPERJUMP_FORCE, ForceMode2D.Impulse);	//Aplicamos una fuerza en el eje positivo de las "Y" a modo de impulso multiplicado por la fuerza de salto.
            }
            else
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
    	}
    }

    bool IsTouchingTheGround()
    {
    	if(Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, groundLayer)) //Si lanzamos un rayo invisible desde los pies al personaje, a 10cm hacia abajo y se encuentra con la capa de suelo...
    	{
    		return true; 	//...Entonces estamos tocando el suelo.
    	}

    	else 
    	{
    		return false;	//Sino, no estamos tocando el suelo.
    	}
    }

    public void Kill()
    {
    	GameManager.sharedInstance.GameOver();		//Se hace referencia al GameManager para que utilice el estado de juego: fin del juego.
    	this.animator.SetBool("isAlive", false);	//La propiedad de estar vivo ahora es falsa.
        float currentMaxScore = PlayerPrefs.GetFloat("maxscore",0);

        if(currentMaxScore < this.GetDistance())
        {
            PlayerPrefs.SetFloat("maxscore", this.GetDistance());
        }

        StopCoroutine("TirePlayer");
    }

    public float GetDistance()
    {
        float travelledDistance = Vector3.Distance(new Vector2(startPosition.x , 0), 
                                                   new Vector2(this.transform.position.x , 0));
        return travelledDistance; //this.transform.position.x - startPosition.x
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;

        if(this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;

        if(this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return this.healthPoints;
    }

    public int GetMana()
    {
        return this.manaPoints;
    }
}
