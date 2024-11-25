using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllManager : MonoBehaviour
{
    private static AllManager _instance;
    public List<Transform> lsPosSpawn = new List<Transform>();
    public AnimalConfig animalConfig;
    public AnimalManager animalManager;
    public WaveManager waveManager;
    public GameObject wavePrefab;
    public Transform waveSpawnPos;
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;
    public List<GameObject> lsCaughtAnimal = new List<GameObject>();

    public bool IsCaughtAll;
    public int countAnimal;
    public static AllManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<AllManager>();
        }
        return _instance;
    }
   

    private void Awake()
    {
        Debug.Log("Start");
        countAnimal = 0;
        vcam1.Priority = 1;
        vcam2.Priority = 0;

        lsCaughtAnimal.Clear();
        IsCaughtAll = false;
        animalConfig = Resources.Load<AnimalConfig>("AnimalConfig");
        this.animalManager = new AnimalManager();
        this.waveManager = new WaveManager();
        waveManager.SetupTransform(wavePrefab.transform);
        waveManager.SpawnWave(waveSpawnPos);
        animalManager.animalConfig = animalConfig;
        
    }

    private void Start()
    {
       
        StartCoroutine(SpawnAnimals());

    }
    IEnumerator SpawnAnimals()
    {
        countAnimal++;
        if (countAnimal == lsPosSpawn.Count) {
            UIManager.Instance().uiGameplay.OnSetUp();
            yield break;
        }
        animalManager.SetupTransform(animalConfig.animalList[countAnimal%2].animalPrefab.transform);
       
        yield return new WaitForSeconds(.5f);

        animalManager.SpawnAnimal(countAnimal, lsPosSpawn[countAnimal]);


        StartCoroutine(SpawnAnimals());
    }
    private void Update()
    {
        animalManager.MyUpdate();
        waveManager.MyUpdate();
    }
   
    public void CheckIfWin()
    {
        int countCaught = 0;
        while (true)
        {
            if (animalManager.animalInfoList[countCaught].isCaught)
            {
                countCaught++;
            }
            else
            {
                break;
            }
        }
    }
    public void CatchAnimal(GameObject goAnimal)
    {
        Debug.Log(goAnimal.name);
        for (int i = 0; i < animalManager.animalInfoList.Count; i++)
        {
            if (animalManager.animalInfoList[i].animalObj.gameObject == goAnimal)
            {
                goAnimal.GetComponent<BoxCollider>().enabled = false;
                animalManager.animalInfoList[i].isCaught = true;
                lsCaughtAnimal.Add(goAnimal);
                PlayerManager.Instance().scanner.SetActive(false);
                if (i==countAnimal - 1)
                {
                    Debug.Log("Caught");
                    IsCaughtAll = true;
                }
                break;
            }
        }
    }
    public void ChangeCamera(int a)
    {
        if(a == 1)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
    }
    private void LateUpdate()
    {
        
    }
  
}
