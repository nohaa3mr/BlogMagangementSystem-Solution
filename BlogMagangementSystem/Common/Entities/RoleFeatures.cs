using BlogMagangementSystem.Common.Enums;
using Mapster.Utils;

namespace BlogMagangementSystem.Common.Entities
{
    public class RoleFeatures : BaseEntity
    {
        public Role Roles { get; set; }
        public Enums.Features Features  { get; set; }

    }
}
