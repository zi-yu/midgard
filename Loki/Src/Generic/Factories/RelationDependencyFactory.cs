using System;
using System.Collections.Generic;
using System.Text;
using Loki.DataRepresentation;
using Loki.Exceptions;

namespace Loki.Generic.Factories
{
    
    public class RelationDependencyFactory
    {
        private List<string> entityNames = new List<string>();
        /// <summary>
        /// Return the representation of the entities that the parameter entity depends
        /// </summary>
        public RelationDependency create(EntityClass entity, Model model) {

            RelationDependency toReturn = new RelationDependency(entity.Name);
            if (entityNames.Contains(entity.Name))
            {
                toReturn.IsReusable = true;
            }
            else
            {
                entityNames.Add(entity.Name);
            }
            foreach (EntityField field in entity.Fields)
            {
				if( field.Mult == Multiplicity.ManyToOne /*|| field.Mult == Multiplicity.ManyToMany*/)
                {
					//RelationDependency relates = create((EntityClass)model[ GetEntityIndex(field.Type.Name, model) ], entity, model);
                    //toReturn.Dependencies.Add(relates);
					toReturn.Dependencies.Add(create((EntityClass)model[ GetEntityIndex(field.Type.Name, model) ], model));
                }
            }

            if (entity.HasParent)
            {
                foreach (EntityField field in ((EntityClass)entity.Parent).Fields)
                {
					if( field.Mult == Multiplicity.ManyToOne /*|| field.Mult == Multiplicity.ManyToMany */)
                    {
                        toReturn.Dependencies.Add(create((EntityClass)model[ GetEntityIndex(field.Type.Name, model) ], model));
                    }
                }
            }
            return toReturn;
        }

		private RelationDependency create(EntityClass entity, EntityClass parent, Model model) {

            RelationDependency toReturn = new RelationDependency(entity.Name);

            if (entityNames.Contains(entity.Name))
            {
                toReturn.IsReusable = true;
            }
            else
            {
                entityNames.Add(entity.Name);
            }

            foreach (EntityField field in entity.Fields)
            {
				if( field.Mult == Multiplicity.ManyToOne /*|| field.Mult == Multiplicity.ManyToMany*/ )
                {
					EntityClass refer = (EntityClass)model[ GetEntityIndex(field.Type.Name, model) ];
					if(refer != parent)
					{
						RelationDependency relates = create(refer, entity, model);
						toReturn.Dependencies.Add(relates);
					}
                }
            }

            if (entity.HasParent)
            {
                foreach (EntityField field in ((EntityClass)entity.Parent).Fields)
                {
					EntityClass refer = (EntityClass)model[ GetEntityIndex(field.Type.Name, model) ];
					if(refer != parent)
					{
						RelationDependency relates = create(refer, entity, model);
						toReturn.Dependencies.Add(relates);
					}
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Return the index of an entity
        /// </summary>
        private int GetEntityIndex(string entityName, Model model)
        {
            int toReturn = 0;
            foreach (Entity entity in model)
            {
                if (entity.Name == entityName)
                    return toReturn;
                ++toReturn;
            }
            throw new LokiException("Expects a dependency entity with the name " + entityName + " that was not found");
        }
    }
}
