using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float abilityBaseCooldown = 0.25f;

    [Header("Move State")]
    public float maxMoveSpeed = 10f;
    public float acceleration = 20f;
    public float decceleration = 20f;

    [Header("Jump State")]
    public float jumpForce = 15f;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float jumpCutMultiplier = 0.4f;
    public float waterForceDown = 2f;

    [Header("Dash Under State")]
    public float dashUnderCooldown = 0.5f;
    public float dashUnderTime = 0.2f;
    public float dashUnderForce = 30f;
    public float dashUnderDrag = 10f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("Looking For Haunt State")]
    public float lookingForHauntMaxDistance = 20f;
    public float lookingForFauntCooldown = 0.5f;
    public float maxHoldTime = 3f;
    public float holdInputTimeScale = 0.25f;
    public int increaseDistancePerUpgrade = 1;

    [Header("Grounded state")]
    public float floatHeight = 1f;

    [Header("Haunting State")]
    public float holdHauntingCancelInputTimer = 1.5f;
    public float forceJumpOutHaunting = 10f;

    [Header("Look For Clues State")]
    public float maxHoldTimeLookForClues = 5f;

    [Header("Fly State")]
    public float maxHoldFlyTime = 1.5f;

    [Header("Fear Attack State")]
    public float lookForFearRange = 7f;

    [Header("Player Damaged State")]
    public Vector2 damagedKnockbackAngle = new Vector2(0.1f, 0.6f);
    public float damagedKnockbackForce = 5f;

    [Header("Follow Waypoints State")]
    public float followWaypointsSpeed = 15f;

    [Header("Player Size")]
    public float height = 1f;
    public float length = 1f;

    [Header("Healing State")]
    public float healCooldown = 0.5f;

    [Header("New Found Knowledge State")]
    public float cameraShakeAmplitude = 1.5f;
    public float cameraShakeFrequency = 1.5f;
    public string firstTimeFoundKnowledgeString = "You may access all Knowledges anytime by pressing the select menu";

    [Header("New Item Found State")]
    public string firstTimeItemFoundString = "You may access all Knowledges anytime by pressing the select menu";
}
