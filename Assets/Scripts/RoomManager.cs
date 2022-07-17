using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeonGame
{
    public class RoomManager : MonoBehaviour
    {


        public List<Room> _allRooms = default;
        public Room _staringRoom = default;

        private Dictionary<Room.RoomType, List<Room>> _roomsByType;

        private List<Room> _spawnedRooms = new List<Room>();

        private void Awake()
        {
            _roomsByType = new Dictionary<Room.RoomType, List<Room>>();
            for (int i = 0; i < _allRooms.Count; i++)
            {
                var r = _allRooms[i];
                if (!_roomsByType.ContainsKey(r.roomType))
                {
                    _roomsByType.Add(r.roomType, new List<Room>());
                }

                _roomsByType[r.roomType].Add(r);
            }

        }

        private void Start()
        {
            RegisterRoom(_staringRoom);
        }

        public List<Room> GetAllRoomsOfType(Room.RoomType t)
        {
            if (_roomsByType.ContainsKey(t))
            {
                return _roomsByType[t];
            }
            else
            {
                return null;
            }
        }

        void RegisterRoom(Room room)
        {
            foreach(SnapPoint s in room.snapPoints)
            {
                s.onSpawnRequired += OnSpawnRequired;
            }
        }

        void UnregisterRoom(Room room)
        {
            foreach (SnapPoint s in room.snapPoints)
            {
                s.onSpawnRequired -= OnSpawnRequired;
            }

            _spawnedRooms.Remove(room);
        }

        public void RemoveRoom(Room room)
        {
            UnregisterRoom(room);
            Destroy(room.gameObject);
        }

        private void OnSpawnRequired(SnapPoint s)
        {
            s.onSpawnRequired -= OnSpawnRequired;


            Room r = s.GenerateAdjacentRoom();

            if(r)
            {
                _spawnedRooms.Add(r);
                RegisterRoom(r);
            }
        }
    }
}
