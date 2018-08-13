using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]

public class Chunk : MonoBehaviour {
	private byte[,,] blocks;

	private MeshFilter meshFilter;
	private MeshCollider meshCollider;

	// ==============================
	void Start () {
		World.current.AddChunk(this);
		this.meshFilter = GetComponent<MeshFilter>();
		this.meshCollider = GetComponent<MeshCollider>();

		this.blocks = new byte[World.current.chunkWidth,
		            			World.current.chunkHeight,
		                       World.current.chunkWidth];

		this.GenerateStruct();
		this.GenerateVisualMesh();
	}
	// ==============================
	void GenerateStruct(){
		float noiseValue;
		float stoneValue;

		Vector3 noisePos = new Vector3();
		Random.seed = World.current.seed;

		Vector3 offset = new Vector3(
			100000 * Random.value,
			100000 * Random.value,
			100000 * Random.value
			);

		for(int x = 0;x < World.current.chunkWidth;x++){
			for(int y = 0;y < World.current.chunkHeight-10;y++){
				for(int z = 0;z < World.current.chunkWidth;z++){
					noisePos.Set(
						x + transform.position.x,
						y + transform.position.y,
						z + transform.position.z
						);

					stoneValue = this.GenerateNoise(noisePos, offset, 100f);

					float tmp = (10 - y) / 10f;
					stoneValue += tmp;

					noiseValue = this.GenerateNoise(noisePos, offset, 35f);

					tmp = (40 - y) / 40f;
					noiseValue += tmp;

					noiseValue += stoneValue;

					if(stoneValue > 0.2){
						this.blocks[x,y,z] = 2;
					}else if(noiseValue > 0.2){
						this.blocks[x,y,z] = 1;
					}
				}
			}
		}
		bool founded;
		for(int x = 0;x < World.current.chunkWidth;x++){
			for(int z = 0;z < World.current.chunkWidth;z++){
				founded = false;
				for(int y = World.current.chunkHeight - 1; (y > 0 && !founded);y--){
					if(this.blocks[x,y,z] != 0){
						if(this.blocks[x,y,z] == 1){
							this.blocks[x,y,z] = 8;
						}
						founded = true;
					}
				}
			}
		}
	
	}
	// ==============================
	float GenerateNoise(Vector3 position, Vector3 offset, float scale){
		float nX = (position.x + offset.x) / scale;
		float nY = (position.y + offset.y) / scale;
		float nZ = (position.z + offset.z) / scale;

		return Noise.Generate(nX, nY, nZ);
	}
	// ==============================
	public void SetBlock(Vector3 pos, byte block){
		if (!( (pos.x < 0) || (pos.y < 0) || (pos.z < 0) ||
		   (pos.x >= World.current.chunkWidth) ||
		   (pos.y >= World.current.chunkHeight) ||
		   (pos.z >= World.current.chunkWidth))){
			this.blocks[ ((int)pos.x), ((int)pos.y), ((int)pos.z)] = block;
		}
	}
	// ==============================
	public byte GetBlock(int x, int y, int z){
		if( (x < 0) || (y < 0) || (z < 0) ||
		   (x >= World.current.chunkWidth) ||
		   (y >= World.current.chunkHeight) ||
		   (z >= World.current.chunkWidth)){
			return 0;
		}else{
			return this.blocks[x,y,z];
		}
	}
	// ==============================
	bool IsTransparent(int x, int y, int z){
		byte block = this.GetBlock(x,y,z);

		if(block == 0){
			return true;
		}else{
			return false;
		}
	}
	// ==============================

	public void GenerateVisualMesh(){
		Mesh mesh = new Mesh();
		mesh.name = "Chunk";

		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();

		for(int x = 0;x < World.current.chunkWidth;x++){
			for(int y = 0;y < World.current.chunkHeight;y++){
				for(int z = 0;z < World.current.chunkWidth;z++){
					if(this.blocks[x,y,z] != 0){
						// left
						if(this.IsTransparent(x-1, y, z)){
							this.CreateFace(this.GetBlock(x,y,z), new Vector3(x,y,z), Vector3.up, Vector3.forward,true, verts, uvs, tris);
						}
						// Right
						if(this.IsTransparent(x+1, y, z)){
							this.CreateFace(this.GetBlock(x,y,z), new Vector3(x+1,y,z), Vector3.up, Vector3.forward,false, verts, uvs, tris);
						}
						// up
						if(this.IsTransparent(x, y+1, z)){
							this.CreateFace(this.GetBlock(x,y,z),new Vector3(x,y+1,z), Vector3.forward, Vector3.right,false, verts, uvs, tris);
						}
						// down
						if(this.IsTransparent(x, y-1, z) && (y > 0)){
							this.CreateFace(this.GetBlock(x,y,z),new Vector3(x,y,z), Vector3.forward, Vector3.right,true, verts, uvs, tris);
						}
						// front
						if(this.IsTransparent(x, y, z-1)){
							this.CreateFace(this.GetBlock(x,y,z),new Vector3(x,y,z), Vector3.up, Vector3.right,false, verts, uvs, tris);
						}
						// back
						if(this.IsTransparent(x, y, z+1)){
							this.CreateFace(this.GetBlock(x,y,z),new Vector3(x,y,z+1), Vector3.up, Vector3.right,true, verts, uvs, tris);
						}
					}
				}
			}

		}

		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		this.meshCollider.sharedMesh = mesh;

		this.meshFilter.mesh = mesh;
		this.meshFilter.sharedMesh = mesh;
	}
	// ==============================
	void CreateFace(byte block, Vector3 corner, Vector3 up, Vector3 side, bool reversed,
	                List<Vector3> verts, List<Vector2> uvs,
	                List<int> tris){
		int index = verts.Count;

		verts.Add(corner);
		verts.Add(corner + up);
		verts.Add(corner + up + side);
		verts.Add(corner + side);

		int x;
		int y;

		int tilling = 4;
		float uvWidth = 1f / tilling;

		x = block % tilling;
		y = block / tilling;

		if(x == 0){
			x = tilling;
		}else{
			y++;
		}

		uvs.Add(new Vector2(x * uvWidth, uvWidth * (tilling-y)));
		uvs.Add(new Vector2(x * uvWidth, (uvWidth * (tilling-y)) + uvWidth));

		uvs.Add(new Vector2((x * uvWidth) - uvWidth, (uvWidth * (tilling-y)) + uvWidth));
		uvs.Add(new Vector2((x * uvWidth) - uvWidth, uvWidth * (tilling-y)));

		if(!reversed){
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}else{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
	}
	// ==============================
	void Update () {
	
	}
}
