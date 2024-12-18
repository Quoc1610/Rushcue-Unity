using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/MapConfig")]
[System.Serializable]
public class MapConfig : ScriptableObject
{
    public List<GameObject> goMapWay;
    public List<MapElement> lsMapElement;
}
[System.Serializable]
public class MapElement
{
    public List<GameObject> lsObstacles;
    public List<GameObject> lsObstaclesSmall;
    public List<GameObject> lsObstaclesMovable;
    public List<Vector3> lsTransformMoveable;

    public int difficulty;
}
