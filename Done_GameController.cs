using UnityEngine;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject[] hazards2;
    public Vector3 spawnValues;
    public int hazardCount;
    public int hazardCount2;
    public float spawnWait; // time between spawning 2 hazards
    public float startWait;  
    public float waveWait;// wait until next wave
    public float waitAfterWin;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private bool playerWins = false;
    private bool restart;
    private int score;
    private int wavesComplete = 0;
    public int waves2Go = 10000;
    private bool waitingToStartGame = false;
    public GameObject waitScreen;
    public GameObject mainPlayer;

   

    //called on 1st frame
    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        waitingToStartGame = true;

        if (waitScreen != null)
        { waitScreen.SetActive(true); }
        else
        {
            waitingToStartGame = false;
            Debug.LogError("waitScreen was not set in the inspector. Please set and try again");
        }
        if (mainPlayer != null)
        { mainPlayer.SetActive(false); }
        else
        { Debug.LogError("mainPlayer was not set in the inspector. Please set and try again"); }
        UpdateScore();
    }

    void Update()
    {
        // if the waitingToStartGame is enabled and the 'S' key has been pressed
        if (waitingToStartGame && (Input.GetKeyDown(KeyCode.Space)))
        {
            // set the flag to false so that will no longer be checking for input to start game
            waitingToStartGame = false;
            if (waitScreen != null)
            {
                waitScreen.SetActive(false);
            }
            if (mainPlayer != null)
            {
                mainPlayer.SetActive(true);
                StartCoroutine(mainGameLoop());
            }
        }

        if (restart)
        {
            // no score or X pressed = no post 
            if (Input.GetKeyDown(KeyCode.X) || (Input.GetKeyDown(KeyCode.Space) && (score == 0)))
            {
                waitingToStartGame = false;
                Application.LoadLevel(Application.loadedLevel);
            }

            //score > 0 
            if (Input.GetKeyDown(KeyCode.Space) && score>0)
            {
                ShareScoreOnTwitter(score);
                waitingToStartGame = true;
                 Application.LoadLevel(Application.loadedLevel);
            }
             
        }
    }

    IEnumerator mainGameLoop() //IEnumerator because its a coroutine
    {
        yield return new WaitForSeconds(startWait); // wait for starting the barrage
        //Main game loop
        while (true)
        {
            // asteroiden
            yield return new WaitForSeconds(waveWait); // wait until next wave
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                //spawnpos is random on x, z outside view
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                //inst. with no rotation:
                Quaternion spawnRotation = Quaternion.identity; //EulerRotation(float x, float y, float z);
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            // feindl. schiffe
            yield return new WaitForSeconds(waveWait); // wait until next wave
            for (int i = 0; i < hazardCount2; i++)
            {
                GameObject hazard2 = hazards2[Random.Range(0, hazards2.Length)];
                //spawnpos is random on x, z outside view
                Vector3 spawnPosition2 = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                //inst. with no rotation:
                Quaternion spawnRotation2 = Quaternion.identity;
                Instantiate(hazard2, spawnPosition2, spawnRotation2);
                yield return new WaitForSeconds(spawnWait);
            }
            wavesComplete++; waves2Go--;
            

            // WIN CONDITION
            if (waves2Go == 0) { gameOver = true; playerWins = true; }


            if (gameOver)
            {
                if (!playerWins)
                    gameOverText.text = "SPACE ATTACK FROM PLANET PLUTO \n\nGAME OVER! \n\nYOUR FINAL SCORE " + score + "\n\nPress Space to post to twitter and return to Start Menu"
                        + "\n\nPress X to not post and return to  Start Menu ";
                else
                    gameOverText.text = "SPACE ATTACK FROM PLANET PLUTO \n\nOMG YOU WIN R! \n\nYOUR FINAL SCORE " + score + "\n\nPress Space to post to twitter and return to Start Menu"
                        + "\n\nPress X to not post and return to  Start Menu ";
                restartText.text = "";
                yield return new WaitForSeconds(waitAfterWin);
                restart = true;
                waitingToStartGame = false;
                break; //break out of while (true) loop
            }
        }
    }

    /** *******************************************************************************************
     * ZU TUN:
    
    HIGH! youtube video about gameplay
    HIGH! post game code to github
    HIGH! SUBMIT TO JAM 
    
    MED High add #TheExtremelyTinyGameStudio Tag

    low .ico
    low warp vfx 
    low boss fight 
    low player ship
    low pause 
    low continous fire
 *  
    DONE post new game to itch
    DONE joystick fire
 *  DONE GAME OVER WINDOW share on Twitter
    DONE sound besser
 *  DONE 3-4 Lichtquellen in krassen Farben
 *  DONE musik besser
 *  DONE cheat codes f speed run -steuer rechts schiff rechts und qwer ===> warp 50 levels ==> cheat code is poiu gleichzeitig drücken
    * ********************************************************************************************
     */

    public void warpMe()
    {
        //nope Destroy all enemy objects
        RemoveEnemies();

        // inc Score???
        AddScore(1000);

        //remove some waves
        waves2Go  = waves2Go - 50;

        //XXX TODO Delay next enemy wave
        float storeWW = waveWait;
        waveWait = 10;

        //XXX TODO DO COOL VFX


        //XXX TODO reset delay to standard 
        waveWait = storeWW;

        //continue Game
    }

    //CALLED TO INCREASE OUR score when needed
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    { scoreText.text = "Score: " + score + "\nWaves completed: " + wavesComplete + "\nWaves to Go: " + waves2Go; }

    //CALLED TO end game 
    public void GameOver()
    { gameOver = true; }

    public void RemoveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }


    public static void ShareScoreOnTwitter(int score)
    {
        string msg = "OMG i just scored " + score + " on my game Space Attack from Planet Pluto!!! ";
        ShareTwitter(msg);
    }

    public static  void ShareTwitter(string text, string url= "https://textgas.itch.io/space-attack-from-planet-pluto",
                             string related="", string lang = "en")
    {
        const string adress = "http://twitter.com/intent/tweet";
        Application.OpenURL(adress +
                            "?text=" + WWW.EscapeURL(text) +
                            "&url=" + WWW.EscapeURL(url) +
                            "&related=" + WWW.EscapeURL(related) +
                            "&lang=" + WWW.EscapeURL(lang));
    }
} 