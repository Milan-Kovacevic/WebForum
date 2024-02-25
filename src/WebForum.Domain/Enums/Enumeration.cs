using System.Diagnostics.CodeAnalysis;

namespace WebForum.Domain.Enums;

// Implements smart enum
public abstract class Enumeration<TEnum, TKey>(TKey id, string name) : IEquatable<Enumeration<TEnum, TKey>>
    where TEnum : Enumeration<TEnum, TKey>
{
    public TKey Id { get; protected init; } = id;
    public string Name { get; protected init; } = name;

    public static IEnumerable<TEnum> GetValues()
    {
        return default;
    }

    public static TEnum? FromName(string name)
    {
        return default;
    }

    public bool Equals(Enumeration<TEnum, TKey>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Enumeration<TEnum, TKey>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Id);
    }

    public override string ToString()
    {
        return Name;
    }
}