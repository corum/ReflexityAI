﻿using Plugins.xNodeUtilityAi.AbstractNodes;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class UtilityNode : MiddleNode {
        
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Scale the 0 on the X axe")] 
        public int MinX;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Evaluated between Min X and Max X")] 
        public int X;
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override), Tooltip("Scale the 1 on the X axe")] 
        public int MaxX;
        [Tooltip("Evaluate the Utility Y using the X values")]
        public AnimationCurve Function = AnimationCurve.Linear(0, 0, 1, 1);
        [Tooltip("Connect to the Option Node")]
        [Output(connectionType: ConnectionType.Override)] public float UtilityY;
        
        public override object GetValue(NodePort port) {
            int minX = GetInputValue("MinX", MinX);
            int maxX = GetInputValue("MaxX", MaxX);
            int x = GetInputValue("X", X);
            if (port.fieldName == "UtilityY") {
                float scaledX = ScaleX(minX, maxX, x);
                UtilityY = Function.Evaluate(scaledX);
            }
            return UtilityY;
        }

        private float ScaleX(int MinValue, int MaxValue, int x) {
            if (MinValue - MaxValue == 0) return 0;
            if (x < MinValue) x = MinValue;
            if (x > MaxValue) x = MaxValue;
            return (float) (x - MinValue) / (MaxValue - MinValue);
        }
        
    }
}
