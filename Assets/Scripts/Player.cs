using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;        //Allows us to use SceneManager
using UnityEngine.UI;
using Completed;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
    public int pointsPerFood = 20;                //Number of points to add to player food points when picking up a food object.
    public int pointsPerUtensilios = 30;                //Number of points to add to player food points when picking up a soda object.
    //public int wallDamage = 1;                    //How much damage a player does to a wall when chopping it.
    public string[] recetaNombres = { "PanArriba", "Hamburguesa", "Plancha","PanAbajo" };
    public int contadorPasos=0;
    private Animator animator;                    //Used to store a reference to the Player's animator component.
    private int points = 0;                            //Used to store player food points total during level.
    public Text pointsText;
    public AudioClip pickUpSound;
    public AudioClip gameOverSound;
    public AudioClip entregarSound;    
    


    private GameObject tick1;
    private GameObject tick2;
    private GameObject tick3;
    private GameObject tick4;
    private GameObject tick5;
    private GameObject tick6;
    private GameObject tick7;
    private GameObject tick8;



    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        tick1 = GameObject.Find("Tick1");
        tick1.SetActive(false);
        tick2 = GameObject.Find("Tick2");
        tick2.SetActive(false);
        tick3 = GameObject.Find("Tick3");
        tick3.SetActive(false);
        tick4 = GameObject.Find("Tick4");
        tick4.SetActive(false);
        tick5 = GameObject.Find("Tick5");
        tick5.SetActive(false);
        tick6 = GameObject.Find("Tick6");
        tick6.SetActive(false);
        tick7 = GameObject.Find("Tick7");
        tick7.SetActive(false);
        tick8 = GameObject.Find("Tick8");
        tick8.SetActive(false);
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();
        //Get the current food point total stored in GameManager.instance between levels.
        //points = GameManager.instance.playerFoodPoints;
        
        pointsText = GameObject.Find("PointsText").GetComponent<Text>();
        pointsText.text = "Points: " + points;
        //Call the Start function of the MovingObject base class.
        base.Start();
    }


    //This function is called when the behaviour becomes disabled or inactive.
    private void OnDisable()
    {
        //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        GameManager.instance.playerFoodPoints = points;
    }


    private void Update()
    {
        //Debug.Log(GameManager.instance.playersTurn);

        //If it's not the player's turn, exit the function.
        if (!GameManager.instance.playersTurn) return;
        int horizontal = 0;      //Used to store the horizontal move direction.
        int vertical = 0;        //Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        if (horizontal < 0)
        {
            animator.SetTrigger("playerLeft");

        }else if (horizontal > 0)
        {
            animator.SetTrigger("playerRight");
            
        } else if (vertical > 0)
        {
            animator.SetTrigger("playerUp");
    
        } 
        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }
        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);
        //Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;
        //If Move returns true, meaning Player was able to move into an empty space.
        if (Move(xDir, yDir, out hit))
        {
            //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
        }
        //Since the player has moved and lost food points, check if the game has ended.
        //CheckIfGameOver();
        //Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }


    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        ////Set hitWall to equal the component passed in as a parameter.
        //Wall hitWall = component as Wall;

        ////Call the DamageWall function of the Wall we are hitting.
        //hitWall.DamageWall(wallDamage);

        ////Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        //animator.SetTrigger("playerChop");
    }


    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            if (contadorPasos == recetaNombres.Length)
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
            }
        }

        //Check if the tag of the trigger collided with is Food.
        else if (other.tag == recetaNombres[contadorPasos])
        {
            SoundManager.instance.RandomizeSfx(pickUpSound, pickUpSound);
            if (recetaNombres[contadorPasos]=="Cuchillo" || recetaNombres[contadorPasos]=="Freidora"
            || recetaNombres[contadorPasos]=="Plancha" || recetaNombres[contadorPasos]=="Plato") {
                
                pointsPerUtensilios = 30;
                ////Add pointsPerFood to the players current food total.
                points += pointsPerUtensilios;
                pointsText.text = "Points: " + points;

            } else {
                pointsPerFood = 20;
                ////Add pointsPerFood to the players current food total.
                points += pointsPerFood;
                pointsText.text = "Points: " + points;
            
                ////Disable the food object the player collided with.
                other.gameObject.SetActive(false);
            }
            
            if (contadorPasos == 0){
                tick1.SetActive(true);
            } else if (contadorPasos == 1){
                tick2.SetActive(true);
            } else if (contadorPasos == 2){
                tick3.SetActive(true);
            } else if (contadorPasos == 3){
                tick4.SetActive(true);
            } else if (contadorPasos == 4){
                tick5.SetActive(true);
            } else if (contadorPasos == 5){
                tick6.SetActive(true);
            } else if (contadorPasos == 6){
                tick7.SetActive(true);
            } else if (contadorPasos == 7){
                tick8.SetActive(true);
            }
            contadorPasos++;
            if (contadorPasos == recetaNombres.Length){
                SoundManager.instance.RandomizeSfx(entregarSound, entregarSound);
            }
        }
    }


    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(0);
    }


    //LoseFood is called when an enemy attacks the player.
    //It takes a parameter loss which specifies how many points to lose.
   public void LosePoints(int loss)
    {
        //Set the trigger for the player animator to transition to the playerHit animation.
        //animator.SetTrigger("playerHit");

        //Subtract lost food points from the players total.
        points -= loss;
        pointsText.text ="Points: " + points;

        if (points < -20 ){
            GameManager.instance.GameOver(1);
        }
        //Check to see if game has ended.
        //CheckIfGameOver();
    }


    //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        //Check if food point total is less than or equal to zero.
        //if (food <= 0)
        //{
            //Call the GameOver function of GameManager.
            //GameManager.instance.GameOver();
        //}
    }
}
