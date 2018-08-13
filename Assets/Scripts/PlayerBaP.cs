using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBaP : MonoBehaviour {
	public float range;
	public LayerMask whatToHit;
	public Image blockImage;
	public Sprite[] blockSprites;

	private RaycastHit hit;
	public byte blockInHand;
	// ==============================
	void Start () {
		this.blockInHand = 1;
		this.blockImage.sprite = this.blockSprites[this.blockInHand-1];
	}
	// ==============================
	void Update () {

//		if(Input.GetAxis("Mouse ScrollWheel") != 0){
//			if(Input.GetAxis("Mouse ScrollWheel") > 0){
//				if(this.blockInHand < 16){
//					this.blockInHand++;
//				}else{
//					this.blockInHand = 1;
//				}
//			}else{
//				if(this.blockInHand > 1){
//					this.blockInHand--;
//				}else{
//					this.blockInHand = 16;
//				}
//			}
//			this.blockImage.sprite = this.blockSprites[this.blockInHand-1];
//		}
//			if(Input.GetMouseButtonDown(0)){
//				this.MakeRayCast();
//				if(this.hit.collider != null){
//					Chunk chunk = this.hit.collider.gameObject.GetComponent<Chunk>();
//
//					Vector3 blockPos = (this.hit.point - (this.hit.normal/2)) - chunk.transform.position;
//
//					blockPos.x = Mathf.Floor(blockPos.x);
//					blockPos.y = Mathf.Floor(blockPos.y);
//					blockPos.z = Mathf.Floor(blockPos.z);
//
//					chunk.SetBlock(blockPos, 0);
//					chunk.GenerateVisualMesh();
//				}
//			
//			}else if(Input.GetMouseButtonDown(1)){
//				this.MakeRayCast();
//				if(this.hit.collider != null){
//					Chunk chunk = this.hit.collider.gameObject.GetComponent<Chunk>();
//					
//					Vector3 blockPos = (this.hit.point + (this.hit.normal/2)) - chunk.transform.position;
//					
//					blockPos.x = Mathf.Floor(blockPos.x);
//					blockPos.y = Mathf.Floor(blockPos.y);
//					blockPos.z = Mathf.Floor(blockPos.z);
//					
//					chunk.SetBlock(blockPos, this.blockInHand);
//					chunk.GenerateVisualMesh();
//				}
//			}
	}
//	 ==============================
	void MakeRayCast(){
		Physics.Raycast(this.transform.position, this.transform.forward, 
		                out hit, this.range, this.whatToHit);

	}

	public void OnClick(){
			if(this.blockInHand < 16){
									this.blockInHand++;
								}else{
									this.blockInHand = 1;
								}
		this.blockImage.sprite = this.blockSprites[this.blockInHand-1];
	}

	public void PlaceBlock(){
		this.MakeRayCast();
		if(this.hit.collider != null){
			Chunk chunk = this.hit.collider.gameObject.GetComponent<Chunk>();
			
			Vector3 blockPos = (this.hit.point + (this.hit.normal/2)) - chunk.transform.position;
			
			blockPos.x = Mathf.Floor(blockPos.x);
			blockPos.y = Mathf.Floor(blockPos.y);
			blockPos.z = Mathf.Floor(blockPos.z);
			
			chunk.SetBlock(blockPos, this.blockInHand);
			chunk.GenerateVisualMesh();
		}
	}

	public void DestroyBlock(){
		this.MakeRayCast();
		if(this.hit.collider != null){
			Chunk chunk = this.hit.collider.gameObject.GetComponent<Chunk>();
			
			Vector3 blockPos = (this.hit.point - (this.hit.normal/2)) - chunk.transform.position;
			
			blockPos.x = Mathf.Floor(blockPos.x);
			blockPos.y = Mathf.Floor(blockPos.y);
			blockPos.z = Mathf.Floor(blockPos.z);
			
			chunk.SetBlock(blockPos, 0);
			chunk.GenerateVisualMesh();
		}
	}
	public void QuitGame(){
		Application.LoadLevel("Menu");
	}
}
