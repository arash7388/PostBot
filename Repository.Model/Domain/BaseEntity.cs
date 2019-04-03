namespace Repository.Entity.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    
    
    public abstract class BaseEntity:IValidatableObject
    {
        public int Id { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }

        /// <summary>
        /// This property determines object status.We do not delete object physically.
        ///Status=1  : object was deleted. 
        ///Status=-1 : object is in normal state(it exists)
        ///Status=0  : object was hard coded or is a default or systematic value
        /// </summary>
        /// 
        public Int16? Status { get; set; }

        public List<ValidationResult> Errors = new List<ValidationResult>();
        
        protected BaseEntity()
        {
           Status = -1;
           InsertDateTime = DateTime.Now;
        }

        public string ErrorMessages()
        {
            var builder = new StringBuilder();
            builder.AppendLine("خطا:");
            foreach (var item in this.Errors)
            {
                builder.AppendLine(item.ErrorMessage);
            }
            return builder.ToString();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
