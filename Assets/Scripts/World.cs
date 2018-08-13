using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	public static World current;

	public int seed;
	public int chunkWidth = 16;
	public int chunkHeight = 20;
	public float viewRange = 5;
	public GameObject chunkPrfb;

	private List<Chunk> chunks;
	private Transform player;

	private float time;

	// ==========================
	void Awake() {
		World.current = this;
	}
	// ==========================
	void Start () {
		if(this.seed == 0){
			this.seed = Random.Range(0, int.MaxValue);
		}
		this.player = GameObject.FindGameObjectWithTag("Player").transform;
		this.player.position = new Vector3(0,chunkHeight+2,0);
		this.chunks = new List<Chunk>();

		this.time = 1;
	}
	// ==========================
	void Update () {
		this.time += Time.deltaTime;

		if(this.time > 1){
			this.time = 0;
			CreateChunks();
		}
	}
	// ==========================
	private void CreateChunks(){
		float maxDistance = this.viewRange * this.chunkWidth;
		Chunk chunk;
		Vector3 position = new Vector3();
		Vector3 playerPos = this.player.transform.position;

		for(float x = (playerPos.x - maxDistance);
		    x < (playerPos.x + maxDistance);x += this.chunkWidth){
			for(float z = (playerPos.z - maxDistance);
			    z < (playerPos.z + maxDistance);z += this.chunkWidth){

				position.x = ((int)(x / this.chunkWidth)) * this.chunkWidth;
				position.z = ((int)(z / this.chunkWidth)) * this.chunkWidth;

				chunk = this.FindChunk(position);
				if(chunk == null){
					Instantiate(this.chunkPrfb, position, Quaternion.identity);
				}
			}
		}

	}
	// ==========================
	public void AddChunk(Chunk chunk){
		this.chunks.Add(chunk);
	}
	// =====================================
	public Chunk FindChunk(Vector3 position){
		Vector3 aux;
		Chunk chunk = null;
		bool founded = false;
		
		for (int i = 0; (i < this.chunks.Count) && (!founded);i++){
			aux = this.chunks[i].transform.position;
			
			if( ( aux.x <= position.x) && (aux.z <= position.z) &&
			   (aux.x + this.chunkWidth > position.x) &&
			   (aux.z + this.chunkWidth > position.z)){
				chunk = this.chunks[i];
				founded = true;
			}
		}
		return chunk;
	}
}
