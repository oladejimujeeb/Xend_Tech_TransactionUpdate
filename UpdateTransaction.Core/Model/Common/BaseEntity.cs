using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.Model.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.Now; 
        public DateTime UpdatedOn { get; set;}
    }
}
