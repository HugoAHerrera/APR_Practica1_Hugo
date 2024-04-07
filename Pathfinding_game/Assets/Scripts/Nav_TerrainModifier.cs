using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Nav_TerrainModifier : MonoBehaviour
{

    private NavMeshModifier _meshSurface;
    // Start is called before the first frame update
    void Start()
    {
        _meshSurface = GetComponent<NavMeshModifier>();
    }

    private void OnTriggerEnter(Collider other){
        //Comprobar que el que entra es de tipo agente
        if(other.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out UnityEngine.AI.NavMeshAgent agent)){
            //Compruebo que el agente se ve afectado por el tipo de terreno
            if(_meshSurface.AffectsAgentType(agent.agentTypeID)){
                //Se divida por el coste de la zona
                agent.speed /= UnityEngine.AI.NavMesh.GetAreaCost(_meshSurface.area);
            }
        }
    }


    private void OnTriggerExit(Collider other){
        if(other.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out UnityEngine.AI.NavMeshAgent agent)){
            //Compruebo que el agente se ve afectado por el tipo de terreno
            if(_meshSurface.AffectsAgentType(agent.agentTypeID)){
                //Se divida por el coste de la zona
                agent.speed *= UnityEngine.AI.NavMesh.GetAreaCost(_meshSurface.area);
            }
        }
    }

}
