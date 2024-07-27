$(document).ready(function ()
{
    $(document).on("click", ".modifyTask", function (event)
    {
        if (!confirm('Do you want to modify?'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
    /* */
    $(document).on("click", ".deleteTask", function (event)
    {
        if (!confirm('Do you want to delete?\nOnce deleted, the task can not be used again.'))
        {
            event.preventDefault(); // Prevent the default behavior
            return false;
        }
        return true;
    });
});