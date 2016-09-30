using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public class EntityRelationship<TSourceEntity, TTargetEntity, TKey>: IEntityRelationship<TSourceEntity>
			where TSourceEntity : class
			where TTargetEntity : class
			where TKey : struct
	{
		public Expression<Func<TSourceEntity, TTargetEntity>> TargetEntity { get; set; }
		public Expression<Func<TTargetEntity, TKey>> TargetKey { get; set; }
		public Expression<Func<TTargetEntity, ICollection<TSourceEntity>>> Navigation { get; set; }

		public EntityRelationship(
			Expression<Func<TSourceEntity, TTargetEntity>> targetEntity,
			Expression<Func<TTargetEntity, TKey>> targetKey,
			Expression<Func<TTargetEntity, ICollection<TSourceEntity>>> fromSourceToTargetNavigation = null)
		{
			TargetEntity = targetEntity;
			TargetKey = targetKey;
			Navigation = fromSourceToTargetNavigation;
		}

		public virtual void Configure(EntityTypeConfiguration<TSourceEntity> entityConfiguration)
		{
			if(Navigation == null)
			{
				entityConfiguration.ConfigureFkRequiredFor(TargetEntity, TargetKey);
			}
			else
			{
				entityConfiguration.ConfigureFkRequiredWithNavigationFor(TargetEntity, Navigation, TargetKey);
			}
		}
	}
}
