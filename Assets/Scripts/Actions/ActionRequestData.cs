using System;
using Unity.Netcode;
using UnityEngine;

namespace ProjectWilson.GamePlay.Actions
{
    /// <summary>
    /// Comprehensive class that contains information needed to play back any action on the server. This is what gets sent client->server when
    /// the Action gets played, and also what gets sent server->client to broadcast the action event. Note that the OUTCOMES of the action effect
    /// don't ride along with this object when it is broadcast to clients; that information is sync'd separately, usually by NetworkVariables.
    /// </summary>
    public struct ActionRequestData : INetworkSerializable
    {
        public ActionID ActionID; //index of the action in the list of all actions in the game - a way to recover the reference to the instance at runtime
        public Vector3 Position;           //center position of skill, e.g. "ground zero" of a fireball skill.
        public Vector3 Direction;          //direction of skill, if not inferrable from the character's current facing.
        public ulong[] TargetIds;          //NetworkObjectIds of targets, or null if untargeted.
        public float Amount;               //can mean different things depending on the Action. For a ChaseAction, it will be target range the ChaseAction is trying to achieve.
        public bool ShouldQueue;           //if true, this action should queue. If false, it should clear all current actions and play immediately.
        public bool ShouldClose;           //if true, the server should synthesize a ChaseAction to close to within range of the target before playing the Action. Ignored for untargeted actions.
        public bool CancelMovement;        // if true, movement is cancelled before playing this action

        public static ActionRequestData Create(Action action) =>
        new()
        {
            ActionID = action.ActionID
        };

        /// <summary>
        /// Returns true if the ActionRequestDatas are "functionally equivalent" (not including their Queueing or Closing properties).
        /// </summary>
        public bool Compare(ref ActionRequestData rhs)
        {
            bool scalarParamsEqual = (ActionID, Position, Direction, Amount) == (rhs.ActionID, rhs.Position, rhs.Direction, rhs.Amount);
            if (!scalarParamsEqual) { return false; }

            if (TargetIds == rhs.TargetIds) { return true; } //covers case of both being null.
            if (TargetIds == null || rhs.TargetIds == null || TargetIds.Length != rhs.TargetIds.Length) { return false; }
            for (int i = 0; i < TargetIds.Length; i++)
            {
                if (TargetIds[i] != rhs.TargetIds[i]) { return false; }
            }

            return true;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ActionID);
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Direction);
            serializer.SerializeValue(ref TargetIds);            
            serializer.SerializeValue(ref Amount);
            serializer.SerializeValue(ref ShouldQueue);
            serializer.SerializeValue(ref CancelMovement);
            serializer.SerializeValue(ref ShouldClose);
        }
    }
}