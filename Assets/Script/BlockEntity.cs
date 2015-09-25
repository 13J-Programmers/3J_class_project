﻿/// 
/// @file  BlockEntity.cs
/// @brief This script is for create block object instance.
/// 

using UnityEngine;
using System.Collections;
using Player.Action;

public class BlockEntity : MonoBehaviour {
	public const int prefabMaxNum = 18;
	/// block prefabs
	public GameObject[] blocks = new GameObject[prefabMaxNum];
	public int nextBlockNum;
	public int currentBlockNum;
	private Queue queue = new Queue();

	public int GetPrefabMaxNum() { return prefabMaxNum; }

	/// BlockEntity methods are invoked from Start() in GameManager.
	/// therefore, initializing variables have to write in Awake().
	void Awake() {
		PushNextBlock(RandomBlock());
	}

	/// Use this for initialization
	void Start() {
		// change every cube of block
		BoxCollider bc;
		Vector3 colliderSize = new Vector3(0.95f, 0.95f, 0.95f);
		for (int i = 0; i < prefabMaxNum; i++) {
			bc = blocks[i].GetComponent<BoxCollider>();
			bc.size = colliderSize;
			foreach (Transform cube in blocks[i].transform) {
				bc = cube.gameObject.GetComponent<BoxCollider>();
				bc.size = colliderSize;
			}
		}
	}

	public void CreateRandomBlock() {
		// shift new block list
		GameObject randBlock = ShiftNextBlock();
		PushNextBlock(RandomBlock());

		// create new block
		GameObject newBlock = Instantiate(
			randBlock, // instance object
			new Vector3(0, 10, 0), // coordinate
			randBlock.transform.rotation // rotation
		) as GameObject;

		newBlock.name = "block(new)";
		newBlock.AddComponent<BlockController>();
		newBlock.AddComponent<ExpectDropPosViewer>();
		SetScoreToCube(newBlock);

		ConnectBlockWithUserAction();
	}

	// private methods ------------------------------

	private GameObject RandomBlock() {
		int randNum = Random.Range(0, this.GetPrefabMaxNum());
		return blocks[randNum];
	}

	private void PushNextBlock(GameObject gameObject) {
		queue.Enqueue(gameObject);
	}
	private GameObject ShiftNextBlock() {
		return (GameObject)queue.Dequeue();
	}

	/// set score to menber of CubeInfo component
	private void SetScoreToCube(GameObject newBlock) {
		CubeInfo cubeInfo;
		cubeInfo = newBlock.AddComponent<CubeInfo>();
		cubeInfo.score = 20;
		foreach (Transform t in newBlock.transform) {
			cubeInfo = t.gameObject.AddComponent<CubeInfo>();
			cubeInfo.score = 20;
		}
	}

	/// connect user action and block
	private void ConnectBlockWithUserAction() {
		GameObject.Find("KeyAction").GetComponent<KeyAction>().ConnectWithBlock();
		GameObject.Find("LeapHandAction").GetComponent<LeapHandAction>().ConnectWithBlock();
	}
}
