
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DungeonGame
{
    public class SnapPoint : MonoBehaviour
    {
        public System.Action<SnapPoint> onSpawnRequired = delegate {};

        [SerializeField] Room.RoomType _RoomType = Room.RoomType.SideWall;
        [SerializeField] GameObject _blockingGeometry;
        [SerializeField] ExplodableWall _explodableWall;

        public Room.RoomType roomType => _RoomType;
        public GameObject blockingGeometry => _blockingGeometry;
        public ExplodableWall explodableWall => _explodableWall;

        private void Start()
        {
            if(_explodableWall == null)
            {
                _explodableWall = GetComponentInChildren<ExplodableWall>();
                    }

            _explodableWall.onExplode += OnWallExplode;
        }

        private void OnDestroy()
        {
            _explodableWall.onExplode -= OnWallExplode;
        }

        private void OnWallExplode(ExplodableWall obj)
        {
            _explodableWall.onExplode -= OnWallExplode;

            onSpawnRequired(this);
        }

        public Room GenerateAdjacentRoom()
        {
            var roomManager = FindObjectOfType<RoomManager>();
            var validRooms = roomManager.GetAllRoomsOfType(roomType);

            if (validRooms != null)
            {
                var randomRoomPrefab = validRooms[Random.Range(0, validRooms.Count)];

                var roomInstance = Instantiate<Room>(randomRoomPrefab);
                roomInstance.transform.SetParent(roomManager.transform);

                roomInstance.transform.position = transform.position;
                roomInstance.transform.rotation = transform.rotation;

                return roomInstance;
            }
            else
            {
                Debug.Log("No valid rooms to generate");
                return null;
            }
        }
    }
}