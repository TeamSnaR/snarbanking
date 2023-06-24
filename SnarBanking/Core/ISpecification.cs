namespace SnarBanking.Core
{
    public interface ISpecification<out T>
    {
        T IsSatisfiedBy();
    }
}

