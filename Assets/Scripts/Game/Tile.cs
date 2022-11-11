using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public int tileValue;
	public bool destroy = false;
	public bool isMergedThisTurn;
	public bool willMergeWithCollidingTile;
	public Vector3 movePosition;
	public Vector3 startingPosition;
	public Transform collidingTile;
}
