using MelonLoader;
using UnityEngine;
using BoneLib;
using System.Collections;
using System.Threading;
[assembly: MelonInfo(typeof(Auto_Find_Test.Core), "Auto-Find Test", "1.0.0", "HL2H0", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace Auto_Find_Test
{
    public class Core : MelonMod
    {
        public Camera sasa;
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
            Hooking.OnUIRigCreated += Hooking_OnUIRigCreated;
        }

        private void Hooking_OnUIRigCreated()
        {
            //MelonLogger.Msg("smthing might happen in 10 sec");
            //Thread.Sleep(5000);
            sasa = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera").GetComponent<Camera>();
            if (sasa == null)
            {
                MelonLogger.Error("no work");
            }
            else
            {
                MelonLogger.Msg("work");
                sasa.fieldOfView = 140;
            }
        }
    }
}