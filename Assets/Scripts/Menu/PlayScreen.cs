using System;
using System.Globalization;
using LazySquirrelLabs.AirHockey.Localization;
using LazySquirrelLabs.AirHockey.Match;
using LazySquirrelLabs.AirHockey.UI.Menu;
using LazySquirrelLabs.AirHockey.UI.Popups;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Screen = LazySquirrelLabs.AirHockey.UI.Screen;

namespace LazySquirrelLabs.AirHockey.Menu
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
		[SerializeField] private MessagePopup _popup;
		[Header("Localization")]
		[SerializeField] private LocalizeStringEvent _extraInfoLocalizer;
		[SerializeField] private LocalizeStringEvent _unitLocalizer;
		[SerializeField] private LocalizedString _scoreLocalizationKey;
		[SerializeField] private LocalizedString _durationLocalizationKey;
		[SerializeField] private LocalizedString _pointsLocalizationKey;
		[SerializeField] private LocalizedString _minutesLocalizationKey;
		[SerializeField] private LocalizedString _provideModeInfoLocalizationKey;
		[SerializeField] private LocalizedString _endlessModeWarningLocalizationKey;

		#endregion

		#region Fields

		private const string ExtraInfoKey = "extra_info";
		private MatchMode _matchMode;
		private uint _extraInfo;
		private bool _needsExtraInfo;
		private bool _validExtraInfo;
		private LocalizedString _extraInfoVariable;

		#endregion

		#region Setup

		protected override void Awake()
		{
			base.Awake();
			_startButton.onClick.AddListener(HandleStart);
			_modeSelector.OnSelect += HandleModeSelect;
			_extraInfoVariable = _extraInfoLocalizer.GetVariable<LocalizedString>(ExtraInfoKey);
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
				var extraInfoLocalization = GetMatchModeExtraInfoLocalization(_matchMode);
				_popup.SetMessageWithVariable(_provideModeInfoLocalizationKey, extraInfoLocalization);
				_popup.Show();
				return;
			}

			// There is no "end of match" popup on endless mode, so we need to let the user know how to leave the match.
			if (_matchMode == MatchMode.Endless)
			{
				_popup.SetMessage(_endlessModeWarningLocalizationKey);
				_popup.OnAcknowledge += StartMatch;
				_popup.Show();
				return;
			}

			StartMatch();
			return;

			bool TryGetExtraInfo()
			{
				var culture = CultureInfo.InvariantCulture;

				if (int.TryParse(_extraInfoInput.text, NumberStyles.Integer, culture, out var value) && value > 0)
				{
					_extraInfo = (uint)value;
					return true;
				}

				return false;
			}

			void StartMatch()
			{
				_popup.OnAcknowledge -= StartMatch;
				var settings = _needsExtraInfo ?
					new MatchSettings(_matchMode, _extraInfo) :
					new MatchSettings(_matchMode);
				OnStartMatch?.Invoke(settings);
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
			_extraInfoInput.text = string.Empty;
			switch (matchMode)
			{
				case MatchMode.HighScore:
				case MatchMode.BestOfScore:
				case MatchMode.Time:
					_extraInfoInput.gameObject.SetActive(true);
					var extraInfoLocalization = GetMatchModeExtraInfoLocalization(matchMode);
					_extraInfoVariable.TableEntryReference = extraInfoLocalization.TableEntryReference;
					var unitLocalization = GetMatchModeUnitLocalization(matchMode);
					_unitLocalizer.Localize(unitLocalization);
					_needsExtraInfo = true;
					break;
				case MatchMode.Endless:
					_extraInfoInput.gameObject.SetActive(false);
					_needsExtraInfo = false;
					break;
				default:
					throw new NotImplementedException($"Match mode not implemented: {_matchMode}");
			}
		}

		#endregion

		#region Internal

		/// <inheritdoc/>
		internal override void Hide()
		{
			gameObject.SetActive(false);
			_popup.Hide();
			base.Hide();
		}

		#endregion

		#region Private

		/// <summary>
		/// Fetches the localization key of the extra information needed by a <paramref name="matchMode"/> to be valid.
		/// </summary>
		/// <param name="matchMode">The mode.</param>
		/// <returns>The localization key for the extra information.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <paramref name="matchMode"/>
		/// does not require extra information.</exception>
		/// <exception cref="NotImplementedException">Thrown whenever the given <paramref name="matchMode"/> has not
		/// been implemented yet.</exception>
		private LocalizedString GetMatchModeExtraInfoLocalization(MatchMode matchMode)
		{
			switch (matchMode)
			{
				case MatchMode.HighScore:
				case MatchMode.BestOfScore:
					return _scoreLocalizationKey;
				case MatchMode.Time:
					return _durationLocalizationKey;
				case MatchMode.Endless:
					const string message = "Endless mode doesn't require info.";
					throw new ArgumentOutOfRangeException(nameof(matchMode), matchMode, message);
				default:
					throw new NotImplementedException($"Mode not implemented: {matchMode}.");
			}
		}

		/// <summary>
		/// Fetches the localization key of units of the extra information needed by a <paramref name="matchMode"/>
		/// to be valid.
		/// </summary>
		/// <param name="matchMode">The mode.</param>
		/// <returns>The localization key for the extra information's unit.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <paramref name="matchMode"/>
		/// does not require extra information.</exception>
		/// <exception cref="NotImplementedException">Thrown whenever the given <paramref name="matchMode"/> has not
		/// been implemented yet.</exception>
		private LocalizedString GetMatchModeUnitLocalization(MatchMode matchMode)
		{
			switch (matchMode)
			{
				case MatchMode.HighScore:
				case MatchMode.BestOfScore:
					return _pointsLocalizationKey;
				case MatchMode.Time:
					return _minutesLocalizationKey;
				case MatchMode.Endless:
					const string message = "Endless mode doesn't require info.";
					throw new ArgumentOutOfRangeException(nameof(matchMode), matchMode, message);
				default:
					throw new NotImplementedException($"Mode not implemented: {matchMode}.");
			}
		}

		#endregion
	}
}