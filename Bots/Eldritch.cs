﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;
using static MapAreaStruc;

public class Eldritch
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
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.FrigidHighlands) CurrentStep = 1;
    }

    public void RunScript()
    {
        Form1_0.Town_0.ScriptTownAct = 5; //set to town act 5 when running this script

        if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
        {
            ScriptDone = true;
            return;
        }

        if (Form1_0.Town_0.GetInTown())
        {
            Form1_0.Battle_0.CastDefense();
            Form1_0.SetGameStatus("GO TO WP");
            CurrentStep = 0;

            Form1_0.Town_0.GoToWPArea(5, 1);
        }
        else
        {
            if (CurrentStep == 0)
            {
                Form1_0.SetGameStatus("FHwp");
                Form1_0.Battle_0.CastDefense();
                Form1_0.WaitDelay(15);

                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.FrigidHighlands)
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
                Form1_0.SetGameStatus("Moving to Attack Pos");
                Position MidPos = new Position { X = 3737, Y = 5065 };
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

            if (CurrentStep == 2)
            {
                Form1_0.Potions_0.CanUseSkillForRegen = false;
                Form1_0.SetGameStatus("KILLING ELDRITCH");
                Form1_0.MobsStruc_0.DetectThisMob("getSuperUniqueName", "Eldritch", false, 200, new List<long>());
                if (Form1_0.MobsStruc_0.GetMobs("getSuperUniqueName", "Eldritch", false, 200, new List<long>()))
                {
                    if (Form1_0.MobsStruc_0.MobsHP > 0)
                    {
                        Form1_0.Battle_0.SetSkills();
                        Form1_0.Battle_0.CastSkills();
                        return;
                    }
                    if (Form1_0.MobsStruc_0.MobsHP < 1)
                    {
                        Form1_0.PathFinding_0.MoveToThisPos(new Position { X = 3772, Y = 5109 });
                        {
                            ScriptDone = true;
                            return;
                        }
                    }
                }
                /*else
                {
                    if (Form1_0.MobsStruc_0.MobsHP <= 1)

                    {
                        ScriptDone = true;

                        /*Form1_0.method_1("Eldritch not detected!", Color.Red);

                        //Eldritch not detected...
                        Form1_0.ItemsStruc_0.GetItems(true);
                        if (Form1_0.MobsStruc_0.GetMobs("getSuperUniqueName", "Eldritch", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.ItemsStruc_0.GrabAllItemsForGold();
                        if (Form1_0.MobsStruc_0.GetMobs("getSuperUniqueName", "Eldritch", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.Potions_0.CanUseSkillForRegen = true;

                        if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
                        return;
                    }
                }*/
                
            }
        }
    }
}
