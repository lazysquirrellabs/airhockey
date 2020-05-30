using System;
using AirHockey.Match;
using UnityEngine;

namespace AirHockey.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private PlayerController _leftPlayer;
        [SerializeField] private PlayerController _rightPlayer;
        [SerializeField] private Puck _puck;
        [SerializeField] private Transform _leftPlayerStart;
        [SerializeField] private Transform _rightPlayerStart;
        [SerializeField] private Transform _puckNeutralStart;
        [SerializeField] private Transform _puckLeftStart;
        [SerializeField] private Transform _puckRightStart;

        #endregion

        #region Public

        public void StartMatch()
        {
            RegroupAt(_puckNeutralStart);
        }

        public void Regroup(Player player)
        {
            switch (player)
            {
                case Player.LeftPlayer:
                    RegroupAt(_puckRightStart);
                    break;
                case Player.RightPlayer:
                    RegroupAt(_puckLeftStart);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(player), player, null);
            }
        }

        #endregion

        #region Private

        private void RegroupAt(Transform puckStart)
        {
            _puck.Regroup(puckStart.position);
            _leftPlayer.Regroup(_leftPlayerStart.position);
            _rightPlayer.Regroup(_rightPlayerStart.position);
        }

        #endregion
    }
}