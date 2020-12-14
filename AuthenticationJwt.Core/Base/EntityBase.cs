namespace AuthenticationJwt.Core.Base
{
    public abstract class EntityBase<T> where T : EntityBase<T>
    {
        protected EntityBase()
        {
        }
    }
}
