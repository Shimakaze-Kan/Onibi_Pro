using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.Common.ValueObjects;
public sealed class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Country { get; }

    private Address(string street, string city,
        string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public static Address Create(string street, string city,
        string postalCode, string country)
    {
        return new(street, city, postalCode, country);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
        yield return Country;
    }

#pragma warning disable CS8618
    private Address() { }
#pragma warning restore CS8618
}
