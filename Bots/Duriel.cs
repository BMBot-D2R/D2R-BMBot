﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.MapAreaStruc;

namespace app
{
    public class Duriel
    {
        Form1 Form1_0;

        public int CurrentStep = 0;
        public bool ScriptDone = false;
        public Position OrificePos = new Position { X = 0, Y = 0 };


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
            if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.CanyonOfTheMagi) CurrentStep = 1;
            if ((int) Form1_0.PlayerScan_0.levelNo >= (int)Enums.Area.TalRashasTomb1 && Form1_0.PlayerScan_0.levelNo <= (int)Enums.Area.TalRashasTomb7)
            {
                CurrentStep = 1; //return to step1 anyway!
            }
            if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DurielsLair) CurrentStep = 3;
        }

        public void RunScript()
        {
            Form1_0.Town_0.ScriptTownAct = 2; //set to town act 5 when running this script

            if (Form1_0.Town_0.GetInTown())
            {
                Form1_0.SetGameStatus("GO TO WP");
                CurrentStep = 0;

                Form1_0.Town_0.GoToWPArea(2, 8);
            }
            else
            {
                if (CurrentStep == 0)
                {
                    Form1_0.SetGameStatus("DOING DURIEL");
                    Form1_0.Battle_0.CastDefense();
                    Form1_0.WaitDelay(15);

                    if ((Enums.Area) Form1_0.PlayerScan_0.levelNo == Enums.Area.CanyonOfTheMagi)
                    {
                        CurrentStep++;
                    }
                    else
                    {
                        DetectCurrentStep();
                        if (CurrentStep == 0) Form1_0.Town_0.GoToTown();
                    }
                }

                if (CurrentStep == 1)
                {
                    //####
                    if (Form1_0.PlayerScan_0.levelNo >= (int)Enums.Area.TalRashasTomb1 && Form1_0.PlayerScan_0.levelNo <= (int)Enums.Area.TalRashasTomb7)
                    {
                        CurrentStep++;
                        return;
                    }
                    //####

                    //id":152, "type":"object", "x":453, "y":258, "name":"orifice", "op":25, "class":"quest"}
                    //Detect the correct tomb where Duriel hide
                    OrificePos = Form1_0.MapAreaStruc_0.GetAreaOfObject("object", "HoradricOrifice", new List<int>(), 65, 72);
                    if (OrificePos.X != 0 &&  OrificePos.Y != 0)
                    {
                        //"id":71, "type":"exit", "x":214, "y":25, "isGoodExit":true}
                        //Form1_0.method_1("Moving to: " + ((Enums.Area)(Form1_0.MapAreaStruc_0.CurrentObjectAreaIndex + 1)), Color.Red);
                        Position ThisFinalPosition = Form1_0.MapAreaStruc_0.GetPositionOfObject("exit", Form1_0.Town_0.getAreaName((int)Form1_0.MapAreaStruc_0.CurrentObjectAreaIndex + 1), (int) Form1_0.PlayerScan_0.levelNo, new List<int>() { });
                        if (Form1_0.Mover_0.MoveToLocation(ThisFinalPosition.X, ThisFinalPosition.Y))
                        {
                            int Tryyyy = 0;
                            while (Form1_0.PlayerScan_0.levelNo == 46 && Tryyyy <= 25)
                            {
                                Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisFinalPosition.X, ThisFinalPosition.Y);
                                
                                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y);
                                Form1_0.PlayerScan_0.GetPositions();
                                Tryyyy++;
                            }
                            //didn't clic correctly on tomb door, substract some pixels
                            Tryyyy = 0;
                            while (Form1_0.PlayerScan_0.levelNo == 46 && Tryyyy <= 25)
                            {
                                Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisFinalPosition.X, ThisFinalPosition.Y);
                                
                                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X - 70, itemScreenPos.Y);
                                Form1_0.PlayerScan_0.GetPositions();
                                Tryyyy++;
                            }
                            //didn't clic correctly on tomb door, substract some pixels
                            Tryyyy = 0;
                            while (Form1_0.PlayerScan_0.levelNo == 46 && Tryyyy <= 25)
                            {
                                Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisFinalPosition.X, ThisFinalPosition.Y);
                                
                                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X + 70, itemScreenPos.Y);
                                Form1_0.PlayerScan_0.GetPositions();
                                Tryyyy++;
                            }

                            Form1_0.PathFinding_0.MoveToThisPos(OrificePos); //Move to Orifice

                            CurrentStep++;
                        }


                    }
                    else
                    {
                        Form1_0.method_1("Duriel location not detected!", Color.Red);
                        Form1_0.Town_0.UseLastTP = false;
                        ScriptDone = true;
                        return;
                    }
                }

                if (CurrentStep == 2)
                {
                    int Tryyyy = 0;
                    int StartLevel = (int) Form1_0.PlayerScan_0.levelNo;
                    while ((int) Form1_0.PlayerScan_0.levelNo == StartLevel && Tryyyy <= 25)
                    {
                        Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, OrificePos.X, OrificePos.Y);
                        
                        Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X - 446, itemScreenPos.Y - 268);
                        Form1_0.PlayerScan_0.GetPositions();
                        Tryyyy++;
                    }

                    if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.DurielsLair)
                    {
                        CurrentStep++;
                    }
                }

                if (CurrentStep == 3)
                {
                    if ((Enums.Area)Form1_0.PlayerScan_0.levelNo != Enums.Area.DurielsLair)
                    {
                        CurrentStep--;
                        return;
                    }

                    Form1_0.WaitDelay(50);  //wait a little bit so duriel can be detected

                    if (Form1_0.Mover_0.MoveToLocation(Form1_0.PlayerScan_0.xPos - 7, Form1_0.PlayerScan_0.yPos + 7))
                    {
                        CurrentStep++;
                    }
                }

                if (CurrentStep == 4)
                {
                    if (Form1_0.PlayerScan_0.levelNo >= (int)Enums.Area.TalRashasTomb1 && Form1_0.PlayerScan_0.levelNo <= (int)Enums.Area.TalRashasTomb7)
                    {
                        CurrentStep--;
                    }

                    Form1_0.Potions_0.CanUseSkillForRegen = false;
                    Form1_0.SetGameStatus("KILLING DURIEL");
                    if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Duriel", false, 200, new List<long>()))
                    {
                        if (Form1_0.MobsStruc_0.MobsHP > 0)
                        {
                            Form1_0.Battle_0.RunBattleScriptOnThisMob("getBossName", "Duriel");
                        }
                        else
                        {
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            if (!Form1_0.ItemsStruc_0.GetItems(true)) Form1_0.WaitDelay(5);
                            Form1_0.ItemsStruc_0.GrabAllItemsForGold();
                            Form1_0.Potions_0.CanUseSkillForRegen = true;

                            Form1_0.Town_0.UseLastTP = false;
                            ScriptDone = true;
                            return;
                            //Form1_0.LeaveGame(true);
                        }
                    }
                    else
                    {
                        Form1_0.method_1("Duriel not detected!", Color.Red);

                        //baal not detected...
                        Form1_0.ItemsStruc_0.GetItems(true);
                        if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Duriel", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.ItemsStruc_0.GrabAllItemsForGold();
                        if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Duriel", false, 200, new List<long>())) return; //redetect baal?
                        Form1_0.Potions_0.CanUseSkillForRegen = true;

                        Form1_0.Town_0.UseLastTP = false;
                        ScriptDone = true;
                        return;
                        //Form1_0.LeaveGame(true);
                    }
                }
            }
        }
    }
}
