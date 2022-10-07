using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ViewModels
{
    /// <summary>
    /// Assigne les correctes valeurs aux monstres, items et joueurs,
    /// et màj la GamSession en cours
    /// </summary>
    public class GameSessionManager : MonoBehaviour
    {
        #region Mono

        private void Start()
        {
            GameSession.InitNewSession();
        }

        private void Update()
        {
            ComputeElapsedTime();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calcule le temps passé dans un dédale
        /// </summary>
        private void ComputeElapsedTime()
        {
            if (GameSession.IsActiveTimeReached)
            {
                return;
            }
            else if(GameSession.ElapsedTime >= GameSession.Settings.ActiveTimeLimit)
            {
                GameSession.IsActiveTimeReached = true;
                return;
            }
            GameSession.ElapsedTime += Time.deltaTime;
        }

        #endregion
    }
}