// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameMessageTypes.cs" company="">
//   
// </copyright>
// <summary>
//   The game message types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiplayerGame.Networking.Messages
{
    /// <summary>
    /// The game message types.
    /// </summary>
    public enum GameMessageTypes
    {
        /// <summary>
        /// The update tile state.
        /// </summary>
        UpdateTileState, 

        /// <summary>
        /// The add tile state.
        /// </summary>
        AddTileState,

        /// <summary>
        /// The remove from grid tile state
        /// </summary>
        RemoveFromGridState,

        /// <summary>
        /// The snap to grid tile state
        /// </summary>
        SnapToGridState,

        /// <summary>
        /// Request tile state
        /// </summary>
        RequestTileState,

        /// <summary>
        /// Request item state
        /// </summary>
        RequestItemState,

        /// <summary>
        /// Add item state
        /// </summary>
        AddItemState,

        /// <summary>
        /// Update item state
        /// </summary>
        UpdateItemState,

        /// <summary>
        /// Rotation state
        /// </summary>
        RotationValueState,

        /// <summary>
        /// Request template state
        /// </summary>
        RequestTemplateState,

        /// <summary>
        /// Add template state
        /// </summary>
        AddTemplateState,

        /// <summary>
        /// Update template state
        /// </summary>
        UpdateTemplateState
     
    }
}