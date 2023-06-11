using Fusion;
using UnityEngine;

namespace Eggacy.Network
{
    public struct NetworkedInput : INetworkInput
    {
        public Vector2 movement;
        public Vector3 orientation;
    }
}