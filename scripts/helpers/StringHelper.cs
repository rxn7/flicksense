public static class StringHelper {
	public static string TimeStringFromSeconds(float seconds) {
		ulong ms = (ulong)(seconds * 1000f);

		ulong minutes = ms / 60000;
		ulong secondsRemainder = (ms / 1000) % 60;
		ulong millisecondsRemainder = ms % 1000;

		return $"{minutes:0}:{secondsRemainder:00}.{millisecondsRemainder:000}";
	}

	public static string Value01ToPercentString(float v) => $"{(v * 100.0f):0}%";
}
