using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AnimalInfo
{
    public Transform animalObj;
    public bool isCaught;
    public NavMeshAgent agent;
    public AnimalInfo(Transform obj)
    {
        this.animalObj = obj;
        this.isCaught = false;
        agent = obj.GetComponent<NavMeshAgent>();
    }

    public void Move(Transform pos,int index)
    {
        //jiggle a little bit
        if(isCaught)
        {
            agent.SetDestination(pos.position);
            if (agent.remainingDistance <= .8f*index)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }

    
    }

}
public class AnimalManager 
{
    public AnimalConfig animalConfig;
    public Transform animalTransform;
    public List<AnimalInfo> animalInfoList = new List<AnimalInfo>();
    public void SetupTransform (Transform animalGO )
    {
       this.animalTransform = animalGO;
    }
    //public AnimalConfig animalConfig;
    public void MyUpdate()
    {
        for (int i = 0; i < animalInfoList.Count; i++)
        {
            animalInfoList[i].Move(PlayerManager.Instance().transform,i);
        }
    }
    public void SpawnAnimal(int a,Vector3 posSpawn)
    {
       
        Transform animal = GameObject.Instantiate(animalTransform,
           posSpawn , Quaternion.identity);
        animalInfoList.Add(new AnimalInfo(animal.transform));
    }
}
