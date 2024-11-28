using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public List<GameObject> lsMovable = new List<GameObject>();
    public GameObject goMainWay;
    public int moveMovable;
    public int difficulty;

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
        countAnimal = 0;
        moveMovable = 0;

        difficulty=UIManager.Instance().uiMainMenu.lvlMap-1;

        //float speed = 10f / 5.7f*(1+4*difficulty/9);
        float speed = 1f;
        this.animalManager = new AnimalManager();
        this.waveManager = new WaveManager();

        waveManager.SetupTransform(wavePrefab.transform, speed);
        waveManager.SpawnWave(waveSpawnPos);

        vcam1.Priority = 1;
        vcam2.Priority = 0;

        lsMovable.Clear();
        lsCaughtAnimal.Clear();

        foreach (var a in lsPosSpawn)
        {
            lsV3PosSpawn.Add(a.position);
        }

        IsCaughtAll = false;
        
      

        animalManager.animalConfig = animalConfig;

    }

    private void Start()
    {
        SpawnMap(0, difficulty);
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

        yield return new WaitForSeconds(.2f);

        animalManager.SpawnAnimal(countAnimal, lsV3PosSpawn[countAnimal]);


        StartCoroutine(SpawnAnimals());
    }
    private void Update()
    {
        animalManager.MyUpdate();
        waveManager.MyUpdate();
        if(countAnimal > 1) 
        {
            if (lsCaughtAnimal.Count>=countAnimal-1)
            {
                PlayerManager.Instance().goArrow.SetActive(false);
            }
            else
            {
                Debug.Log("Count Animal: " + countAnimal);
                Debug.Log(lsCaughtAnimal.Count);
                Vector3 direction = (animalManager.animalInfoList[lsCaughtAnimal.Count].animalObj.transform.position - PlayerManager.Instance().goArrow.transform.position).normalized;
                    PlayerManager.Instance().goArrow.transform.rotation = Quaternion.LookRotation(direction);
               

            }
        }

        if (lsMovable.Count!=0)
        {
            if (Vector3.Distance(PlayerManager.Instance().transform.position,
                lsMovable[moveMovable].transform.position)<=16f)
            {
            Debug.Log("Move");
            Debug.Log(lsMovable[moveMovable].name);
            MoveObs(lsMovable[moveMovable]);
            }
        }
    }
    public void MoveObs(GameObject goMovable)
    {
        if (goMovable.GetComponent<TreeObs>()!=null)
        {
            TreeObs treeObs = goMovable.GetComponent<TreeObs>();
            treeObs.fallTree();
        }
        else 
        {
            CarObs carObs = goMovable.GetComponent<CarObs>();
            carObs.StartMoving();
        }
       
        if (moveMovable < lsMovable.Count - 1)
        {
            moveMovable++;
        }
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

        if (difficulty % 3 == 0)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*3+8);
            GenTile(4, difficulty,1);
        }
        else if (difficulty % 3 == 1)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*5+8);
            GenTile(6, difficulty,1);
        }
        else if (difficulty % 3 == 2)
        {
            goWin.transform.localPosition = new Vector3(0, 0, 16*7+8);
            GenTile(8, difficulty,1);
        }

    }

    public void GenTile(int length, int difficulty,int moveable)
    {
        bool hasMovable = difficulty >= 3;
        int countSmallObs = 0;
        if(hasMovable)
        {
            countSmallObs = 5;
        }
        else
        {
            countSmallObs = Random.Range(1,3);
        }
        for (int i = 0; i < length; i++)
        {
            GameObject go = Instantiate(mapConfig.goMapWay[0], Vector3.zero, Quaternion.identity);
            go.transform.SetParent(goMainWay.transform);
            go.transform.localPosition = new Vector3(0, 0, i * 16);

            List<Vector3> lsPos = new List<Vector3>();

            if (hasMovable)
            {
                int randomMoveable = Random.Range(0, mapConfig.lsMapElement[moveable].lsTransformMoveable.Count);

                GameObject goMovable = Instantiate(mapConfig.lsMapElement[moveable].lsObstaclesMovable[randomMoveable],Vector3.zero, Quaternion.identity);
                goMovable.transform.SetParent(go.transform);
                goMovable.transform.localPosition = mapConfig.lsMapElement[moveable].lsTransformMoveable[randomMoveable];
                lsMovable.Add(goMovable);
            }

            int bigObstacleIndex = Random.Range(0, mapConfig.lsMapElement[moveable].lsObstacles.Count);
            GameObject goObsBig = Instantiate(mapConfig.lsMapElement[moveable].lsObstacles[bigObstacleIndex], Vector3.zero, Quaternion.identity);
            goObsBig.transform.SetParent(go.transform);
            if(i==0) goObsBig.transform.localPosition = new Vector3(0, 0, 0);
            else goObsBig.transform.localPosition =new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f));
            lsPos.Add(goObsBig.transform.localPosition);


            for (int count = 0; count <= countSmallObs; count++)
            {
                int smallObstacleIndex = Random.Range(0, mapConfig.lsMapElement[moveable].lsObstaclesSmall.Count);
                GameObject goObsSmall = Instantiate(mapConfig.lsMapElement[moveable].lsObstaclesSmall[smallObstacleIndex], Vector3.zero, Quaternion.identity);
                goObsSmall.transform.SetParent(go.transform);

                Vector3 posNew;
                if(i== 0)
                {
                    do
                    {
                        posNew = new Vector3(Random.Range(-6f, 8f), 0, Random.Range(-6f, 8f));
                    } while (!IsPositionValid(posNew, lsPos, 3f));
                }
                else
                {
                    do
                    {
                        posNew = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                    } while (!IsPositionValid(posNew, lsPos, 3f));
                }
                do
                {
                    posNew = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                } while (!IsPositionValid(posNew, lsPos, 3f));

                goObsSmall.transform.localPosition = posNew;
                lsPos.Add(posNew);
                lsMap.Add(goObsSmall);
            }

            if (i >= 4)
            {
                Vector3 posNew;
                do
                {
                    posNew = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));
                } while (!IsPositionValid(posNew, lsPos, 3f));
                posNew.z = lsV3PosSpawn[lsV3PosSpawn.Count - 1].z + 12;
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
