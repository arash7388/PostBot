using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Common;
using Repository.Data;
using Repository.Data.Enum;
using Repository.Data.Migrations;
using Repository.Entity.Domain;

namespace Repository.DAL
{
   public class UnitOfWork : IUnitOfWork
    {
        private readonly MTOContext _mtoContext;
        private bool _dispoed;
       
        private BaseRepository<User> _users;
        //private BaseRepository<Product> _products;
        //private BaseRepository<ProductCategory> _productCategories;
        private BaseRepository<Subscriber> _subscribers;
        private BaseRepository<RatingGroup> _ratingGroups;
        private BaseRepository<RatingItem> _ratingItems;
        private BaseRepository<Post> _posts;
        private BaseRepository<Tag> _tags;
        private BaseRepository<TagPost> _tagPosts;
        private BaseRepository<Link> _links;
        private BaseRepository<Personnel> _personnels;
        
        public UnitOfWork(MTOContext mtoContext)
        {
            this._mtoContext = mtoContext;
        }

        public UnitOfWork()
        {
            this._mtoContext = new MTOContextFactory().GetMTOContext();
        }

        public BaseRepository<T> BaseRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>(this._mtoContext);
        }

        public BaseRepository<User> Users
        {
            get
            {
                return this._users ?? (this._users = new BaseRepository<User>(this._mtoContext));
            }
        }

        //public BaseRepository<ProductCategory> ProductCategories
        //{
        //    get
        //    {
        //        return this._productCategories ?? (this._productCategories = new BaseRepository<ProductCategory>(this._mtoContext));
        //    }
        //}

        //public BaseRepository<Product> Products
        //{
        //    get
        //    {
        //        return this._products ?? (this._products = new BaseRepository<Product>(this._mtoContext));
        //    }
        //}

        public BaseRepository<Subscriber> Subscribers
        {
            get
            {
                return this._subscribers ?? (this._subscribers = new BaseRepository<Subscriber>(this._mtoContext));
            }
        }

        public BaseRepository<RatingGroup> RatingGroups
        {
            get
            {
                return this._ratingGroups ?? (this._ratingGroups = new BaseRepository<RatingGroup>(this._mtoContext));
            }
        }

        public BaseRepository<RatingItem> RatingItems
        {
            get
            {
                return this._ratingItems ?? (this._ratingItems = new BaseRepository<RatingItem>(this._mtoContext));
            }
        }

        public BaseRepository<Post> Posts
        {
            get
            {
                return this._posts ?? (this._posts = new BaseRepository<Post>(this._mtoContext));
            }
        }

        public BaseRepository<Tag> Tags
        {
            get
            {
                return this._tags ?? (this._tags = new BaseRepository<Tag>(this._mtoContext));
            }
        }

        public BaseRepository<TagPost> TagPosts
        {
            get
            {
                return this._tagPosts ?? (this._tagPosts = new BaseRepository<TagPost>(this._mtoContext));
            }
        }

        public BaseRepository<Link> Links
        {
            get
            {
                return this._links ?? (this._links = new BaseRepository<Link>(this._mtoContext));
            }
        }

        public BaseRepository<Personnel> Personnels
        {
            get
            {
                return this._personnels ?? (this._personnels = new BaseRepository<Personnel>(this._mtoContext));
            }
        }
        private string GetErrorText(DbEntityValidationException exception)
        {
            var errors = this.GetErrorMessage(exception.EntityValidationErrors);

            string errorText = "";

            foreach (var validationResult in errors)
            {
                errorText += validationResult.ErrorMessage + "\n";
            }

            return errorText;
        }

        public ActionResult SaveChanges()
        {
            ActionResult result = new ActionResult() { IsSuccess = false };
            try
            {
                this._mtoContext.SaveChanges();

                result = new ActionResult();
                result.IsSuccess = true;
                result.ResultCode = (int)EntityExceptionEnum.None;
                result.ResultMessage = "تغییرات با موفقیت ذخیره شد";
            }
            catch (DbEntityValidationException exception)
            {
                result.ResultCode = (int)EntityExceptionEnum.DbEntityValidationException;
                result.ResultMessage = GetErrorText(exception);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                //this.ShowErrors(exception);
                result.ResultCode = (int)EntityExceptionEnum.DbUpdateConcurrencyException;
                result.EnglishMessage = exception.Message;
            }
            catch (DbUpdateException exception)
            {
                //this.ShowErrors(exception);
                result.ResultCode = (int)EntityExceptionEnum.DbUpdateException;
                result.EnglishMessage = exception.Message;
            }

            return result;
        }
        
        private void ShowErrors(DbUpdateException exception)
        {
            string errorText = exception.Message;

            if (exception.InnerException != null)
            {
                errorText += "\n" + exception.InnerException.Message;

                if (exception.InnerException.InnerException != null)
                    errorText += "\n" + exception.InnerException.InnerException.Message;
            }

            //MessageBox.Show(errorText);
        }

        private void ShowErrors(DbUpdateConcurrencyException exception)
        {
            string errorText = exception.Message;

            if (exception.InnerException != null)
            {
                errorText += "\n" + exception.InnerException.Message;

                if (exception.InnerException.InnerException != null)
                    errorText += "\n" + exception.InnerException.InnerException.Message;
            }

            //MessageBox.Show(errorText);
        }

        private void ShowErrors(DbEntityValidationException exception)
        {
            var errors = this.GetErrorMessage(exception.EntityValidationErrors);

            string errorText = "";

            foreach (var validationResult in errors)
            {
                errorText += validationResult.ErrorMessage + "\n";
            }

            //MessageBox.Show(errorText);
        }

        private List<ValidationResult> GetErrorMessage(IEnumerable<DbEntityValidationResult> validationResults)
        {
            var validationResult = new List<ValidationResult>();

            foreach (var dbEntityValidationResult in validationResults)
            {
                return this.GetErrorMessage(dbEntityValidationResult.ValidationErrors);
            }

            return validationResult;
        }

        private List<ValidationResult> GetErrorMessage(IEnumerable<DbValidationError> validationErrors)
        {
            List<ValidationResult> errorMessages = (from validationError in validationErrors
                                                    select new ValidationResult(validationError.ErrorMessage,
                                                        new[] { validationError.PropertyName })).ToList<ValidationResult>();
            return errorMessages;
        }
        
        public void RejectChanges()
        {
            //this.MTOContext.RejectChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._dispoed)
            {
                if (disposing)
                {
                    this._mtoContext.Dispose();
                }
            }
            this._dispoed = true;
        }

        public ActionResult ExecCommand(string cmd, params object[] parameters)
        {
            ActionResult result = new ActionResult();
            try
            {
                _mtoContext.Database.ExecuteSqlCommand(cmd, parameters);
                result.IsSuccess = true;

            }
            catch (Exception ex)
            {
                result.EnglishMessage = ex.Message;
            }

            return result;
        }
    }

  
}
