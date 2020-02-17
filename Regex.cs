public static void Main()
{
	Console.WriteLine(RegexLib.KfzVersicherung.KfzKennzeichen.Transform("hh-rr 123"));
}

public sealed class RegexLib
{
	public sealed class KfzVersicherung
	{
	    private static readonly Regex KfzKennzeichenExpression = new Regex(@"^(?<city>[a-zäöüA-ZÄÖÜ]{1,3})[ -](?<letters>[a-zA-Z]{1,2})[ -]?(?<digits>[1-9]{1}\d{0,3})$");
	    private static readonly Regex VertragsnummerExpression = new Regex(@"^((?<sparte>[Kk][Rr])\s*(?<nummer>\d{1,3}[A-Za-z]{3}\d{2})|(?<sparte>[A-Za-z]{1,2})\s*(?<nummer>\d{1,9}))$");

	    public static readonly RegExPattern KfzKennzeichen = new RegExPattern( KfzKennzeichenExpression, (groups) => string.Format("{0}-{1}-{2}", groups["city"], groups["letters"], groups["digits"]).ToUpper());
	    public static readonly RegExMatcher Vertragsnummer = new RegExMatcher( VertragsnummerExpression );
	}
}

public sealed class RegExPattern: RegExMatcher
	{
	private Func<GroupCollection, string> ReplaceInterpolation { get;  }

	public RegExPattern(Regex pattern, Func<GroupCollection, string> replaceInterpolation = null) : base(pattern)
	{
	    ReplaceInterpolation = replaceInterpolation;
	}
	public bool CanTransform => ReplaceInterpolation != null;

	public string Transform(string input) => CanTransform && IsMatch(input) ? ReplaceInterpolation(MatchingGroups(input)) : string.Empty;
}

public class RegExMatcher
{
	public RegExMatcher(Regex pattern)
	{
	    Pattern = pattern;
	}

	public Regex Pattern { get; private set; }

	public bool IsMatch(string input) => Pattern.IsMatch(input);

	public Match Match(string input) => !IsMatch(input) ? null : Pattern.Match(input);

	public GroupCollection MatchingGroups(string input) => (!IsMatch(input) ? null : Match(input).Groups) ?? null;
}
