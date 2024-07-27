$(document).ready(function ()
{
    /* */
    $(document).on("click", ".approveUser", function (event)
    {
        if (!confirm('Do you want to approve user?'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
    $(document).on("click", ".unblockUser", function (event)
    {
        if (!confirm('Do you want to unlock user?'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
    $(document).on("click", ".modifyUser", function (event)
    {
        if (!confirm('Do you want to modify?'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
    $(document).on("click", ".deleteUser", function (event)
    {
        if (!confirm('Do you want to delete?\nOnce deleted, the user can not be used again.'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
    $(document).on("click", ".reactivateUser", function (event)
    {
        if (!confirm('Do you want to reactivate the user?'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
});