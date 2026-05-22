using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;

        public User(string name, string email, string phoneNumber,string password,UserRole role)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException("Nama tidak boleh kosong");
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email tidak boleh kosong");
            if (string.IsNullOrEmpty(phoneNumber)) throw new ArgumentNullException("Nomor HP tidak boleh kosong");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("Password tidak boleh kosong");

            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Role tidak valid", nameof(role));

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
        }

        public void Update(string name, string email, string phoneNumber, string? password, UserRole role, bool isDeleted)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Nama tidak boleh kosong");
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email tidak boleh kosong");
            if (string.IsNullOrEmpty(phoneNumber)) throw new ArgumentNullException("Nomor HP tidak boleh kosong");
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Role tidak valid", nameof(role));

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(password))
                Password = password;

            Role = role;
            IsDeleted = isDeleted;

            SetUpdate();
        }
    }
}
