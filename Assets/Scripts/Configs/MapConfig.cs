using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public List<Vector3> lsTransformBig;
    public List<Vector3> lsTransformSmall;

    public int difficulty;
}
