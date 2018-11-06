using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SO
{
    public class AIManager : MonoBehaviour
    {

        NavMeshAgent agent;
        StatesManager states;
        
        
        void Start()
        {
            states = GetComponent<StatesManager>();
            states.Init();

            agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            
        }
        
        void Update()
        {

        }
    }
}