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
        if (countAnimal == lsPosSpawn.Count) yield break;
        animalManager.SetupTransform(animalConfig.animalList[countAnimal%2].animalPrefab.transform);
       
        yield return new WaitForSeconds(.5f);

        animalManager.SpawnAnimal(countAnimal, lsPosSpawn[countAnimal]);

        StartCoroutine(SpawnAnimals());
    }
    private void Update()
    {
        animalManager.MyUpdate();
        waveManager.MyUpdate();
        if (countAnimal == lsPosSpawn.Count)
        {
            waveManager.StartWave();
        }
    }
    public void CatchAnimal(GameObject goAnimal)
    {
        Debug.Log(goAnimal.name);
        for (int i = 0; i < animalManager.animalInfoList.Count; i++)
        {
            if (animalManager.animalInfoList[i].animalObj.gameObject == goAnimal)
            {
                animalManager.animalInfoList[i].isCaught = true;
                break;
            }
        }
    }
    private void LateUpdate()
    {
        
    }
  
}
