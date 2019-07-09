using System;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisExperimentsTests.Utils
{
    internal class CollectedIdentifierData : IEquatable<CollectedIdentifierData>
    {
        public CollectedIdentifierData(String identifier, LinePosition startPosition, LinePosition endPosition)
        {
            Identifier = identifier;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public String Identifier { get; }

        public LinePosition StartPosition { get; }

        public LinePosition EndPosition { get; }

        public Boolean Equals(CollectedIdentifierData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(Identifier, other.Identifier) && StartPosition.Equals(other.StartPosition) && EndPosition.Equals(other.EndPosition);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CollectedIdentifierData)obj);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = (Identifier != null ? Identifier.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ StartPosition.GetHashCode();
                hashCode = (hashCode * 397) ^ EndPosition.GetHashCode();
                return hashCode;
            }
        }

        public override String ToString()
        {
            return $"CollectedIdentifierData: Identifier = \"{Identifier}\", StartPosition = {StartPosition}, EndPosition = {EndPosition}";
        }
    }
}