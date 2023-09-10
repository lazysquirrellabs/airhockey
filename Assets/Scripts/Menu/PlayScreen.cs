using System;
using System.Globalization;
using AirHockey.Match;
using AirHockey.UI.Menu;
using AirHockey.UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Screen = AirHockey.UI.Screen;

namespace AirHockey.Menu
{
    /// <summary>
    /// The play/start match screen in the main menu.
    /// </summary>
    internal class PlayScreen : Screen
    {
        #region Events

        /// <summary>
        /// Invoked whenever the play/start match button has been successfully invoked.
        /// </summary>
        internal event Action<MatchSettings> OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private ModeToggleGroup _modeSelector;
        [SerializeField] private Button _startButton;
        [SerializeField] private InputField _extraInfoInput;
        [SerializeField] private Text _extraFieldLabel;
        [SerializeField] private Text _extraInfoUnit;
        [SerializeField] private MessagePopup _popup;

        #endregion

        #region Fields

        private MatchMode _matchMode;
        private uint _extraInfo;
        private bool _needsExtraInfo;
        private bool _validExtraInfo;

        #endregion

        #region Setup

        protected override void Awake()
        {
            base.Awake();
            _startButton.onClick.AddListener(HandleStart);
            _modeSelector.OnSelect += HandleModeSelect;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _startButton.onClick.RemoveListener(HandleStart);
            _modeSelector.OnSelect -= HandleModeSelect;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles a click on the "Start" button.
        /// </summary>
        private void HandleStart()
        {
            if (_needsExtraInfo && !TryGetExtraInfo())
            {
                _popup.Message = $"PROVIDE THE MODE {_matchMode.InfoName().ToUpper()} BEFORE STARTING A MATCH";
                _popup.Show();
                return;
            }

            var settings = _needsExtraInfo ? new MatchSettings(_matchMode, _extraInfo) : new MatchSettings(_matchMode);
            OnStartMatch?.Invoke(settings);

            bool TryGetExtraInfo()
            {
                var culture = CultureInfo.InvariantCulture;
                if (int.TryParse(_extraInfoInput.text, NumberStyles.Integer, culture, out var value) && value > 0)
                {
                    _extraInfo = (uint) value;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Handles the selection of a <see cref="MatchMode"/>. Invoked whenever a mode toggle is selected.
        /// </summary>
        /// <param name="matchMode">The selected mode.</param>
        /// <exception cref="NotImplementedException">Thrown whenever the given <paramref name="matchMode"/> has not
        /// implemented yet.</exception>
        private void HandleModeSelect(MatchMode matchMode)
        {
            _matchMode = matchMode;

            switch (matchMode)
            {
                case MatchMode.HighScore:
                case MatchMode.BestOfScore:
                case MatchMode.Time:
                    _extraInfoInput.gameObject.SetActive(true);
                    _extraFieldLabel.text = $"INSERT {_matchMode.InfoName().ToUpper()} HERE";
                    _extraInfoInput.text = "";
                    _extraInfoUnit.text = _matchMode.InfoUnitName().ToUpper();
                    _needsExtraInfo = true;
                    break;
                case MatchMode.Endless:
                    _extraInfoInput.gameObject.SetActive(false);
                    _needsExtraInfo = false;
                    _extraInfoUnit.text = "";
                    break;
                default:
                    throw new NotImplementedException($"Match mode not implemented: {_matchMode}");
            }
        }

        #endregion

        #region Internal
        
        /// <inheritdoc />
        internal override void Hide()
        {
            gameObject.SetActive(false);
            _popup.Hide();
            base.Hide();
        }

        #endregion
    }
}