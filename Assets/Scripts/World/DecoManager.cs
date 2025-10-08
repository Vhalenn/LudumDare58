using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Sirenix.OdinInspector;

[ExecuteAlways]
public class DecoManager : MonoBehaviour
{
    [Header("Param")]
    [SerializeField] private Mesh grassMesh;
    [SerializeField] private Material grassMat;

    [Header("Storage")]
    [SerializeField] private int length;
    [SerializeField] private int matrixLength;
    [SerializeField] private DecoGrass[] decoArray;
    [SerializeField] private Matrix4x4[] matrixArray;


    [Button]
    private void GetAllMatrixChilds()
    {
        decoArray = GetComponentsInChildren<DecoGrass>();
        length = decoArray.Length;

        List<Matrix4x4> grassList = new();

        for (int i = 0; i < length; i++)
        {
            grassList.AddRange(decoArray[i].GrassMatrix);
        }

        matrixArray = grassList.ToArray();
        matrixLength = matrixArray.Length;
    }

    [Button]
    private void ToggleDebugView()
    {
        bool getBaseState = false;
        bool baseState = false;
        for (int i = 0; i < length; i++)
        {
            if (decoArray[i] == null) continue;

            if(!getBaseState)
            {
                baseState = !decoArray[i].GizmoState;
                getBaseState = true;
            }

            decoArray[i].SetGizmoState(baseState);
        }
    }

    void Update()
    {
        if (!grassMesh || !grassMat || matrixLength == 0) return;

        Graphics.DrawMeshInstanced(grassMesh, 0, grassMat, matrixArray, matrixLength, default, ShadowCastingMode.Off);
    }
}
