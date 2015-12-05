using UnityEngine;
using System.Collections;

public class SlotController : MonoBehaviour {

	public bool redCard;
	public string action = "EMPTY";
    public Material[] cardMaterials = new Material[2];

    private MeshRenderer meshRenderer;

    public void SetMaterial(bool redPlayer) {
        meshRenderer = GetComponent<MeshRenderer>();

        if (redPlayer) {
			redCard = true;
			meshRenderer.material = cardMaterials [0];
		}
		else {
			redCard = false;
			meshRenderer.material = cardMaterials [1];
		}
    }

	public void ResetSlot(){
		if(transform.childCount > 0)
			Destroy (transform.GetChild (0).gameObject);

		action = "EMPTY";
	}

	public IEnumerator ResolveSlot(){

		DisplayCardController childScript = transform.GetChild (0).GetComponent<DisplayCardController> ();

		yield return StartCoroutine(childScript.ResolveCard (redCard));
	}
}
