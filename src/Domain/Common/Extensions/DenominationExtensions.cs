namespace Domain.Common.Extensions;

public static class DenominationExtensions
{
    public static Denomination? TryToDenomination(this string denomination)
    {
        return denomination.ToLower() switch
        {
            "usd" => Denomination.Usd,
            "mxn" => Denomination.Mxn,
            _ => null
        };
    }
}