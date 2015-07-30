﻿using UnityEngine;
using System.Collections;

public class ExpectDropPosViewer : MonoBehaviour {
	int POOL_X;
	int POOL_Y;
	int POOL_Z;
	BlockPoolController blockPoolControl;
	bool[,,] pool;
	GameObject controllingBlock;
	GameObject showDropPosBlock;
	bool isSync = true;

	void Awake() {
		blockPoolControl = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		controllingBlock = gameObject;
	}

	// Use this for initialization
	void Start() {
		CloneSkeltonBlock();

		POOL_X = BlockPoolController.POOL_X;
		POOL_Y = BlockPoolController.POOL_Y;
		POOL_Z = BlockPoolController.POOL_Z;
		pool = new bool[POOL_X, POOL_Y, POOL_Z];

		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (blockPoolControl.blockPool[x, y, z] == null) continue;
					pool[x, y, z] = true;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		SyncOriginBlock();
	}

	public void StopSync() {
		isSync = false;
	}

	public void StopShowing() {
		Destroy(showDropPosBlock);
	}

	// private methods --------------------------------

	private Vector3 ExpectDropPos(Vector3 pos) {
		return new Vector3(pos.x, pos.y - 20.5f, pos.z);
	}

	// clone block to create skelton it
	private void CloneSkeltonBlock() {
		Vector3 originBlockPos = controllingBlock.transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		showDropPosBlock = Instantiate(
			controllingBlock, 
			cloneBlockPos,
			controllingBlock.transform.rotation
		) as GameObject;
		showDropPosBlock.name = "block(expected-drop-pos)";

		Destroy(showDropPosBlock.GetComponent<BlockPoolController>());
		Destroy(showDropPosBlock.GetComponent<ExpectDropPosViewer>());
		Destroy(showDropPosBlock.GetComponent<BoxCollider>());
	}

	private void SetSkeltonCube(Transform block) {
		foreach (Transform cube in block.transform) {
			cube.gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
		}
	}

	private void SyncOriginBlock() {
		if (!GameObject.Find("block(expected-drop-pos)")) return;
		if (!isSync) return;

		Vector3 originBlockPos = controllingBlock.transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		if (!showDropPosBlock) return;
		showDropPosBlock.transform.localPosition = cloneBlockPos;
		showDropPosBlock.transform.rotation = controllingBlock.transform.rotation;
	}
}


