using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class MatchDistance : IComparable, IComparable<MatchDistance>
    {
        public int MatchCount;
        public double Distance;

        public MatchDistance(int matchCount, double matchDistance)
        {
            MatchCount = matchCount;
            Distance = matchDistance;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            MatchDistance other = obj as MatchDistance; // avoid double casting
            if (other == null)
            {
                throw new ArgumentException("A RatingInformation object is required for comparison.", "obj");
            }
            return this.CompareTo(other);
        }

        public int CompareTo(MatchDistance other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            if (this.MatchCount == other.MatchCount && this.Distance == other.Distance)
            {
                return 0;
            }

            if (this.MatchCount != other.MatchCount)
            {
                return this.MatchCount.CompareTo(other.MatchCount);
            }

            return this.Distance.CompareTo(other.Distance);
        }

        public static int Compare(MatchDistance left, MatchDistance right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        // Omitting Equals violates rule: OverrideMethodsOnComparableTypes.
        public override bool Equals(object obj)
        {
            MatchDistance other = obj as MatchDistance; //avoid double casting
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return this.CompareTo(other) == 0;
        }

        // Omitting getHashCode violates rule: OverrideGetHashCodeOnOverridingEquals.
        public override int GetHashCode()
        {
            return MatchCount.GetHashCode() ^ Distance.GetHashCode();
        }

        // Omitting any of the following operator overloads 
        // violates rule: OverrideMethodsOnComparableTypes.
        public static bool operator ==(MatchDistance left, MatchDistance right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(MatchDistance left, MatchDistance right)
        {
            return !(left == right);
        }

        public static bool operator <(MatchDistance left, MatchDistance right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator >(MatchDistance left, MatchDistance right)
        {
            return (Compare(left, right) > 0);
        }

        public static MatchDistance operator -(MatchDistance left, MatchDistance right)
        {
            if(left.MatchCount == right.MatchCount)
            {
                return new MatchDistance(left.MatchCount, left.Distance - right.Distance);
            }

            return new MatchDistance(left.MatchCount - right.MatchCount, 0);
        }

        public override string ToString()
        {
            return $"{MatchCount}:{Distance}";
        }
    }
}
