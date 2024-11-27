using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance { get; private set; }

    [SerializeField] private CharacterController controller;
    [SerializeField] private FloatingJoystick joystick;

    public float speed = 2f;
    public float disRun = 0;
    public float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    public Vector3 posStart;
    [SerializeField] private Animator animator;
    public GameObject scanner;
    public float distance;
    private bool isInsideTrigger = false;
    public GameObject goArrow;


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
        posStart = transform.position;
        joystick=UIManager.Instance().uiGameplay.joystick;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDisToWave();
    }
    private void CheckDisToWave()
    {
        distance = Vector3.Distance(transform.position,AllManager.Instance().waveManager.waveInfo.waveObj.position);
        if (distance < 4f)
        {
            AllManager.Instance().ChangeCamera(1);
        }
    }
    private void ChangeAnim(float amount)
    {
        animator.SetFloat("Blend", amount);
    }
    private void Move()
    {
        if (joystick.Direction != Vector2.zero)
        {
            maxSpeed=3.5f;
            //maxSpeed = .18f * 3 + (.18f * UIManager.Instance().saveData.speedLvl)/2;
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            direction.Normalize();
            controller.Move(direction * speed * Time.deltaTime);
            

            speed=maxSpeed;
            ChangeAnim(speed / maxSpeed);
    

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        else
        {

            speed = 0f;
            controller.Move(direction * speed * Time.deltaTime);
            ChangeAnim(speed / maxSpeed);

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
            isInsideTrigger = true;
            scanner.SetActive(true);
            StartCoroutine(CheckOneSecond(other));
        }
        if (other.gameObject.tag == "Wave")
        {
            
            UIManager.Instance().OnPause(1);
            disRun = Vector3.Distance(transform.position, posStart)*100/18;
            UIManager.Instance().uiGameplay.joystick.OnPointerUp(null);
            UIManager.Instance().uiResult.OnSetUp(0,UIManager.Instance().CalculateCoin());
            UIManager.Instance().uiResult.gameObject.SetActive(true);

        }
        if (other.gameObject.tag == "Win")
        {
            disRun = Vector3.Distance(transform.position, posStart) * 100 / 18;
            UIManager.Instance().OnPause(1);
            UIManager.Instance().uiGameplay.joystick.OnPointerUp(null);
            //UIManager.Instance().uiGameplay.gameObject.SetActive(false);
            UIManager.Instance().uiResult.OnSetUp(1, UIManager.Instance().CalculateCoin());
            UIManager.Instance().uiResult.gameObject.SetActive(true);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Animal")
        {
            scanner.SetActive(false);
            isInsideTrigger = false;
            StopCoroutine(CheckOneSecond(other));
            
        }
    }
}

