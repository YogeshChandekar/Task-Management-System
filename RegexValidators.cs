namespace TaskManagementPortal
{
    public class RegexValidators
    {
        public const string EmailRegex = @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const string NameRegex = @"^(?=[a-zA-Z])[a-zA-Z0-9\w-_ ]{3,49}[^\W]$";

        public const string MobileRegex = @"^\d{10}$";// only 10 digits are allowed

        public const string PasswordRegex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@$%^&+?=])(?!.*[-<>.'/:;,`~}|_{)(#])(?!.*[[])(?!.*[]])(?=\S+$).{8,20}$";
    }
}
