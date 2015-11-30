using UnityEngine;
using System.Collections;

public class BeamController : MonoBehaviour {

    private MeshRenderer beamRenderer;

    // Use this for initialization
    void Start () {
        beamRenderer = GetComponent<MeshRenderer>();
		beamRenderer.sortingLayerName = "FX";
		beamRenderer.sortingOrder = 0;
	}
	
}
