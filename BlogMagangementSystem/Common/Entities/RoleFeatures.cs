using BlogMagangementSystem.Common.Enums;

namespace BlogMagangementSystem.Common.Entities
{
    public class RoleFeatures : BaseEntity
    {
        public Role Roles { get; set; }
        public Enums.Features Features  { get; set; }

    }
}
