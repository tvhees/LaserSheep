using UnityEngine;
using System.Collections;

public class SlotController : MonoBehaviour {

	public string colour;
	public string action = "EMPTY";
    public Material[] cardMaterials = new Material[2];

    private MeshRenderer meshRenderer;

    public void SetMaterial(string sheepColour) {
        colour = sheepColour;
        meshRenderer = GetComponent<MeshRenderer>();

        if (colour == "RedSheep")
            meshRenderer.material = cardMaterials[0];
        else
            meshRenderer.material = cardMaterials[1];
    }

}
