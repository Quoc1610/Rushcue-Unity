using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{
    public static PlayerManager _instance { get; private set; }

    [SerializeField] private CharacterController controller;
    [SerializeField] private FloatingJoystick joystick;

    public float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 50f; 

    [SerializeField] private Animator animator;
    public GameObject scanner;
    private bool isInsideTrigger = false;


    public static PlayerManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<PlayerManager>();
        }
        return _instance;
    }

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        scanner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void ChangeAnim(float amount)
    {
        animator.SetFloat("Blend", amount);
    }
    private void Move(){
        if (joystick.Direction != Vector2.zero)
        {
            
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            direction.Normalize();
            controller.Move(direction * speed * Time.deltaTime);
            if(speed < maxSpeed)
            {

                speed += 0.1f;
                ChangeAnim(speed/maxSpeed);
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
       
        }
        else
        {
            if(speed > 0)
            {
                speed -= 0.1f;
                controller.Move(direction * speed * Time.deltaTime);
                ChangeAnim(speed/maxSpeed);
            }

        }
    }
    private IEnumerator CheckOneSecond(Collider other)
    {
        yield return new WaitForSeconds(1);
        if (isInsideTrigger)
        {
            Debug.Log(other.gameObject.name);
            AllManager.Instance().CatchAnimal(other.gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Animal")
        {
            scanner.SetActive(true);
            isInsideTrigger = true;
            StartCoroutine(CheckOneSecond(other));
        }
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Animal")
        {
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Animal")
        {
            scanner.SetActive(false);
            isInsideTrigger = false;
        }
    }
}

