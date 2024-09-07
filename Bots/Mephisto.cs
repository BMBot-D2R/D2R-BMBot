﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MapAreaStruc;

public class Mephisto
{
    Form1 Form1_0;

    public int CurrentStep = 0;
    public bool ScriptDone = false;

    public void SetForm1(Form1 form1_1)
    {
        Form1_0 = form1_1;
    }

    public void ResetVars()
    {
        CurrentStep = 0;
        ScriptDone = false;
    }

    public void DetectCurrentStep()
    {
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DuranceOfHateLevel2) CurrentStep = 1;
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DuranceOfHateLevel3) CurrentStep = 2;
    }

    public void RunScript()
    {
        Form1_0.SetGameStatus("Mephisto Script");

        Form1_0.Town_0.ScriptTownAct = 5; //set to town act 5 when running this script

        if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
        {
            ScriptDone = true;
            return;
        }

        if (Form1_0.Town_0.GetInTown())
        {
            //Form1_0.SetGameStatus("GO TO WP");
            CurrentStep = 0;

            Form1_0.Town_0.GoToWPArea(3, 8);
        }
        else
        {
            if (CurrentStep == 0)
            {
                Form1_0.SetGameStatus("Moving to MEPHISTO");
                Form1_0.Battle_0.CastDefense();
                Form1_0.WaitDelay(15);

                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DuranceOfHateLevel2)
                {
                    CurrentStep++;
                }
                else
                {
                    DetectCurrentStep();
                    if (CurrentStep == 0)
                    {
                        Form1_0.Town_0.FastTowning = false;
                        Form1_0.Town_0.GoToTown();
                    }
                }
            }

            if (CurrentStep == 1)
            {
                //####
                if (Form1_0.PlayerScan_0.levelNo == (int)Enums.Area.DuranceOfHateLevel3)
                {
                    CurrentStep++;
                    return;
                }
                //####

                Form1_0.PathFinding_0.MoveToExit(Enums.Area.DuranceOfHateLevel3);
                CurrentStep++;
            }

            if (CurrentStep == 2)
            {
                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DuranceOfHateLevel3)
                    
                    Form1_0.SetGameStatus("Moving to Attack Pos");
                    Position MidPos = new Position { X = 17558, Y = 8068 };
                    Form1_0.WaitDelay(50);

                if (Form1_0.Mover_0.MoveToLocation(MidPos.X, MidPos.Y))
                {
                    if (CharConfig.RunningOnChar == "Sorceress")
                    {
                        Form1_0.Battle_0.SetSkillsStatic();
                        CurrentStep++;
                    }
                    else
                    {
                        CurrentStep++;
                        return;
                    }
                }
            }

            if (CurrentStep == 3)
            {
                //####
                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DuranceOfHateLevel2)
                {
                    CurrentStep = 1;
                    return;
                }
                //####

                Form1_0.Potions_0.CanUseSkillForRegen = false;
                Form1_0.SetGameStatus("KILLING MEPHISTO");
                Form1_0.MobsStruc_0.DetectThisMob("getBossName", "Mephisto", false, 200, new List<long>());
                if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Mephisto", false, 200, new List<long>()))
                {
                    if (Form1_0.MobsStruc_0.MobsHP > 0)
                    {
                        Form1_0.Battle_0.RunBattleScriptOnThisMob("getBossName", "Mephisto", new List<long>());
                    }
                    else
                        {
                            if (Form1_0.Battle_0.EndBossBattle())
                            {
                                Form1_0.Town_0.FastTowning = false;
                                Form1_0.Town_0.UseLastTP = false;
                                ScriptDone = true;

                                return;
                            }
                        }
                    }
                    else
                    {
                        Form1_0.method_1("Mephisto not detected!", Color.Red);

                        //Mephisto not detected...
                        Form1_0.ItemsStruc_0.GetItems(true);
                        if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Mephisto", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.ItemsStruc_0.GrabAllItemsForGold();
                        if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Mephisto", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.Potions_0.CanUseSkillForRegen = true;

                        if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
                        return;
                    }
                }
            }
        }
    }
