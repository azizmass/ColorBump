using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallControlller : MonoBehaviour
{
    [SerializeField] float wallDistance = 5f;
    [SerializeField] float minCamDistance = 4f;
    [SerializeField] Material[] materials;


    // Reference to the RigidBody Component
    Rigidbody rb;

    // Used to calculate the direction from the mouse's last to current position
    Vector2 lastMousePos = Vector2.zero;

    // The force with which we can move the ball by swiping
    public float thrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    public Button startButton;
    public Canvas menue;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;

        // Hides the button
        menue.gameObject.SetActive(false);
    }




    public GameObject winPanel;

    IEnumerator Win(float delayTime)
    {
        // Do stuff before waiting 
        thrust = 0;
        speed = 0;
        rb.velocity = Vector3.zero;

        //Wait some time 
        yield return new WaitForSeconds(delayTime);

        // Do other stuff after waiting

        // Activate the pannel
        winPanel.gameObject.SetActive(true);

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            StartCoroutine(Win(0.5f));
        }
    }



    public void setMaterial(int index)
    {
        if(index<materials.Length)
        {
            GetComponent<Renderer>().material = materials[index];
        }
    }

    // Update is called once per frame



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Evil")
        {
            StartCoroutine(Die(2));
        }
    }


    IEnumerator Die(float delayTime)
    {
        // Do stuff before replaying the level 
        Debug.Log("You're dead");

        // Stop the Player from moving
        speed = 0;
        thrust = 0;

        // Wait some seconds
        yield return new WaitForSeconds(delayTime);

        // Do stuff after waiting some seconds

        //Replay the Level
        SceneManager.LoadScene(0);
    }


    public float speed = 5f;

    void FixedUpdate()
    {
        //Move the ball forward
        rb.MovePosition(rb.position + Vector3.forward * speed * Time.fixedDeltaTime);

        // Move the Camera forward
        Camera.main.transform.position += Vector3.forward * speed * Time.fixedDeltaTime;
    }




    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        if(pos.z<Camera.main.transform.position.z+minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }
        
        if (pos.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        else if(pos.x>wallDistance){
        pos.x = wallDistance;
        }
        transform.position = pos;
    }
    void Update()
    {

        Vector2 deltaPos=Vector2.zero;
        if(Input.GetMouseButton(0))
        {
            Vector2 currentMousePos=Input.mousePosition;
            if (lastMousePos == Vector2.zero)
            {
                lastMousePos=currentMousePos;
            }
            deltaPos= currentMousePos - lastMousePos;
            Vector3 force=new Vector3(deltaPos.x,0,deltaPos.y)*thrust; 
            rb.AddForce(force);
        }
        else
        {
            lastMousePos=Vector2.zero;
        }
    }


}
