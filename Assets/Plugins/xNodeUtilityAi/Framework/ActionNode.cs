﻿using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [NodeTint(255, 120, 120)]
    public abstract class ActionNode : Node {
        
        [Input(ShowBackingValue.Never)] public Object Data;
        [Output(connectionType: ConnectionType.Override)] public ActionNode LinkedOption;
        public int Order;
        
        public abstract void Execute(object context, object[] parameters);
        public abstract object GetContext();
        public abstract object[] GetParameters();
        
        // public Object GetData() {
            // return GetInputPort(nameof(Data)) != null ? GetInputValue<Object>(nameof(Data)) : null;
        // }

    }
}