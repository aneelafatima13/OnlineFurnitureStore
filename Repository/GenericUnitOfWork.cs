using MvcPaging;
using OnlineFurnitureStore.DAL;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace OnlineFurnitureStore.Repository
{
    public class GenericUnitOfWork:IDisposable
    {
        private OnlineFurnitureStoreEntities DBEntity = new OnlineFurnitureStoreEntities();
        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
            return new GenericRepository<Tbl_EntityType>(DBEntity);
        }

        public void SaveChanges()
        {
            DBEntity.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DBEntity.Dispose();
                }
            }
            this.disposed = true;
        }


        public IPagedList<Tbl_Product> GetProducts(string ProductName, string Category, string Price, string Quantity, int? ActiveFlag, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value : 1;

            const int defaultPageSize = 20;

            Expression<Func<Tbl_Product, bool>> filter = null;

            #region Filters

            filter = x => (!string.IsNullOrEmpty(ProductName) ? x.ProductName.ToLower().Contains(ProductName.ToLower()) : true)
                    && (string.IsNullOrEmpty(Category) || x.Tbl_Category.CategoryName.ToLower().Contains(Category.ToLower()))
                    && (string.IsNullOrEmpty(Price) || x.Price.ToString() == Price)
                    && (string.IsNullOrEmpty(Quantity) || x.Quantity.ToString() == Quantity)
                    && ((ActiveFlag != null && ActiveFlag != -100) ? x.IsActive == (ActiveFlag == 1 ? true : false) : true);

            #endregion

            IQueryable<Tbl_Product> qry = DBEntity.Tbl_Product.AsQueryable();
            IPagedList<Tbl_Product> lst = qry.Where(filter).OrderBy(item => item.ProductId).ToPagedList(currentPageIndex, defaultPageSize);

            return lst;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

    }
}