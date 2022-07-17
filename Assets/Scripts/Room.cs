using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGame
{
    public class Room : MonoBehaviour
    {
        public enum RoomType
        {
            SideWall,
            BackWall
        }

        public RoomType roomType = RoomType.SideWall;

        [SerializeField] private List<SnapPoint> _snapPoints = default;
        public List<SnapPoint> snapPoints => _snapPoints;

        public Room GenerateAdjacentRoom(SnapPoint snapPoint)
        {
            return snapPoint.GenerateAdjacentRoom();
        }

        private void OnValidate()
        {
            _snapPoints = GetComponentsInChildren<SnapPoint>().ToList();
        }
    }
}
