using System.Threading;

namespace LazySquirrelLabs.AirHockey.Utils
{
	internal static class CancellationTokenExtensions
	{
		#region Internal

		/// <summary>
		/// Unifies 2 <see cref="CancellationToken"/> into a single one. The unified token will be cancelled whenever 
		/// any of the provided tokens gets cancelled.
		/// </summary>
		/// <param name="token1">The first token to be unified.</param>
		/// <param name="token2">The second token to be unified.</param>
		/// <returns>The unified token.</returns>
		internal static CancellationToken Unify(this CancellationToken token1, CancellationToken token2)
		{
			return CancellationTokenSource.CreateLinkedTokenSource(token1, token2).Token;
		}

		#endregion
	}
}