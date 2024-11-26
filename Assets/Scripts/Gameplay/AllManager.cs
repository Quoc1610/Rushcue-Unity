using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AllManager : MonoBehaviour
{
    private static AllManager _instance;
    public List<Transform> lsPosSpawn = new List<Transform>();
    public List<Vector3> lsV3PosSpawn = new List<Vector3>();
    public AnimalConfig animalConfig;
    public AnimalManager animalManager;
    public WaveManager waveManager;
    public GameObject wavePrefab;
    public Transform waveSpawnPos;
    public GameObject goWin;
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
        foreach (var a in lsPosSpawn)
        {
            lsV3PosSpawn.Add(a.position);
        }

        IsCaughtAll = false;
        this.animalManager = new AnimalManager();
        this.waveManager = new WaveManager();
        waveManager.SetupTransform(wavePrefab.transform);
        waveManager.SpawnWave(waveSpawnPos);
        animalManager.animalConfig = animalConfig;

    }

    private void Start()
    {

        
        SpawnMap(0, 1);

    }
    IEnumerator SpawnAnimals()
    {
        countAnimal++;
        if (countAnimal == lsV3PosSpawn.Count)
        {
            UIManager.Instance().uiGameplay.OnSetUp();
            yield break;
        }
        animalManager.SetupTransform(animalConfig.animalList[countAnimal % 2].animalPrefab.transform);

        yield return new WaitForSeconds(.5f);

        animalManager.SpawnAnimal(countAnimal, lsV3PosSpawn[countAnimal]);


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
        foreach (GameObject go in lsMap)
        {
            Destroy(go);
        }
        lsMap.Clear();

        if (difficulty % 2 == 0)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*4);
            GenTile(4, difficulty);
        }
        else if (difficulty % 2 == 1)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*5);
            GenTile(6, difficulty);
        }
        else if (difficulty % 3 == 2)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*6);
            GenTile(8, difficulty);
        }

    }

    public void GenTile(int length, int difficulty)
    {
        for (int i = 0; i < length; i++)
        {

            GameObject go = Instantiate(mapConfig.goMapWay[0], Vector3.zero, Quaternion.identity);
            go.transform.SetParent(goMainWay.transform);
            go.transform.localPosition = new Vector3(0, 0, i * 16);
            int bigObstacleIndex = Random.Range(0, mapConfig.lsMapElement[0].lsObstacles.Count);
            GameObject goObsBig = Instantiate(mapConfig.lsMapElement[0].lsObstacles[bigObstacleIndex], Vector3.zero, Quaternion.identity);
            if (i == 0)
            {
                goObsBig.transform.SetParent(go.transform);
                goObsBig.transform.localPosition = Vector3.zero;
            }
            else
            {
                goObsBig.transform.SetParent(go.transform);
                goObsBig.transform.localPosition = new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f));
            }

            List<Vector3> lsPos = new List<Vector3> { goObsBig.transform.localPosition };

            Vector3 posNew;
            for (int count = 0; count <= difficulty; count++)
            {
                int smallObstacleIndex = Random.Range(0, mapConfig.lsMapElement[0].lsObstaclesSmall.Count);
                GameObject goObsSmall = Instantiate(mapConfig.lsMapElement[0].lsObstaclesSmall[smallObstacleIndex], Vector3.zero, Quaternion.identity);
                goObsSmall.transform.SetParent(go.transform);

                
                do
                {
                    posNew = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                } while (!IsPositionValid(posNew, lsPos, 3f));

                goObsSmall.transform.localPosition = posNew;
                lsPos.Add(posNew);
                lsMap.Add(goObsSmall);
            }
            if (i>=4)
            {
                do
                {
                    posNew = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                } while (!IsPositionValid(posNew, lsPos, 3f));
                posNew.z = lsV3PosSpawn[lsV3PosSpawn.Count-1].z+12;
                lsV3PosSpawn.Add(posNew);
            }
            
           
            lsMap.Add(go);
            lsMap.Add(goObsBig);
        }
        StartCoroutine(SpawnAnimals());
    }


    private bool IsPositionValid(Vector3 posNew, List<Vector3> existingPos, float minDis)
    {
        foreach (var pos in existingPos)
        {
            if (Vector3.Distance(pos, posNew) < minDis)
            {
                return false;
            }
        }
        return true;
    }

}
