using System.Diagnostics.CodeAnalysis;

namespace WebApi.DataTypes.ForRequest
{
    [ExcludeFromCodeCoverage]

    public abstract class ModelBaseForRequest<E, M>
    where E : class
    where M : ModelBaseForRequest<E, M>, new()
    {
        public static E ToEntity(M model)
        {
            if (model == null) return null;
            return model.ToEntity();
        }

        public abstract E ToEntity();
    }
}
