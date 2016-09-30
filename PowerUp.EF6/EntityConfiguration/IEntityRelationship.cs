using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public interface IEntityRelationship<T> where T : class
	{
		void Configure(EntityTypeConfiguration<T> entityConfiguration);
	}
}
