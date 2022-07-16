using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 1.0f;
        
        

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<PlayerInput>().onActionTriggered += HandleAction;
        
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}