using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BuffManager : MonoBehaviour
{
    
    public Coordinate[] tmp =
    {
        new Coordinate(3.8f,-2.87f,46.94f),
        new Coordinate(-3.47f,-2.89f,46.93f),
        new Coordinate(-16.04f,-2.87f,18.78f),
        new Coordinate(30.01f,-6.915f,20.94f),
        new Coordinate(17.97f,-6.89f,85.4f),
        new Coordinate(51.9f,-2.73f,67.43f),
        new Coordinate(8.66f,1.56f,101.18f),
        new Coordinate(77.19f,1.26f,101.31f),
    };
    public List<Coordinate> Coordinates = new();
    

    public GameObject[] Buff_Prefabs;

    
    void Start()
    {
        
        //SpawnBuff(Buff_Prefabs[0], new Vector3(1, 0, 0));
        //SpawnBuff(Buff_Prefabs[1], new Vector3(-1, 0, 0));
        for (int i = 0; i < tmp.Length; i++)
        {
            Coordinates.Add(tmp[i]);
        }

        

        for (int i = 0; i < 6; i++)
        
        {
            int tmp = Random.Range(0, Coordinates.Count);
            SpawnBuff(Buff_Prefabs[i], Coordinates[tmp].GetPosition());
            Coordinates.RemoveAt(tmp);
        }
    }

    public void SpawnBuff(GameObject buff_Prefabs, Vector3 _position)
    {
        GameObject buff = Instantiate(buff_Prefabs);
        buff.transform.position = _position;
    }

    public struct Coordinate
    {
        public float x;
        public float y;
        public float z;

        public Coordinate(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(x, y, z);
            
        }
    }
}
