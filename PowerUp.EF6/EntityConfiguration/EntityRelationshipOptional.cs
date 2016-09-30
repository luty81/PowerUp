using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public class EntityRelationshipOptional<TSourceEntity, TTargetEntity, TKey> : EntityRelationship<TSourceEntity, TTargetEntity, TKey>
		where TSourceEntity : class
		where TTargetEntity : class
		where TKey : struct
	{
		public EntityRelationshipOptional(
			Expression<Func<TSourceEntity, TTargetEntity>> targetEntity,
			Expression<Func<TTargetEntity, TKey>> targetKey,
			Expression<Func<TTargetEntity, ICollection<TSourceEntity>>> fromSourceToTargetNavigation = null)
				: base(targetEntity, targetKey, fromSourceToTargetNavigation) { }

		public override void Configure(EntityTypeConfiguration<TSourceEntity> entityConfiguration)
		{
			if(Navigation == null)
			{
				entityConfiguration.ConfigureFkOptionalFor(TargetEntity, TargetKey);
			}
			else
			{
				entityConfiguration.ConfigureFkOptionalWithNavigationFor(TargetEntity, Navigation, TargetKey);
			}
		}
	}
}
