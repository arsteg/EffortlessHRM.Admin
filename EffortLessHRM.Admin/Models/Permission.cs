using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EffortLessHRM.Admin.Models
{
    [BsonIgnoreExtraElements]
    public class Permission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Permission name is required")]
        [BsonElement("permissionName")]
        public string PermissionName { get; set; }

        [BsonElement("permissionDetails")]
        public string? PermissionDetails { get; set; }

        [Required(ErrorMessage = "Resource is required")]
        [BsonElement("resource")]
        public string Resource { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [BsonElement("action")]
        public string Action { get; set; }

        [BsonElement("uiElement")]
        public string? UiElement { get; set; }

        // Self-reference to another Permission
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("parentPermission")]
        [JsonPropertyName("parentPermission")]
        public string? ParentPermissionId { get; set; }

        // Optional navigation property if needed
        [BsonIgnore]
        public Permission? ParentPermission { get; set; }

        //// Optional: Navigation for RolePermissions
        //[BsonIgnore]
        //public List<RolePermission>? RolePermissions { get; set; }
    }
}
