﻿/// 
/// @file  GameManager.cs
/// @brief This script manages important game state and moves game mode.
/// 

/**
 * \mainpage Ecoris - Eco Tetris in 3D
 * 
 * \section s1 About this
 * 
 * Ecoris is the abbreviation of Eco-tetris.
 * 
 * features:
 * - play tetris in 3d
 * - gameplay with LEAP-MOTION
 * - thinking ecology in this game
 * - award the title depending on your score
 * 
 * \section s2 See Also
 * 
 * Every codes of this project are open.
 * 
 * - \link https://github.com/13J-Programmers/3J_class_project \endlink
 */

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool isCountDownMode = false;
	public bool isGamePlayMode = false;
	public int isTimesUpMode = 0; ///< 0: not started, 1: times up mode, 2: finish
	public bool isGameFinish = false;

	public string handedness = "right";
	public int lines = 0; // removed lines
	public int score = 0; // obtained score
	public float remainingTime = 180; // sec
	
	private BlockEntity blockEntity;
	private GameInfoViewer gameInfoViewer;
	private StartCanvasController startCanvas;
	private TimesUpCanvasController timesUpCanvas;

	void Awake() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
		gameInfoViewer = GameObject.Find("GameInfoViewer").GetComponent<GameInfoViewer>();
		startCanvas = GameObject.Find("StartCanvas").GetComponent<StartCanvasController>();
		timesUpCanvas = GameObject.Find("TimesUpCanvas").GetComponent<TimesUpCanvasController>();
	}

	// Use this for initialization
	void Start() {
		// start count down
		isCountDownMode = true;
		StartCoroutine(CountDown());
	}
	
	// game flow:
	// 
	//        |transition
	//        ∨
	//     CountDown (3,2,1,start!)
	//        |
	//        ∨
	//     GamePlay------------+
	//        |                |
	//        |times_up        |pool_overflow
	//        ∨                |
	//     TimesUp             |
	//        |                |
	//        ∨                ∨
	//     ShowResult       GameOver
	//        |                |
	//        ∨                |
	//     RestartGame <~------+
	//        |
	//        |transition
	//        ∨
	// 
	void Update() {
		// start
		if (isCountDownMode) return;
		if (!isCountDownMode && !isGamePlayMode && !isGameFinish) {
			GameStart();
		}

		// in game
		if (isGamePlayMode) {
			remainingTime -= Time.deltaTime;
		}

		// to finish
		if (isGamePlayMode && remainingTime <= 0) {
			gameInfoViewer.enabled = false;

			// display "Time's Up"
			if (isTimesUpMode == 0) {
				isTimesUpMode = 1;
				DisableGameModules();
				StartCoroutine(TimesUp());
			}

			if (isTimesUpMode == 1) return;
			GameFinish();
		}

		// in result
		if (!isGamePlayMode && Input.GetKey("return")) {
			RestartGame();
		}
	}

	public void GameStart() {
		isGamePlayMode = true;
		score = 0;
		remainingTime = 180;
		blockEntity.CreateRandomBlock();
	}

	public void GameOver() {
		//print("GameOver");
		FinishGameProcess();
		DisableGameModules();
		var gameoverCanvas = GameObject.Find("GameoverCanvas").GetComponent<ICanvas>();
		gameoverCanvas.ShowResult(score);
	}

	public void GameFinish() {
		//print("GameFinish");
		FinishGameProcess();
		var resultCanvas = GameObject.Find("ResultCanvas").GetComponent<ICanvas>();
		resultCanvas.ShowResult(score);
	}

	public void RestartGame() {
		if (GameObject.Find("FedeSystem")) {
			GameObject.Find("FedeSystem").GetComponent<Fade>().LoadLevel("Title", 1f);
		} else {
			Application.LoadLevel("Title");
		}
	}

	// private ------------------------------------------

	/// display number to count down.
	/// then shows image "start!".
	private IEnumerator CountDown() {
		// display number
		startCanvas.SetText("3");
		yield return new WaitForSeconds(1);
		startCanvas.SetText("2");
		yield return new WaitForSeconds(1);
		startCanvas.SetText("1");
		yield return new WaitForSeconds(1);
		startCanvas.SetText("");
		// display image
		startCanvas.SetStart();
		isCountDownMode = false;
		yield return new WaitForSeconds(1);
		startCanvas.SetStart(false);
	}

	/// display image "Times Up" while 2 seconds.
	private IEnumerator TimesUp() {
		// display image
		timesUpCanvas.SetTimesUp();
		yield return new WaitForSeconds(2);
		timesUpCanvas.SetTimesUp(false);
		isTimesUpMode = 2;
	}

	/// perform this process when the game is finished.
	private void FinishGameProcess() {
		isGamePlayMode = false;
		isGameFinish = true;
		gameInfoViewer.enabled = false;
	}

	/// stop specific game modules
	private void DisableGameModules() {
		// modules to be stopped
		string[] modules = new string[] {
			"Main Camera#CameraController", 
			"BlockEntity#BlockEntity", 
			"LeapHandAction#LeapHandAction", 
			"KeyAction#KeyAction", 
		};

		// stop modules
		foreach (string module in modules) {
			string[] tmp = module.Split('#');
			string objectName = tmp[0];
			string[] moduleNames = tmp[1].Split(',');
			foreach (string moduleName in moduleNames) {
				//print(objectName + "." + moduleName);
				Component targetModule = GameObject.Find(objectName).GetComponent(moduleName);
				Destroy(targetModule);
			}
		}
	}
}



