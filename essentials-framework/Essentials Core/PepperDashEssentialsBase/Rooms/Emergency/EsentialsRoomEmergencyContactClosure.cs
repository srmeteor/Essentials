﻿using System;
using Crestron.SimplSharpPro;
using PepperDash.Essentials.Core.Rooms.Config;

namespace PepperDash.Essentials.Core.Rooms
{
    public class EssentialsRoomEmergencyContactClosure : EssentialsRoomEmergencyBase
    {
        EssentialsRoomBase Room;
        string Behavior;
        bool TriggerOnClose;

        public EssentialsRoomEmergencyContactClosure(string key, EssentialsRoomEmergencyConfig config, EssentialsRoomBase room) :
            base(key)
        {
            Room = room;
            var cs = Global.ControlSystem;

            if (config.Trigger.Type.Equals("contact", StringComparison.OrdinalIgnoreCase))
            {
                var portNum = (uint)config.Trigger.Number;
                if (portNum <= cs.NumberOfDigitalInputPorts)
                {
                    cs.DigitalInputPorts[portNum].Register();
                    cs.DigitalInputPorts[portNum].StateChange += EsentialsRoomEmergencyContactClosure_StateChange;
                }
            }
            Behavior = config.Behavior;
            TriggerOnClose = config.Trigger.TriggerOnClose;
        }

        void EsentialsRoomEmergencyContactClosure_StateChange(DigitalInput digitalInput, DigitalInputEventArgs args)
        {
            if (args.State && TriggerOnClose || !args.State && !TriggerOnClose)
                RunEmergencyBehavior();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RunEmergencyBehavior()
        {
            if (Behavior.Equals("shutdown"))
                Room.Shutdown();
        }
    }
}