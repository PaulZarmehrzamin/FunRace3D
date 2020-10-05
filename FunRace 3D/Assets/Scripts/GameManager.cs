// Madbox Test - FunRace 3D
// By Paul Zarmehrzamin

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform player;    // Player
    public Animator animator;   // Animator
    public float speed;         // Movement speed
    public float restartDelay;  // Delay before restarting the game

    [SerializeField]
    private Transform[] paths;  // Paths to follow
    public float tPath;         // Progression on current path
    private bool run;           // Game is running

    public GameObject uiHold;   // Hold to run UI
    public GameObject uiWin;    // Level complete UI
    public AudioSource sHit;    // Hit sound
    public AudioSource sWin;    // Win sound

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Start is called before the first frame update
    void Start()
    {
        // Set player position
        tPath = 0.25f;
        transform.position = nextPosition(tPath);
        transform.LookAt(nextPosition(tPath + speed * Time.deltaTime));
        run = true;
    }

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Update is called once per frame
    void Update()
    {
        // Check that we reach the last path
        if (tPath >= paths.Length - 0.25f && run)
        {
            run = false;
            EndGame();
        }
        // Move player position if click/spacebar/touchscreen
        else if (run && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.touchCount > 0))
        {
            // Get new progression
            tPath += speed * Time.deltaTime;
            // Set rotation and position
            transform.position = nextPosition(tPath);
            transform.LookAt(nextPosition(tPath + speed * Time.deltaTime));
            animator.SetBool("isRunning", true);
            // Hide hold to run
            uiHold.SetActive(false);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    // Get next position
    Vector3 nextPosition(float tPath)
    {
        // Get points of the current path
        int currentPath = Mathf.FloorToInt(tPath);
        Vector3 point0 = paths[currentPath].GetChild(0).position;
        Vector3 point1 = paths[currentPath].GetChild(1).position;
        Vector3 point2 = paths[currentPath].GetChild(2).position;
        Vector3 point3 = paths[currentPath].GetChild(3).position;
        // Get new position according to the parameter tPath
        float t = tPath % 1f;
        return (Mathf.Pow(1 - t, 3) * point0
            + 3 * t * Mathf.Pow(1 - t, 2) * point1
            + 3 * (1 - t) * Mathf.Pow(t, 2) * point2
            + Mathf.Pow(t, 3) * point3);
    }

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Restart at the last Checkpoint
    public void RestartCheckpoint()
    {
        if (run)
        {
            run = false;
            // Set player at checkpoint
            if (tPath >= 5f)
            {
                tPath = 5f;
            }
            else if (tPath >= 1f)
            {
                tPath = 1f;
            }
            else
            {
                tPath = 0.25f;
            }
            // Hit Animation
            animator.SetBool("isHit", true);
            // Play sound
            sHit.Play();

            Invoke("Restart", restartDelay);

        }
    }

    void Restart()
    {
        Debug.Log("Restart at Checkpoint : " + Mathf.FloorToInt(tPath));
        // Reset game manager position and rotation
        transform.position = nextPosition(tPath);
        transform.LookAt(nextPosition(tPath + speed * Time.deltaTime));
        // Reset player position, rotation, inertia and animation
        Rigidbody r = player.GetComponent<Rigidbody>();
        r.useGravity = false;
        r.velocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
        animator.SetBool("isHit", false);
        // Game is ready
        run = true;
        uiHold.SetActive(true);
    }

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    void EndGame()
    {
        Debug.Log("Endgame");
        animator.SetTrigger("isDancing");
        uiWin.SetActive(true);
        sWin.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
