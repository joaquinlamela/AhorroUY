using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using RepositoryException;
using System.Collections.Generic;
using System.Linq;


namespace BusinessLogic
{
    public class CategoryManagement : ICategoryManagement
    {
        private IRepository<Category> categoryRepository;

        public CategoryManagement(IRepository<Category> aCategoryRepository)
        {
            categoryRepository = aCategoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                List<Category> allCategories = categoryRepository.GetAll().ToList();
                return allCategories;
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(MessageExceptionBusinessLogic.ErrorNotExistCategories, e);
            }
        }
    }
}
