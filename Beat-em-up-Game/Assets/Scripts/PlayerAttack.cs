﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ComboState {
    NONE,
    PUNCH_1,
    PUNCH_2,
    PUNCH_3,
    KICK_1,
    KICK_2
}

public class PlayerAttack : MonoBehaviour
{

    public CharacterController player;
    public Animator playerAnimatorController;

    private bool activateTimerToReset;

    private float default_Combo_Timer = 0.5f;
    private float current_Combo_Timer; 

    private ComboState current_Combo_State;

    void Awake() {
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        current_Combo_State = ComboState.NONE;
        current_Combo_Timer = default_Combo_Timer;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttacks(); 
        ResetComboState(); 
    }

    void ComboAttacks(){
        // JumpKick
        if (!player.isGrounded && Input.GetKeyDown(KeyCode.X)) {

            playerAnimatorController.SetTrigger(AnimationTags.KICK_JUMP_TRIGGER);
        }
        // Punch 1, Punch 2 , punch 3
        if (player.isGrounded && Input.GetKeyDown(KeyCode.Z)) {
            // comprobamos el estado del combo antes de dejarle continuar
            if (current_Combo_State == ComboState.PUNCH_3 ||
                current_Combo_State == ComboState.KICK_1 ||                     
                current_Combo_State == ComboState.KICK_2 ) {
                    return;
                }                                                                                                                                                   
                       
            current_Combo_State++;  // pasamos al siguiente estado del combo                                              
            activateTimerToReset = true;   //activamos el periodo de tiempo para el reset               
            current_Combo_Timer = default_Combo_Timer; // ponemos el temporizador en default

            if (current_Combo_State == ComboState.PUNCH_1) {
                playerAnimatorController.SetTrigger(AnimationTags.PUNCH_1_TRIGGER);
            }
            if (current_Combo_State == ComboState.PUNCH_2) {
                playerAnimatorController.SetTrigger(AnimationTags.PUNCH_2_TRIGGER);
            }
            if (current_Combo_State == ComboState.PUNCH_3) {
                playerAnimatorController.SetTrigger(AnimationTags.PUNCH_3_TRIGGER);
            }
        }

        // Kick 1 and Kick2
        if(player.isGrounded && Input.GetKeyDown(KeyCode.X)) {

            // si el current combo es Kick2 o Punch3 cancelamos combo al apretar x
            if (current_Combo_State == ComboState.KICK_2 ||
                current_Combo_State == ComboState.PUNCH_3 ) {

                return;
            }  

            // si el current combo es NONE, punch 1 o punch 2
            // entonces cambiamos el current combo al Kick 1 para iniciar el combo de piernas
            if (current_Combo_State == ComboState.NONE ||
                current_Combo_State == ComboState.PUNCH_1 ||                     
                current_Combo_State == ComboState.PUNCH_2 ) {

                current_Combo_State = ComboState.KICK_1;

            } else if (current_Combo_State == ComboState.KICK_1) {
                current_Combo_State++;
            }

            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            if (current_Combo_State == ComboState.KICK_1) {

                playerAnimatorController.SetTrigger(AnimationTags.KICK_1_TRIGGER);
            }
            if (current_Combo_State == ComboState.KICK_2) {

                playerAnimatorController.SetTrigger(AnimationTags.KICK_2_TRIGGER);
                ResetComboState();
            }
        }
    }

    void ResetComboState() {
        if(activateTimerToReset) {
            current_Combo_Timer -= Time.deltaTime;
            if (current_Combo_Timer <= 0f) {

                current_Combo_State = ComboState.NONE;
                activateTimerToReset = false;
                current_Combo_Timer = default_Combo_Timer;
            }
        }
    }
}
