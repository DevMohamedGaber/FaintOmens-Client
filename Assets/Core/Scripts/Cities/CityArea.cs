using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;
namespace Game
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshSurface))]
    public class CityArea : MonoBehaviour
    {
        public int id;
        public Transform entry;
        [SerializeField] Material skybox;
        //City data => Storage.data.cities[id];
        void Start()
        {
            if(skybox != null)
            {
                RenderSettings.skybox = skybox;
            }
        }
    }
}