using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.ViewModels.UserModel
{
    public class RegisterReqModel
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!; // Added ConfirmPassword field
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CampusId { get; set; } = null!;
        public int Phone { get; set; }
        public int Role {  get; set; }
    }
}
