// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTimer.cs" company="">
//   
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiplayerGame
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameTimer
    {
        #region Constants and Fields

        /// <summary>
        /// The stopwatch start.
        /// </summary>
        private long stopwatchStart;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTimer"/> class.
        /// </summary>
        public GameTimer()
        {
            this.Reset();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The reset.
        /// </summary>
        public void Reset()
        {
            this.stopwatchStart = this.TimeGetTime();
        }

        /// <summary>
        /// The stopwatch.
        /// </summary>
        /// <param name="ms">
        /// The ms.
        /// </param>
        /// <returns>
        /// The stopwatch.
        /// </returns>
        public bool Stopwatch(int ms)
        {
            if (this.TimeGetTime() > this.stopwatchStart + ms)
            {
                this.Reset();
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The time get time.
        /// </summary>
        /// <returns>
        /// The time get time.
        /// </returns>
        private long TimeGetTime()
        {
            return DateTime.Now.Ticks / 10000; // convert ticks to milliseconds. 10,000 ticks in 1 millisecond.
        }

        #endregion
    }
}