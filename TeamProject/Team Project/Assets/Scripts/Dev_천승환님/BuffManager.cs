using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffManager : MonoBehaviour
{
    public Coordinate[] tmp =
    {
        new Coordinate(3.8,-2.87,46.94),
        new Coordinate(-3.47,-2.89,46.93),
        new Coordinate(-16.04,-2.87,18.78),
        new Coordinate(30.01,-6.915,20.94),
        new Coordinate(17.97,-6.89,85.4),
        new Coordinate(51.9,-2.73,67.43),
        new Coordinate(8.66,1.56,101.18),
        new Coordinate(77.19,1.26,101.31),
    };
    public List<Coordinate> Coordinates = new();

    //public GameObject[] Buff_Prefabs;

    
    void Start()
    {
        //SpawnBuff(Buff_Prefabs[0], new Vector3(1, 0, 0));
        //SpawnBuff(Buff_Prefabs[1], new Vector3(-1, 0, 0));
        Coordinates.RemoveAt(Random.Range(0, Coordinates.Count));
        Coordinates.RemoveAt(Random.Range(0, Coordinates.Count));
        for (int i = 0; i < tmp.Length; i++)
        Coordinates.Add(tmp[i]);
        for(int i = 0; i < 6; i++)
        {
            //GameObject buff_Prefabs = Buff_Prefabs[i];
            //Vector3 buff_Position = Coordinates[Random.Range(0, Coordinates.Length)].GetPosition();
            //SpawnBuff(buff_Prefabs, buff_Position);
        }
    }

    public void SpawnBuff(GameObject buff_Prefabs, Vector3 _position)
    {
        GameObject buff = Instantiate(buff_Prefabs);
        buff.transform.position = _position;
    }

    public struct Coordinate
    {
        public double x;
        public double y;
        public double z;

        public Coordinate(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /*public Vector3 GetPosition()
        {
            return new Vector3(x, y, z);
            
        }*/
    }
}
