using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Class for UserDTO
    /// </summary>
    public class ExpandedUserDTO
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// 
        /// <value>
        /// The email.
        /// </value>
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [Required(ErrorMessage = "Second Name is Required")]
        [Display(Name = "Second Name")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<UserRolesDTO> Roles { get; set; }
    }

    /// <summary>
    /// Class for RolesDTO
    /// </summary>
    public class UserRolesDTO
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [Key]
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role is Required")]
        public string RoleName { get; set; }
    }

    /// <summary>
    /// Class for User and RoleDTO
    /// </summary>
    public class UserRoleDTO
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Role Name")]
        public string  RoleName { get; set; }
    }

    /// <summary>
    /// Class for list of Roles
    /// </summary>
    public class RoleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }
    }

    /// <summary>
    /// Class for UserAndRolesDTO
    /// </summary>
    public class UserAndRolesDTO
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        /// <value>
        /// The user role.
        /// </value>
        public List<UserRoleDTO> colUserRoleDTO { get; set; }
    }
}