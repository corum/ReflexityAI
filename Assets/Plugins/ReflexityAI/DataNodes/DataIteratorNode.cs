using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.ReflexityAI.DataNodes {
    [CreateNodeMenu("Reflexity/Data/Iterator")]
    public class DataIteratorNode : DataNode, ICacheable {

        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)] public List<Object> Enumerable;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;

        public int Index { get; set; }

        public Type _argumentType;
        public Type ArgumentType {
            get {
                if (_argumentType == null && _argumentTypeName != string.Empty) {
                    _argumentType = Type.GetType(_argumentTypeName);
                }
                return _argumentType;
            }
        }

        public object _cachedCurrentValue;
        public object CurrentValue {
            get {
                if (_cachedCurrentValue == null) {
                    _cachedCurrentValue = GetCollection().ElementAt(Index);
                }
                return _cachedCurrentValue;
            }
        }
        
        public int CollectionCount => GetCollection().Count();

        [SerializeField, HideInInspector] private string _argumentTypeName;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            if (to.fieldName == nameof(Enumerable) && to.node == this) {
                ClearDynamicPorts();
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Enumerable));
                if (typeof(IEnumerable).IsAssignableFrom(reflectionData.Type)) {
                    Type type = reflectionData.Type.GetGenericArguments()[0];
                    _argumentTypeName = type.AssemblyQualifiedName;
                    AddDynamicOutput(type, ConnectionType.Multiple, TypeConstraint.Inherited, type.Name);
                } else {
                    from.Disconnect(to);
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            if (port.fieldName == nameof(Enumerable) && port.node == this) {
                foreach (NodePort nodePort in GetOutputPort(ArgumentType.Name).GetConnections()) {
                    nodePort.ClearConnections();
                }
                _argumentType = null;
                _argumentTypeName = string.Empty;
            }
        }

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            } 
            else {
                if (!Application.isPlaying) return new ReflectionData(ArgumentType, null, true);
                return new ReflectionData(ArgumentType, CurrentValue, true);
            }
        }
        
        public void ClearCache() {
            _cachedCurrentValue = null;
        }

        public void ClearShortCache() {
            _cachedCurrentValue = null;
        }
        
        private IEnumerable<object> GetCollection() {
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Enumerable));
            return (IEnumerable<object>) reflectionData.Value;
        }
        
    }
}