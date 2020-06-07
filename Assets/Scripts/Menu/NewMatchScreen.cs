using System;
using System.Globalization;
using AirHockey.Match;
using AirHockey.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Menu
{
    public class NewMatchScreen : MonoBehaviour
    {
        #region Events

        public event Action<MatchSettings> OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private ModeToggleGroup _modeSelector;
        [SerializeField] private Button _startButton;
        [SerializeField] private InputField _extraInfoInput;
        [SerializeField] private Text _extraFieldLabel;
        [SerializeField] private Text _extraInfoUnit;

        #endregion

        #region Fields

        private Mode _mode;
        private uint _extraInfo;
        private bool _needsExtraInfo;
        private bool _validExtraInfo;

        #endregion

        #region Setup

        private void Awake()
        {
            _startButton.onClick.AddListener(HandleStart);
            _modeSelector.OnSelect += HandleModeSelect;
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(HandleStart);
            _modeSelector.OnSelect -= HandleModeSelect;
        }

        #endregion

        #region Event handlers

        private void HandleStart()
        {
            if (_needsExtraInfo && !TryGetExtraInfo())
            {
                Debug.LogError("Can't start match because the mode doesn't have a value");
                return;
            }
            
            // TODO: replace by value toggle
            MatchSettings settings;
            if (_needsExtraInfo)
                settings = new MatchSettings(_modeSelector.Selected, _extraInfo);
            else
                settings = new MatchSettings(_modeSelector.Selected);
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

        private void HandleModeSelect(Mode mode)
        {
            _mode = mode;

            switch (mode)
            {
                case Mode.HighScore:
                case Mode.BestOfScore:
                case Mode.Time:
                    _extraInfoInput.gameObject.SetActive(true);
                    _extraFieldLabel.text = $"Insert {_mode.InfoName()} here";
                    _extraInfoInput.text = "";
                    _extraInfoUnit.text = _mode.InfoUnitName();
                    _needsExtraInfo = true;
                    break;
                case Mode.Endless:
                    _extraInfoInput.gameObject.SetActive(false);
                    _needsExtraInfo = false;
                    _extraInfoUnit.text = "";
                    break;
                default:
                    throw new NotImplementedException($"Match mode not implemented: {_mode}");
            }
        }

        #endregion

        #region Public

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}