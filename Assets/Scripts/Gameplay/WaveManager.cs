using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo
{
    public float speed;
    public bool isMove;
    public Transform waveObj;
    public WaveInfo(float speed)
    {
        this.speed = speed;
        this.isMove = false;
        this.waveObj = waveObj;
    }
    
    public void Move()
    {
        if (isMove)
        {
            
            waveObj.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

}
public class WaveManager
{
    public Transform waveTransform;
    public WaveInfo waveInfo;
    public void SetupTransform(Transform waveGO)
    {
        float speed = 10f / 5.7f;
        waveInfo = new WaveInfo(speed);
        this.waveTransform = waveGO;
        
    }
    public void SpawnWave(Transform posSpawn)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        Transform wave = GameObject.Instantiate(waveTransform,
            posSpawn.position, rotation);
        waveInfo.waveObj = wave;
    }
    public void StartWave()
    {
        waveInfo.isMove = true;
    }
    public void MyUpdate()
    {
        waveInfo.Move();
    }
}
