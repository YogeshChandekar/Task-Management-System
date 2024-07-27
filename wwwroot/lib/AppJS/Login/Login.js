$(document).ready(function ()
{
    $('#loginPassword-toggle').click(function ()
    {
        let passwordInput = $('#Password');
        let passwordToggleIcon = $(this).find('i');

        if (passwordInput.attr('type') === 'password')
        {
            passwordInput.attr('type', 'text');
            passwordToggleIcon.removeClass('fa-eye-slash').addClass('fa-eye');
        } else
        {
            passwordInput.attr('type', 'password');
            passwordToggleIcon.removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });
});

if (window.history.replaceState)
{
    window.history.replaceState(null, null, window.location.href);
}