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
    public MapConfig mapConfig;
    public List<GameObject> lsMap = new List<GameObject>();
    public List<GameObject> lsCaughtAnimal = new List<GameObject>();
    public GameObject goMainWay;

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
        this.animalManager = new AnimalManager();
        this.waveManager = new WaveManager();
        waveManager.SetupTransform(wavePrefab.transform);
        waveManager.SpawnWave(waveSpawnPos);
        animalManager.animalConfig = animalConfig;

    }

    private void Start()
    {

        StartCoroutine(SpawnAnimals());
        SpawnMap(0, 0);

    }
    IEnumerator SpawnAnimals()
    {
        countAnimal++;
        if (countAnimal == lsPosSpawn.Count)
        {
            UIManager.Instance().uiGameplay.OnSetUp();
            yield break;
        }
        animalManager.SetupTransform(animalConfig.animalList[countAnimal % 2].animalPrefab.transform);

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
                if (i == countAnimal - 1)
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
        if (a == 1)
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
    public void SpawnMap(int mapIndex, int difficulty)
    {
        foreach(GameObject go in lsMap)
        {
            Destroy(go);
        }
        lsMap.Clear();

        if (difficulty == 0)
        {
            GenTile(4,mapIndex);
        }
        else if (difficulty == 1)
        {
            GenTile(6, mapIndex);
        }
        else if (difficulty == 2)
        {
            GenTile(8, mapIndex);
        }

    }
    public void GenTile(int length,int mapIndex)
    {
        int a, b, c, d;

        for (int i = 0; i < length; i++)
        {
            Debug.Log("Spawn");
            GameObject go = Instantiate(mapConfig.goMapWay[mapIndex],Vector3.zero, Quaternion.identity);
            go.transform.SetParent(goMainWay.transform);
            go.transform.localPosition = new Vector3(0, 0, i * 16);

            //spawn obstacle
            a = Random.Range(0, mapConfig.lsMapElement[mapIndex].lsObstacles.Count);
            b = Random.Range(0, mapConfig.lsMapElement[mapIndex].lsObstaclesSmall.Count);
            c = Random.Range(0, mapConfig.lsMapElement[mapIndex].lsTransformBig.Count);
            d = Random.Range(0, mapConfig.lsMapElement[mapIndex].lsTransformSmall.Count);

            GameObject goObsBig = Instantiate(mapConfig.lsMapElement[mapIndex].lsObstacles[a], Vector3.zero, Quaternion.identity);
            goObsBig.transform.SetParent(go.transform);
            goObsBig.transform.localPosition = mapConfig.lsMapElement[mapIndex].lsTransformBig[c];


            GameObject goObsSmall = Instantiate(mapConfig.lsMapElement[mapIndex].lsObstaclesSmall[b], Vector3.zero, Quaternion.identity);
            goObsSmall.transform.SetParent(go.transform);
            goObsSmall.transform.localPosition = mapConfig.lsMapElement[mapIndex].lsTransformSmall[d];

            lsMap.Add(go);
            lsMap.Add(goObsBig);
            lsMap.Add(goObsSmall);

        }
    }
}
