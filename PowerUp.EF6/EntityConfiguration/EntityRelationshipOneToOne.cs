using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public class EntityRelationshipOneToOne<TSourceEntity, TTargetEntity>
	{
		public Expression<Func<TSourceEntity, TTargetEntity>> Target { get; private set; }
		public Expression<Func<TTargetEntity, TSourceEntity>> ReturnToSource { get; private set; }

		public EntityRelationshipOneToOne(
			Expression<Func<TSourceEntity, TTargetEntity>> target,
			Expression<Func<TTargetEntity, TSourceEntity>> returnToSource)
		{
			Target = target;
			ReturnToSource = returnToSource;
		}
	}
}
