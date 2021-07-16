using System.Collections.Generic;
using System.Linq;

namespace WebApi.DataTypes.ForResponse
{
   public abstract class ModelBaseForResponse<E, M>
   where E : class
   where M : ModelBaseForResponse<E, M>, new()
    {
        public static IEnumerable<M> ToModel(IEnumerable<E> entities)
        {
            List<E> entitiesInList = entities.ToList();
            List<M> toReturn = new List<M>();
            foreach (E entity in entitiesInList)
            {
                toReturn.Add(ToModel(entity));
            }

            toReturn.AsEnumerable();
            return toReturn;
        }

        public static M ToModel(E entity)
        {
            if (entity == null) return null;
            return new M().SetModel(entity);
        }
        protected abstract M SetModel(E entity);
    }
}
