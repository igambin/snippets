public static void Main()
{
	Console.WriteLine(new KFZKennzeichen().Transform("hh-rr 123"));
}

public static class RegExPatterns
{
	public static Regex KFZKennzeichen = new Regex(@"^(?<city>[a-zäöüA-ZÄÖÜ]{1,3})[ -](?<letters>[a-zA-Z]{1,2})[ -]?(?<digits>[1-9]{1}\d{0,3})$");
	public static Regex Vertragsnummer = new Regex(@"^((?<sparte>[Kk][Rr])\s*(?<nummer>\d{1,3}[A-Za-z]{3}\d{2})|(?<sparte>[A-Za-z]{1,2})\s*(?<nummer>\d{1,9}))$");
}

public class KFZKennzeichen : RegExPattern
{
	public KFZKennzeichen() : base(RegExPatterns.KFZKennzeichen, (groups) => string.Format("{0}-{1}-{2}", groups["city"], groups["letters"], groups["digits"]).ToUpper()) { }	
}


public abstract class RegExPattern
{
	public Regex Pattern { get; private set; }

	protected Func<GroupCollection, string> ReplaceInterpolation { get; set; }

	protected RegExPattern(Regex pattern, Func<GroupCollection, string> replaceInterpolation = null)
	{
		Pattern = pattern;
		ReplaceInterpolation = replaceInterpolation;
	}

	public bool IsMatch(string input) => Pattern.IsMatch(input); 

	public Match Match(string input) => !IsMatch(input) ? null : Pattern.Match(input);

	public GroupCollection MatchingGroups(string input) => (!IsMatch(input) ? null : Match(input).Groups) ?? null;

	public bool CanTransform => ReplaceInterpolation != null;

	public string Transform(string input) => CanTransform  && IsMatch(input) ? ReplaceInterpolation(MatchingGroups(input)) : string.Empty;
}
