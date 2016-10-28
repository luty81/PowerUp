using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
    public class EntityRelationshipFactory
    {
        public static IEntityRelationship<TSource> Required<TSource, TTarget, TKey>(
            Expression<Func<TSource, TTarget>> target, 
            Expression<Func<TTarget, TKey>> key,
            Expression<Func<TTarget, ICollection<TSource>>> navigation)
            where TSource : class
            where TTarget : class
            where TKey : struct            
        {
            return new EntityRelationship<TSource, TTarget, TKey>(target, key, navigation);
        }


        public static IEntityRelationship<TSource> Optional<TSource, TTarget, TKey>(
            Expression<Func<TSource, TTarget>> target,
            Expression<Func<TTarget, TKey>> key,
            Expression<Func<TTarget, ICollection<TSource>>> navigation)
            where TSource : class
            where TTarget : class
            where TKey : struct
        {
            return new EntityRelationshipOptional<TSource, TTarget, TKey>(target, key, navigation);
        }

        public static IEntityRelationship<L> ManyToMany<L, R, LKey, RKey>(
            Expression<Func<L, ICollection<R>>> leftNavigation, Expression<Func<R, ICollection<L>>> rightNavigation,
            Expression<Func<L, LKey>> leftKey, Expression<Func<R, RKey>> rightKey)
            where L : class where R : class
            where LKey : struct where RKey : struct
        {
            return 
                new EntityRelationshipManyToMany<L, R, LKey, RKey>(
                    leftNavigation, rightNavigation, leftKey, rightKey);
        }

        public static IEntityRelationship<TSource> OneToOne<TSource, TTarget>(
            Expression<Func<TSource, TTarget>> target,
            Expression<Func<TTarget, TSource>> source)
            where TSource : class
            where TTarget : class
        {
            return new EntityRelationshipOneToOne<TSource, TTarget>(target, source);
        }
    }
}
