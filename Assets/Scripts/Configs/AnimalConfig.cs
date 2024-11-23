using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalConfig", menuName = "Configs/AnimalConfig")]
[System.Serializable] 
public class AnimalConfig : ScriptableObject
{
    public List<Animal> animalList = new List<Animal>();
}
[System.Serializable]
public class Animal
{
    public GameObject animalPrefab;
    public int posCaught;
}