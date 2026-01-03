using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPositionData : BlackBoardData 
{
    public override BlackBoardKey BlackBoardKey 
    {
        get { return BlackBoardKey.AttackPosition; }
    }
        
    public string PointId { get; set; }
    public Transform Point { get; set; }
    public Vector3 Position { get; set; }

    public override string ToString()
    {
        return string.Format(
            "{{ BlackBoardKey = {0}, PointId = {1} , Point = {2}, Position = {3} }}",
            BlackBoardKey, PointId, Point, Position
        );
    }
}
